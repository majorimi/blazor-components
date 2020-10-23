//Standard return focused element and restore from C# object
export function focusElement(element) {
    if (element && typeof element.focus === "function") {
        element.focus();
    }
}
export function getFocusedElement() {
    return document.activeElement;
}

//Store one last focused element and restore it
let focusedElement = null;
export function storeFocusedElement() {
    focusedElement = document.activeElement;
}
export function restoreStoredElementFocus(clearElementRef) {
    focusElement(focusedElement);

    if (clearElementRef) {
        focusedElement = null;
    }
}