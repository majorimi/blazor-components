// Majorsoft Blazor DragAndDrop JS interop.
// Blazor's managed drag event handlers receive a read-only snapshot of the DataTransfer object, so the
// "write" side of the HTML Drag and Drop API (effectAllowed, dropEffect, setData, setDragImage) must be
// applied from native event listeners. These helpers attach those listeners to the real DOM elements.

const draggableHandlers = new WeakMap();
const dropZoneHandlers = new WeakMap();

let globalDropGuardInstalled = false;

// Suppresses the browser's default action for drops that land outside any DropZone.
// Without this a file/link dropped on the page body navigates the whole document to that
// resource, which on Blazor Server tears down the SignalR circuit (and on WASM reloads the app).
// DropZone handlers still run normally: they receive the event during bubbling, before it
// reaches window, and already call preventDefault themselves for handled drops.
function installGlobalDropGuard() {
	if (globalDropGuardInstalled || typeof window === 'undefined') {
		return;
	}
	globalDropGuardInstalled = true;

	window.addEventListener('dragover', preventDefaultOutsideZone);
	window.addEventListener('drop', preventDefaultOutsideZone);
}

function preventDefaultOutsideZone(e) {
	e.preventDefault();
}

export function registerDraggable(element, options) {
	if (!element) {
		return;
	}
	installGlobalDropGuard();
	unregisterDraggable(element);

	const handler = (e) => {
		if (!e.dataTransfer) {
			return;
		}

		if (options && options.effectAllowed) {
			e.dataTransfer.effectAllowed = options.effectAllowed;
		}

		let dataSet = false;
		if (options && options.data) {
			for (const format in options.data) {
				if (Object.prototype.hasOwnProperty.call(options.data, format)) {
					try {
						e.dataTransfer.setData(format, options.data[format]);
						dataSet = true;
					} catch {
						// Some formats are restricted by the browser; ignore failures.
					}
				}
			}
		}

		// Firefox (and some other browsers) will not initiate a drag unless some data is set on
		// the DataTransfer during dragstart. When no custom Data was provided set a harmless default
		// so dragging always works; the strongly-typed payload is carried via the state service.
		if (!dataSet) {
			try {
				e.dataTransfer.setData('text/plain', '');
			} catch {
				// Ignore: nothing else we can do if even the default format is rejected.
			}
		}

		if (options && options.dragImageElementId && typeof e.dataTransfer.setDragImage === 'function') {
			const image = document.getElementById(options.dragImageElementId);
			if (image) {
				e.dataTransfer.setDragImage(image, options.dragImageOffsetX || 0, options.dragImageOffsetY || 0);
			}
		}
	};

	element.addEventListener('dragstart', handler);
	draggableHandlers.set(element, handler);
}

export function unregisterDraggable(element) {
	if (element && draggableHandlers.has(element)) {
		element.removeEventListener('dragstart', draggableHandlers.get(element));
		draggableHandlers.delete(element);
	}
}

export function registerDropZone(element, options) {
	if (!element) {
		return;
	}
	installGlobalDropGuard();
	unregisterDropZone(element);

	const handler = (e) => {
		if (e.dataTransfer && options && options.dropEffect) {
			e.dataTransfer.dropEffect = options.dropEffect;
		}
	};

	element.addEventListener('dragover', handler);
	dropZoneHandlers.set(element, handler);
}

export function unregisterDropZone(element) {
	if (element && dropZoneHandlers.has(element)) {
		element.removeEventListener('dragover', dropZoneHandlers.get(element));
		dropZoneHandlers.delete(element);
	}
}
