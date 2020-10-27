export function getBoundingClientRect(element) {
    if (element && typeof element.getBoundingClientRect === "function") {
        var rect = element.getBoundingClientRect();

        let result = {
            Bottom: rect.bottom,
            Height: rect.height,
            Left: rect.left,
            Right: rect.right,
            Top: rect.top,
            Width: rect.width,
            X: rect.x,
            Y: rect.y
        }

        return result;
    }
}