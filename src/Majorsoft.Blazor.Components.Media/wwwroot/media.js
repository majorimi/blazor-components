/* Majorsoft.Blazor.Components.Media
   JS interop module for media capture and playback.
   Capture: getUserMedia camera/microphone streams, canvas photo snapshots, MediaRecorder
   audio/video recording and AnalyserNode based audio level metering.
   Playback: HTML <video>/<audio> element control with event callbacks to .NET.
   Recorded data stays in JS as Blob; .NET pulls it on demand as a stream (getRecordingData). */

let _nextCaptureId = 1;
const _captures = new Map();

let _nextPlayerId = 1;
const _players = new Map();

//Blob URLs created by this module (recordings, stream sources) to revoke on cleanup.
function revokeUrl(url) {
	if (url) {
		try {
			URL.revokeObjectURL(url);
		} catch (e) { }
	}
}

function errorText(e) {
	if (!e) {
		return "Unknown error";
	}
	return (e.name ? e.name + ": " : "") + (e.message ?? e.toString());
}

/* ============================ Support and devices ============================ */

export function isCaptureSupported() {
	return !!(navigator.mediaDevices && navigator.mediaDevices.getUserMedia);
}

export function isMediaRecorderSupported() {
	return typeof MediaRecorder !== "undefined";
}

export async function getDevices() {
	if (!navigator.mediaDevices || !navigator.mediaDevices.enumerateDevices) {
		return [];
	}

	const devices = await navigator.mediaDevices.enumerateDevices();
	return devices.map(d => ({
		deviceId: d.deviceId,
		groupId: d.groupId,
		kind: d.kind,
		label: d.label
	}));
}

//Requests user permission by opening (then immediately closing) a media stream.
//Returns null on success, otherwise the error message. Makes device labels available.
export async function requestPermissions(audio, video) {
	try {
		const stream = await navigator.mediaDevices.getUserMedia({ audio: audio, video: video });
		stream.getTracks().forEach(t => t.stop());
		return null;
	} catch (e) {
		return errorText(e);
	}
}

const _videoMimeCandidates = [
	"video/webm;codecs=vp9,opus",
	"video/webm;codecs=vp8,opus",
	"video/webm",
	"video/mp4;codecs=avc1.42E01E,mp4a.40.2",
	"video/mp4"
];
const _audioMimeCandidates = [
	"audio/webm;codecs=opus",
	"audio/webm",
	"audio/mp4;codecs=mp4a.40.2",
	"audio/mp4",
	"audio/ogg;codecs=opus"
];

export function getSupportedMimeTypes(video) {
	if (!isMediaRecorderSupported()) {
		return [];
	}
	const candidates = video ? _videoMimeCandidates : _audioMimeCandidates;
	return candidates.filter(t => MediaRecorder.isTypeSupported(t));
}

/* ============================ Capture (camera/microphone) ============================ */

function getCapture(id) {
	return _captures.get(id);
}

//Starts a camera and/or microphone stream. videoElement is optional (audio only capture).
//options: { video, audio, videoDeviceId, audioDeviceId, facingMode, width, height }
//Returns { id, error }; id is 0 when the stream could not be opened.
export async function startCapture(videoElement, dotnetRef, options) {
	if (!isCaptureSupported()) {
		return { id: 0, error: "getUserMedia is not supported by this browser." };
	}

	const constraints = { video: false, audio: false };
	if (options.video) {
		const video = {};
		if (options.videoDeviceId) {
			video.deviceId = { exact: options.videoDeviceId };
		}
		else if (options.facingMode) {
			video.facingMode = options.facingMode;
		}
		if (options.width > 0) {
			video.width = { ideal: options.width };
		}
		if (options.height > 0) {
			video.height = { ideal: options.height };
		}
		constraints.video = video;
	}
	if (options.audio) {
		constraints.audio = options.audioDeviceId ? { deviceId: { exact: options.audioDeviceId } } : true;
	}

	try {
		const stream = await navigator.mediaDevices.getUserMedia(constraints);
		const id = _nextCaptureId++;
		const inst = {
			id: id,
			stream: stream,
			video: videoElement ?? null,
			dotnetRef: dotnetRef,
			recorder: null,
			chunks: [],
			recordingStart: 0,
			recording: null, //{ blob, url, mimeType, sizeBytes, durationMs }
			photo: null, //{ blob, url }
			audioCtx: null,
			analyser: null,
			levelTimer: 0
		};
		_captures.set(id, inst);

		//Stream can end externally (e.g. user revokes permission, device unplugged).
		stream.getTracks().forEach(t => t.onended = () => {
			if (_captures.has(id)) {
				dotnetRef.invokeMethodAsync("CaptureErrorAsync", "Media stream track ended: " + t.kind);
			}
		});

		if (videoElement) {
			videoElement.srcObject = stream;
			await videoElement.play();
		}
		return { id: id, error: null };
	} catch (e) {
		return { id: 0, error: errorText(e) };
	}
}

