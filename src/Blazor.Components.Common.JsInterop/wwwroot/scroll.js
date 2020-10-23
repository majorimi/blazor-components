export function scrollToElement(element) {
    if (element && typeof element.scrollIntoView === "function") {
        element.scrollIntoView();
    }
}

//Use for e.g. Text area
export function scrollToEnd(element) {
    if (element && typeof element.hasOwnProperty("scrollTop")) {
        element.scrollTop = element.scrollHeight;
    }
}
export function scrollToTop(element) {
    if (element && typeof element.hasOwnProperty("scrollTop")) {
        element.scrollTop = 0;
    }
}
export function scrollTo(element, x) {
    if (element && typeof element.hasOwnProperty("scrollTop")) {
        element.scrollTop = x;
    }
}
export function getScrollPosition(element, x) {
    if (element && typeof element.hasOwnProperty("scrollHeight")) {
        return element.scrollTop;
    }
}

//Page scroll
export function scrollToPageEnd() {
    document.body.scrollTop = document.body.scrollHeight;
    document.documentElement.scrollTop = document.body.scrollHeight;
}
export function scrollToPageTop() {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
}
export function scrollToPage(x) {
    document.body.scrollTop = x;
    document.documentElement.scrollTop = x;
}

export function getPageScrollPosition() {
    return document.documentElement.scrollTop;
}