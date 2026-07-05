// Returns true when the browser supports the native EyeDropper API.
export function isEyeDropperSupported() {
    return typeof window !== "undefined" && "EyeDropper" in window;
}

// Opens the native browser color picker (EyeDropper API) and returns the picked
// color as a HEX string (e.g. "#ff8800") or null when not supported / cancelled.
export async function openEyeDropper() {
    if (!isEyeDropperSupported()) {
        return null;
    }

    try {
        const eyeDropper = new window.EyeDropper();
        const result = await eyeDropper.open();
        return result ? result.sRGBHex : null;
    } catch (e) {
        // User pressed Escape or denied the request.
        return null;
    }
}