export function stopCapture(id) {
	const inst = getCapture(id);
	if (!inst) {
		return;
	}
	_captures.delete(id);

	stopLevelMeter(id, inst);
	if (inst.recorder && inst.recorder.state !== "inactive") {
		inst.recorder.onstop = null;
		inst.recorder.ondataavailable = null;
		try {
			inst.recorder.stop();
		} catch (e) { }
	}
	inst.stream.getTracks().forEach(t => t.stop());
	if (inst.video) {
		inst.video.srcObject = null;
	}
	if (inst.recording) {
		revokeUrl(inst.recording.url);
		inst.recording = null;
	}
	if (inst.photo) {
		revokeUrl(inst.photo.url);
		inst.photo = null;
	}
}

//Renders a video frame to a canvas and returns photo metadata; the image data stays in JS as Blob
//(Blob URL for display, bytes via the matching get*Data function) to avoid large interop messages.
function captureFrameFromElement(videoElement, options, setBlob) {
	if (!videoElement || videoElement.videoWidth === 0) {
		return Promise.resolve(null);
	}

	const canvas = document.createElement("canvas");
	canvas.width = videoElement.videoWidth;
	canvas.height = videoElement.videoHeight;
	canvas.getContext("2d").drawImage(videoElement, 0, 0);

	return new Promise(resolve => {
		canvas.toBlob(blob => {
			if (!blob) {
				resolve(null);
				return;
			}
			const url = setBlob(blob);
			resolve({
				url: url,
				width: canvas.width,
				height: canvas.height,
				mimeType: blob.type,
				sizeBytes: blob.size
			});
		}, options.mimeType ?? "image/png", options.quality ?? undefined);
	});
}

//Takes a photo of the current camera preview frame. options: { mimeType, quality }.
export function takePhoto(id, options) {
	const inst = getCapture(id);
	if (!inst) {
		return null;
	}
	return captureFrameFromElement(inst.video, options, blob => {
		if (inst.photo) {
			revokeUrl(inst.photo.url);
		}
		inst.photo = { blob: blob, url: URL.createObjectURL(blob) };
		return inst.photo.url;
	});
}

//Returns the last photo as bytes for .NET stream download (IJSStreamReference).
export async function getPhotoData(id) {
	const inst = getCapture(id);
	if (!inst || !inst.photo) {
		return null;
	}
	return new Uint8Array(await inst.photo.blob.arrayBuffer());
}

//options: { mimeType, videoBitsPerSecond, audioBitsPerSecond, timeSliceMs }
//Returns { error, mimeType }.
export function startRecording(id, options) {
	const inst = getCapture(id);
	if (!inst) {
		return { error: "Capture is not started.", mimeType: null };
	}
	if (!isMediaRecorderSupported()) {
		return { error: "MediaRecorder is not supported by this browser.", mimeType: null };
	}
	if (inst.recorder && inst.recorder.state !== "inactive") {
		return { error: "Recording is already running.", mimeType: inst.recorder.mimeType };
	}

	let mimeType = options.mimeType;
	if (mimeType && !MediaRecorder.isTypeSupported(mimeType)) {
		mimeType = null;
	}
	if (!mimeType) {
		const hasVideo = inst.stream.getVideoTracks().length > 0;
		mimeType = getSupportedMimeTypes(hasVideo)[0] ?? null;
	}

	const recorderOptions = {};
	if (mimeType) {
		recorderOptions.mimeType = mimeType;
	}
	if (options.videoBitsPerSecond > 0) {
		recorderOptions.videoBitsPerSecond = options.videoBitsPerSecond;
	}
	if (options.audioBitsPerSecond > 0) {
		recorderOptions.audioBitsPerSecond = options.audioBitsPerSecond;
	}

	try {
		if (inst.recording) {
			revokeUrl(inst.recording.url);
			inst.recording = null;
		}
		inst.chunks = [];

		const recorder = new MediaRecorder(inst.stream, recorderOptions);
		inst.recorder = recorder;
		inst.recordingStart = performance.now();

		recorder.ondataavailable = e => {
			if (e.data && e.data.size > 0) {
				inst.chunks.push(e.data);
			}
		};
		recorder.onstop = () => {
			const blob = new Blob(inst.chunks, { type: recorder.mimeType || mimeType || "" });
			inst.chunks = [];
			inst.recording = {
				blob: blob,
				url: URL.createObjectURL(blob),
				mimeType: blob.type,
				sizeBytes: blob.size,
				durationMs: performance.now() - inst.recordingStart
			};
			inst.dotnetRef.invokeMethodAsync("RecordingFinishedAsync", {
				url: inst.recording.url,
				mimeType: inst.recording.mimeType,
				sizeBytes: inst.recording.sizeBytes,
				durationMs: inst.recording.durationMs
			});
		};
		recorder.onstart = () => inst.dotnetRef.invokeMethodAsync("RecordingStateChangedAsync", "recording");
		recorder.onpause = () => inst.dotnetRef.invokeMethodAsync("RecordingStateChangedAsync", "paused");
		recorder.onresume = () => inst.dotnetRef.invokeMethodAsync("RecordingStateChangedAsync", "recording");
		recorder.onerror = e => inst.dotnetRef.invokeMethodAsync("CaptureErrorAsync", "MediaRecorder error: " + errorText(e.error ?? e));

		recorder.start(options.timeSliceMs > 0 ? options.timeSliceMs : undefined);
		return { error: null, mimeType: recorder.mimeType || mimeType };
	} catch (e) {
		inst.recorder = null;
		return { error: errorText(e), mimeType: null };
	}
}

