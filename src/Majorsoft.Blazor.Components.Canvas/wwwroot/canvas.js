/* Majorsoft.Blazor.Components.Canvas
   Generic dispatcher over CanvasRenderingContext2D and WebGL(2)RenderingContext.
   Non-serializable JS objects (gradients, patterns, WebGL buffers/shaders/programs/textures,
   uniform locations, ...) are kept in a per-context handle map and travel to .NET as { __h: n }.
   Typed array arguments travel from .NET as { __f32|__i32|__u16|__u8: [...] } markers. */

let _nextId = 1;
const _instances = new Map();

function getInstance(id) {
	return _instances.get(id);
}

//Converts a .NET argument to the real JS value (handle refs and typed array markers).
function revive(inst, arg) {
	if (arg !== null && typeof arg === "object" && !Array.isArray(arg) && !(arg instanceof Uint8Array)) {
		if ("__h" in arg) {
			return inst.handles.get(arg.__h) ?? null;
		}
		if ("__f32" in arg) {
			return new Float32Array(arg.__f32);
		}
		if ("__i32" in arg) {
			return new Int32Array(arg.__i32);
		}
		if ("__u16" in arg) {
			return new Uint16Array(arg.__u16);
		}
		if ("__u8" in arg) {
			return new Uint8Array(arg.__u8);
		}
	}
	return arg;
}

//Converts a JS result to something serializable for .NET. Objects become handles.
function marshal(inst, res) {
	if (res === undefined || res === null) {
		return null;
	}
	const t = typeof res;
	if (t === "number" || t === "string" || t === "boolean") {
		return res;
	}
	if (Array.isArray(res) || res instanceof Float32Array || res instanceof Int32Array) {
		return Array.from(res);
	}
	const h = inst.nextHandle++;
	inst.handles.set(h, res);
	return { __h: h };
}

//Creates a rendering context on the given canvas. Returns instance id, 0 when not supported.
export function createContext(canvas, contextType, contextAttributes) {
	if (!canvas || !canvas.getContext) {
		return 0;
	}
	const ctx = canvas.getContext(contextType, contextAttributes ?? undefined);
	if (!ctx) {
		return 0;
	}

	//WebGL 1.0: polyfill instancing and vertex array objects from extensions so the .NET API is uniform.
	if (contextType === "webgl") {
		const instancing = ctx.getExtension("ANGLE_instanced_arrays");
		if (instancing && !ctx.drawArraysInstanced) {
			ctx.drawArraysInstanced = (mode, first, count, instances) => instancing.drawArraysInstancedANGLE(mode, first, count, instances);
			ctx.drawElementsInstanced = (mode, count, type, offset, instances) => instancing.drawElementsInstancedANGLE(mode, count, type, offset, instances);
			ctx.vertexAttribDivisor = (index, divisor) => instancing.vertexAttribDivisorANGLE(index, divisor);
		}
		const vao = ctx.getExtension("OES_vertex_array_object");
		if (vao && !ctx.createVertexArray) {
			ctx.createVertexArray = () => vao.createVertexArrayOES();
			ctx.bindVertexArray = (vertexArray) => vao.bindVertexArrayOES(vertexArray);
			ctx.deleteVertexArray = (vertexArray) => vao.deleteVertexArrayOES(vertexArray);
		}
	}

	const id = _nextId++;
	_instances.set(id, {
		canvas: canvas,
		ctx: ctx,
		handles: new Map(),
		nextHandle: 1,
		loop: null
	});
	return id;
}

//Calls a context method: call(id, "fillRect", [0, 0, 10, 10]).
export function call(id, method, args) {
	const inst = getInstance(id);
	if (!inst) {
		return null;
	}
	const res = inst.ctx[method](...args.map(a => revive(inst, a)));
	return marshal(inst, res);
}

//Sets a context property: setProperty(id, "fillStyle", "red").
export function setProperty(id, property, value) {
	const inst = getInstance(id);
	if (inst) {
		inst.ctx[property] = revive(inst, value);
	}
}

//Reads a context property.
export function getProperty(id, property) {
	const inst = getInstance(id);
	return inst ? marshal(inst, inst.ctx[property]) : null;
}

