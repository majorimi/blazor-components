export function focusElementById(id) {
    let element = document.getElementById(id);
    element.focus();
}

export function focusElement(element) {
    if (element && typeof element.focus === "function") {
        element.focus();
    }
}

export function getFocusedElement() {
    return document.activeElement;
}