export function pauseRecording(id) {
	const inst = getCapture(id);
	if (inst && inst.recorder && inst.recorder.state === "recording") {
		inst.recorder.pause();
	}
}

export function resumeRecording(id) {
	const inst = getCapture(id);
	if (inst && inst.recorder && inst.recorder.state === "paused") {
		inst.recorder.resume();
	}
}

export function stopRecording(id) {
	const inst = getCapture(id);
	if (inst && inst.recorder && inst.recorder.state !== "inactive") {
		inst.recorder.stop();
	}
}

export function getRecordingState(id) {
	const inst = getCapture(id);
	return inst && inst.recorder ? inst.recorder.state : "inactive";
}

//Returns the last finished recording as bytes for .NET stream download (IJSStreamReference).
export async function getRecordingData(id) {
	const inst = getCapture(id);
	if (!inst || !inst.recording) {
		return null;
	}
	return new Uint8Array(await inst.recording.blob.arrayBuffer());
}

//Audio level metering with an AnalyserNode; reports RMS level 0..1 to .NET on a timer.
export function startLevelMeter(id, intervalMs) {
	const inst = getCapture(id);
	if (!inst || inst.levelTimer || inst.stream.getAudioTracks().length === 0) {
		return;
	}

	const AudioContextType = window.AudioContext ?? window.webkitAudioContext;
	if (!AudioContextType) {
		return;
	}

	inst.audioCtx = new AudioContextType();
	if (inst.audioCtx.state === "suspended") {
		//Autoplay policy can create the context suspended; resume is allowed here since
		//the meter is started from a user gesture (opening the microphone).
		inst.audioCtx.resume();
	}
	const source = inst.audioCtx.createMediaStreamSource(inst.stream);
	inst.analyser = inst.audioCtx.createAnalyser();
	inst.analyser.fftSize = 2048;
	source.connect(inst.analyser);

	const data = new Uint8Array(inst.analyser.fftSize);
	inst.levelTimer = setInterval(() => {
		inst.analyser.getByteTimeDomainData(data);
		let sum = 0;
		for (let i = 0; i < data.length; i++) {
			const v = (data[i] - 128) / 128;
			sum += v * v;
		}
		const rms = Math.sqrt(sum / data.length);
		inst.dotnetRef.invokeMethodAsync("AudioLevelChangedAsync", Math.min(1, rms * Math.SQRT2));
	}, intervalMs > 0 ? intervalMs : 100);
}

export function stopLevelMeter(id, instance) {
	const inst = instance ?? getCapture(id);
	if (!inst) {
		return;
	}
	if (inst.levelTimer) {
		clearInterval(inst.levelTimer);
		inst.levelTimer = 0;
	}
	inst.analyser = null;
	if (inst.audioCtx) {
		try {
			inst.audioCtx.close();
		} catch (e) { }
		inst.audioCtx = null;
	}
}

/* ============================ Players (video/audio elements) ============================ */

function getPlayer(id) {
	return _players.get(id);
}

//NaN/Infinity are not JSON serializable (duration is NaN before metadata, Infinity for live streams).
function finite(value) {
	return Number.isFinite(value) ? value : 0;
}

function playerState(element) {
	return {
		currentTime: finite(element.currentTime),
		duration: finite(element.duration),
		paused: element.paused,
		ended: element.ended,
		muted: element.muted,
		volume: finite(element.volume),
		playbackRate: finite(element.playbackRate),
		readyState: element.readyState,
		isSeekable: element.seekable && element.seekable.length > 0
	};
}

const _playerEvents = ["play", "pause", "ended", "seeked", "volumechange", "ratechange", "loadedmetadata", "canplay", "waiting"];