//Executes many operations in one interop call. Op: { t: "c", m: method, a: args } or { t: "s", p: property, v: value }.
export function batch(id, ops) {
	const inst = getInstance(id);
	if (!inst) {
		return;
	}
	for (const op of ops) {
		if (op.t === "c") {
			inst.ctx[op.m](...op.a.map(a => revive(inst, a)));
		}
		else {
			inst.ctx[op.p] = revive(inst, op.v);
		}
	}
}

//Calls a method on a handled object instead of the context, e.g. gradient.addColorStop(...).
export function callHandle(id, handle, method, args) {
	const inst = getInstance(id);
	if (!inst) {
		return null;
	}
	const target = inst.handles.get(handle);
	if (!target) {
		return null;
	}
	const res = target[method](...args.map(a => revive(inst, a)));
	return marshal(inst, res);
}

//Releases a single object handle (buffer, gradient, etc.) from the handle map.
export function releaseHandle(id, handle) {
	const inst = getInstance(id);
	if (inst) {
		inst.handles.delete(handle);
	}
}

//Returns the drawing buffer size of the canvas in pixels.
export function getSize(id) {
	const inst = getInstance(id);
	return inst ? { width: inst.canvas.width, height: inst.canvas.height } : null;
}

//Syncs the drawing buffer size to the CSS layout size of the canvas element (used with % dimensions).
//Note: resizing resets 2D context state and clears the canvas; WebGL viewport is updated automatically.
export function syncSize(id) {
	const inst = getInstance(id);
	if (!inst) {
		return null;
	}
	const canvas = inst.canvas;
	const width = canvas.clientWidth;
	const height = canvas.clientHeight;

	if (width > 0 && height > 0 && (canvas.width !== width || canvas.height !== height)) {
		canvas.width = width;
		canvas.height = height;
		if (typeof inst.ctx.viewport === "function") {
			inst.ctx.viewport(0, 0, width, height);
		}
	}
	return { width: canvas.width, height: canvas.height };
}

//Canvas element level: PNG/JPEG snapshot of the canvas content.
export function toDataURL(id, type, quality) {
	const inst = getInstance(id);
	return inst ? inst.canvas.toDataURL(type ?? "image/png", quality ?? undefined) : null;
}

/* ---------- Canvas 2D helpers ---------- */

function loadImage(url) {
	return new Promise((resolve, reject) => {
		const img = new Image();
		img.crossOrigin = "anonymous";
		img.onload = () => resolve(img);
		img.onerror = () => reject(new Error(`Failed to load image: ${url}`));
		img.src = url;
	});
}

//Draws an image loaded from URL: dest args are the numeric drawImage() arguments after the image.
export async function drawImageUrl(id, url, args) {
	const inst = getInstance(id);
	if (inst) {
		const img = await loadImage(url);
		inst.ctx.drawImage(img, ...args);
	}
}

//Draws another element (e.g. video or other canvas by ElementReference).
export function drawImageElement(id, element, args) {
	const inst = getInstance(id);
	if (inst && element) {
		inst.ctx.drawImage(element, ...args);
	}
}

//Creates a repeating pattern from an image URL. Returns a handle.
export async function createPatternUrl(id, url, repetition) {
	const inst = getInstance(id);
	if (!inst) {
		return null;
	}
	const img = await loadImage(url);
	return marshal(inst, inst.ctx.createPattern(img, repetition));
}

//Raw RGBA bytes of a canvas region (Uint8Array maps to .NET byte[]).
export function getImageData(id, x, y, width, height) {
	const inst = getInstance(id);
	if (!inst) {
		return null;
	}
	return new Uint8Array(inst.ctx.getImageData(x, y, width, height).data.buffer);
}

//Writes raw RGBA bytes back to the canvas.
export function putImageData(id, data, width, height, dx, dy) {
	const inst = getInstance(id);
	if (inst) {
		const imageData = new ImageData(new Uint8ClampedArray(data.buffer ?? data), width, height);
		inst.ctx.putImageData(imageData, dx, dy);
	}
}

//Text width measured with the current font.
export function measureText(id, text) {
	const inst = getInstance(id);
	return inst ? inst.ctx.measureText(text).width : 0;
}

/* ---------- WebGL helpers ---------- */

