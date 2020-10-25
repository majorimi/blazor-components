/* Extensions */
//Element scrolled to page top
export function scrollToElement(element) {
    if (element && typeof element.scrollIntoView === "function") {
        element.scrollIntoView();
    }
}
//Element scrolled inside another element
export function scrollToElementInParent(parent, element) {
    if (parent && element && typeof parent.hasOwnProperty("scrollTop") && isElementHidden(element)) {
        parent.scrollTop = element.offsetHeight;
    }
}
export function scrollInParentById(parent, elementId) {
    if (parent && elementId && typeof parent.hasOwnProperty("scrollTop")) {
        let element = document.getElementById(elementId);

        if (element && isElementHidden(element)) {
            parent.scrollTop = element.offsetHeight;
        }
    }
}
export function scrollInParentByClass(parent, elementClass) {
    if (parent && elementClass && typeof parent.hasOwnProperty("scrollTop")) {
        let elements = document.getElementsByClassName(elementClass);

        if (elements && elements[0]) {
            let element = elements[0];
            if (isElementHiddenBelow(element)) {
                parent.scrollTop += elements[0].offsetHeight;
                if (isElementHiddenBelow(element)) { //re-check if it still not visible
                    parent.scrollTop = element.offsetTop - parent.clientHeight + element.offsetHeight;
                }
            }
            else if (isElementHiddenAbove(element)) {
                parent.scrollTop -= elements[0].offsetHeight;
                if (isElementHiddenAbove(element)) { //re-check if it still not visible
                    parent.scrollTop = element.offsetTop;
                }
            }
        }
    }
}
export function isElementHidden(element) {
    return isElementHiddenBelow(element) || isElementHiddenAbove(element);
}
export function isElementHiddenBelow(element) {
    if (!element || !element.offsetParent) {
        return false;
    }

    return (element.offsetHeight + element.offsetTop) > (element.offsetParent.scrollTop + element.offsetParent.clientHeight);
}
export function isElementHiddenAbove(element) {
    if (!element || !element.offsetParent) {
        return false;
    }

    return (element.offsetTop) < (element.offsetParent.scrollTop);
}

//Scrolling inside an element use it for e.g. Textarea
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

/* Injectable functions */
//HTLM page scroll
export function scrollToPageEnd() {
    document.body.scrollTop = document.body.scrollHeight;
    document.documentElement.scrollTop = document.body.scrollHeight;
}
export function scrollToPageTop() {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
}
export function scrollToPageX(x) {
    document.body.scrollTop = x;
    document.documentElement.scrollTop = x;
}
export function getPageScrollXPosition() {
    return document.documentElement.scrollTop;
}

let _eventSubscribedCount = 0;
let _pageScrollEventHandler = null;
export function addScrollEvent(dotnetRef) {
    //window.addEventListener("scroll", function (event) {
    //    var top = this.scrollY,
    //        left = this.scrollX;
    //}, false);

    _eventSubscribedCount++;
    if (_pageScrollEventHandler) {
        return; //Already subscribed
    }

    createScrollEventHandler(dotnetRef);
    window.addEventListener("scroll", _pageScrollEventHandler, false);
}
export function removeScrollEvent() {
    _eventSubscribedCount--;

    if (_eventSubscribedCount <= 0) {
        window.removeEventListener("scroll", _pageScrollEventHandler, false);
        _pageScrollEventHandler = null;
    }
}
function createScrollEventHandler(dotnetRef) {
    _pageScrollEventHandler = function (event) {
        let e = {
            X: this.scrollX,
            Y: this.scrollY
        };

        dotnetRef.invokeMethodAsync("PageScroll", e);
    }
}