export function initPlayer(element, dotnetRef, subscribeTimeUpdate) {
	const id = _nextPlayerId++;
	const inst = { id: id, element: element, dotnetRef: dotnetRef, handlers: [], objectUrl: null, frame: null };
	_players.set(id, inst);

	const add = (name, handler) => {
		element.addEventListener(name, handler);
		inst.handlers.push({ name: name, handler: handler });
	};

	_playerEvents.forEach(name => add(name, () => dotnetRef.invokeMethodAsync("PlayerEventAsync", name, playerState(element))));

	//timeupdate fires ~4-66x/sec, only wired when .NET subscribed to avoid interop flooding.
	if (subscribeTimeUpdate) {
		add("timeupdate", () => dotnetRef.invokeMethodAsync("PlayerEventAsync", "timeupdate", playerState(element)));
	}

	add("error", () => {
		const err = element.error;
		const message = err ? ("Media error code " + err.code + (err.message ? ": " + err.message : "")) : "Unknown media error";
		dotnetRef.invokeMethodAsync("PlayerErrorAsync", message);
	});

	return id;
}

export function disposePlayer(id) {
	const inst = getPlayer(id);
	if (!inst) {
		return;
	}
	_players.delete(id);
	inst.handlers.forEach(h => inst.element.removeEventListener(h.name, h.handler));
	revokeUrl(inst.objectUrl);
	if (inst.frame) {
		revokeUrl(inst.frame.url);
	}
}

//Play must be awaited: browsers reject it when autoplay policy blocks playback.
//Returns null on success, otherwise the error message.
export async function playerPlay(id) {
	const inst = getPlayer(id);
	if (!inst) {
		return "Player is not initialized.";
	}
	try {
		await inst.element.play();
		return null;
	} catch (e) {
		return errorText(e);
	}
}

export function playerPause(id) {
	const inst = getPlayer(id);
	if (inst) {
		inst.element.pause();
	}
}

export function playerSeek(id, seconds) {
	const inst = getPlayer(id);
	if (inst) {
		inst.element.currentTime = seconds;
	}
}

export function playerSetVolume(id, volume) {
	const inst = getPlayer(id);
	if (inst) {
		inst.element.volume = Math.min(1, Math.max(0, volume));
	}
}

export function playerSetMuted(id, muted) {
	const inst = getPlayer(id);
	if (inst) {
		inst.element.muted = muted;
	}
}

export function playerSetRate(id, rate) {
	const inst = getPlayer(id);
	if (inst) {
		inst.element.playbackRate = rate;
	}
}

export function playerGetState(id) {
	const inst = getPlayer(id);
	return inst ? playerState(inst.element) : null;
}

export function playerSetSource(id, url) {
	const inst = getPlayer(id);
	if (!inst) {
		return;
	}
	revokeUrl(inst.objectUrl);
	inst.objectUrl = null;
	inst.element.src = url ?? "";
	inst.element.load();
}

//Sets the media source from a .NET stream (DotNetStreamReference), e.g. a fresh recording or DB content.
export async function playerSetStreamSource(id, streamRef, mimeType) {
	const inst = getPlayer(id);
	if (!inst) {
		return;
	}
	const buffer = await streamRef.arrayBuffer();
	revokeUrl(inst.objectUrl);
	inst.objectUrl = URL.createObjectURL(new Blob([buffer], { type: mimeType ?? "" }));
	inst.element.src = inst.objectUrl;
	inst.element.load();
}

export async function playerRequestFullscreen(id) {
	const inst = getPlayer(id);
	if (inst && inst.element.requestFullscreen) {
		await inst.element.requestFullscreen();
	}
}

export async function playerTogglePictureInPicture(id) {
	const inst = getPlayer(id);
	if (!inst || !document.pictureInPictureEnabled) {
		return false;
	}
	if (document.pictureInPictureElement === inst.element) {
		await document.exitPictureInPicture();
		return false;
	}
	await inst.element.requestPictureInPicture();
	return true;
}

//Snapshot of the current video frame (video player only). options: { mimeType, quality }.
export function playerCaptureFrame(id, options) {
	const inst = getPlayer(id);
	if (!inst) {
		return null;
	}
	return captureFrameFromElement(inst.element, options, blob => {
		if (inst.frame) {
			revokeUrl(inst.frame.url);
		}
		inst.frame = { blob: blob, url: URL.createObjectURL(blob) };
		return inst.frame.url;
	});
}

//Returns the last captured frame as bytes for .NET stream download (IJSStreamReference).
export async function playerGetFrameData(id) {
	const inst = getPlayer(id);
	if (!inst || !inst.frame) {
		return null;
	}
	return new Uint8Array(await inst.frame.blob.arrayBuffer());
}