//Compiles a vertex + fragment shader pair and links them into a program in one interop call.
//Throws with shader/program info log on failure. Returns a program handle.
export function compileProgram(id, vertexSource, fragmentSource) {
	const inst = getInstance(id);
	if (!inst) {
		return null;
	}
	const gl = inst.ctx;

	const compile = (type, source, name) => {
		const shader = gl.createShader(type);
		gl.shaderSource(shader, source);
		gl.compileShader(shader);
		if (!gl.getShaderParameter(shader, gl.COMPILE_STATUS)) {
			const log = gl.getShaderInfoLog(shader);
			gl.deleteShader(shader);
			throw new Error(`${name} shader compile failed: ${log}`);
		}
		return shader;
	};

	const vs = compile(gl.VERTEX_SHADER, vertexSource, "Vertex");
	const fs = compile(gl.FRAGMENT_SHADER, fragmentSource, "Fragment");

	const program = gl.createProgram();
	gl.attachShader(program, vs);
	gl.attachShader(program, fs);
	gl.linkProgram(program);
	gl.deleteShader(vs);
	gl.deleteShader(fs);

	if (!gl.getProgramParameter(program, gl.LINK_STATUS)) {
		const log = gl.getProgramInfoLog(program);
		gl.deleteProgram(program);
		throw new Error(`Program link failed: ${log}`);
	}
	return marshal(inst, program);
}

//Uploads a texture image loaded from URL to the bound texture at the given target (TEXTURE_2D or a cube map face).
export async function texImage2DUrl(id, target, url, flipY) {
	const inst = getInstance(id);
	if (!inst) {
		return;
	}
	const gl = inst.ctx;
	const img = await loadImage(url);
	gl.pixelStorei(gl.UNPACK_FLIP_Y_WEBGL, flipY ? 1 : 0);
	gl.texImage2D(target, 0, gl.RGBA, gl.RGBA, gl.UNSIGNED_BYTE, img);
}

//Updates a region of the bound texture from an image loaded from URL.
export async function texSubImage2DUrl(id, target, xoffset, yoffset, url, flipY) {
	const inst = getInstance(id);
	if (!inst) {
		return;
	}
	const gl = inst.ctx;
	const img = await loadImage(url);
	gl.pixelStorei(gl.UNPACK_FLIP_Y_WEBGL, flipY ? 1 : 0);
	gl.texSubImage2D(target, 0, xoffset, yoffset, gl.RGBA, gl.UNSIGNED_BYTE, img);
}

//Reads RGBA pixel bytes from the current framebuffer (Uint8Array maps to .NET byte[]).
export function readPixels(id, x, y, width, height) {
	const inst = getInstance(id);
	if (!inst) {
		return null;
	}
	const gl = inst.ctx;
	const buffer = new Uint8Array(width * height * 4);
	gl.readPixels(x, y, width, height, gl.RGBA, gl.UNSIGNED_BYTE, buffer);
	return buffer;
}

//Returns whether the given WebGL extension is supported and enables it.
export function hasExtension(id, name) {
	const inst = getInstance(id);
	return inst ? inst.ctx.getExtension(name) !== null : false;
}

/* ---------- Input capture ---------- */

//Starts tracking keyboard/mouse state on the canvas element. Keyboard events require the canvas to be
//focused (set a tabindex). State is read with getInputState or delivered with every render loop frame.
export function startInputCapture(id, options) {
	const inst = getInstance(id);
	if (!inst || inst.input) {
		return;
	}
	options = options ?? {};
	const canvas = inst.canvas;
	const input = { keys: new Set(), pressed: new Set(), buttons: new Set(), x: 0, y: 0, dx: 0, dy: 0, wheel: 0, path: [], options: options, handlers: [] };
	const add = (type, fn, opts) => {
		canvas.addEventListener(type, fn, opts);
		input.handlers.push([type, fn]);
	};
	const prevent = (e) => {
		if (options.preventDefault !== false) {
			e.preventDefault();
		}
	};

	if (options.keyboard !== false) {
		add("keydown", e => {
			input.keys.add(e.code);
			if (!e.repeat) {
				input.pressed.add(e.code); //Fire-once set: survives until the next snapshot even if the key was already released.
			}
			prevent(e);
		});
		add("keyup", e => { input.keys.delete(e.code); prevent(e); });
		add("blur", () => input.keys.clear()); //Do not leave keys stuck when focus is lost.
	}
	if (options.mouse !== false) {
		add("mousedown", e => input.buttons.add(e.button));
		add("mouseup", e => input.buttons.delete(e.button));
		add("mousemove", e => {
			//Mouse position in drawing buffer pixels (CSS size can differ, e.g. % dimensions).
			input.x = e.offsetX * (canvas.width / canvas.clientWidth);
			input.y = e.offsetY * (canvas.height / canvas.clientHeight);
			input.dx += e.movementX;
			input.dy += e.movementY;
			//While a button is held every position is recorded, so fast strokes lose no points
			//between two snapshots (drawing apps). Capped as a safety net.
			if (input.buttons.size > 0 && input.path.length < 2048) {
				input.path.push(input.x, input.y);
			}
		});
		add("wheel", e => { input.wheel += e.deltaY; prevent(e); }, { passive: false });
		add("contextmenu", prevent); //Right button usable as custom input.
	}

	inst.input = input;
}

//Stops input tracking and removes all listeners.
export function stopInputCapture(id) {
	const inst = getInstance(id);
	if (inst && inst.input) {
		for (const [type, fn] of inst.input.handlers) {
			inst.canvas.removeEventListener(type, fn);
		}
		inst.input = null;
	}
}

//Snapshot of the current input state; movement and wheel deltas accumulate between reads and reset on read.
function snapshotInput(inst) {
	const input = inst.input;
	if (!input) {
		return null;
	}

	const gamepads = [];
	if (input.options.gamepads !== false && navigator.getGamepads) {
		for (const gamepad of navigator.getGamepads()) {
			if (gamepad && gamepad.connected) {
				gamepads.push({
					index: gamepad.index,
					id: gamepad.id,
					axes: Array.from(gamepad.axes),
					buttons: gamepad.buttons.map(b => b.value)
				});
			}
		}
	}

	const snapshot = {
		keys: Array.from(input.keys),
		pressedKeys: Array.from(input.pressed),
		buttons: Array.from(input.buttons),
		mouseX: input.x,
		mouseY: input.y,
		movementX: input.dx,
		movementY: input.dy,
		wheelDelta: input.wheel,
		mousePath: input.path,
		isPointerLocked: document.pointerLockElement === inst.canvas,
		gamepads: gamepads
	};
	input.pressed = new Set();
	input.dx = 0;
	input.dy = 0;
	input.wheel = 0;
	input.path = [];
	return snapshot;
}

//Reads the input state on demand (outside of the render loop).
export function getInputState(id) {
	const inst = getInstance(id);
	return inst ? snapshotInput(inst) : null;
}

//Requests pointer lock on the canvas (FPS style relative mouse). Must be called from a user gesture.
export function requestPointerLock(id) {
	const inst = getInstance(id);
	if (inst && inst.canvas.requestPointerLock) {
		inst.canvas.requestPointerLock();
	}
}

//Exits pointer lock.
export function exitPointerLock() {
	if (document.exitPointerLock) {
		document.exitPointerLock();
	}
}

/* ---------- Render loop ---------- */

//Starts a requestAnimationFrame loop invoking the .NET FrameAsync(deltaSeconds, totalSeconds) callback.
//The next frame is scheduled only after .NET returned, so slow callbacks skip frames instead of flooding interop.
export function startRenderLoop(id, dotNetRef) {
	const inst = getInstance(id);
	if (!inst || inst.loop) {
		return;
	}
	const loop = { running: true, last: performance.now(), start: performance.now() };
	inst.loop = loop;

	const tick = async (now) => {
		if (!loop.running || !_instances.has(id)) {
			return;
		}
		const delta = (now - loop.last) / 1000;
		loop.last = now;
		try {
			//Input snapshot travels with the frame callback: no extra interop round trip per frame.
			await dotNetRef.invokeMethodAsync("FrameAsync", delta, (now - loop.start) / 1000, snapshotInput(inst));
		}
		catch {
			loop.running = false;
			return;
		}
		if (loop.running) {
			requestAnimationFrame(tick);
		}
	};
	requestAnimationFrame(tick);
}

//Stops the render loop of the given context.
export function stopRenderLoop(id) {
	const inst = getInstance(id);
	if (inst && inst.loop) {
		inst.loop.running = false;
		inst.loop = null;
	}
}

//Releases the context, its handles, input listeners and loop.
export function dispose(id) {
	stopRenderLoop(id);
	stopInputCapture(id);
	const inst = getInstance(id);
	if (inst) {
		inst.handles.clear();
		_instances.delete(id);
	}
}
