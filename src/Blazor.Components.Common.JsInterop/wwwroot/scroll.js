/* Extensions */
//Element scrolled to page top
export function scrollToElement(element) {
    if (element && typeof element.scrollIntoView === "function") {
        element.scrollIntoView();
    }
}
export function scrollToElementById(id) {
    if (id) {
        scrollToElement(document.getElementById(id));
    }
}
export function scrollToElementByName(name) {
    if (name) {
        let elements = document.getElementsByName(name)
        if (elements && elements.length > 0) {
            scrollToElement(elements[0]);
        }
    }
}

let undef = "undefined";
//Element scrolled inside another element
export function scrollToElementInParent(parent, element) {
    if (parent && element && typeof parent.scrollTop !== undef && isElementHidden(element)) {
        parent.scrollTop = element.offsetHeight;
    }
}
export function scrollInParentById(parent, elementId) {
    if (parent && elementId && typeof parent.scrollTop !== undef) {
        let element = document.getElementById(elementId);

        if (element && isElementHidden(element)) {
            parent.scrollTop = element.offsetHeight;
        }
    }
}
export function scrollInParentByClass(parent, elementClass) {
    if (parent && elementClass && typeof parent.scrollTop !== undef) {
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
    if (element && typeof element.scrollTop !== undef && typeof element.scrollHeight !== undef) {
        element.scrollTop = element.scrollHeight;
    }
}
export function scrollToTop(element) {
    if (element && typeof element.scrollTop !== undef) {
        element.scrollTop = 0;
    }
}
export function scrollToX(element, x) {
    if (element && typeof element.scrollTop !== undef) {
        element.scrollTop = x;
    }
}
export function scrollToY(element, y) {
    if (element && typeof element.scrollLeft !== undef) {
        element.scrollLeft = y;
    }
}
export function getScrollXPosition(element) {
    if (element && typeof element.scrollTop !== undef) {
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
export function scrollToPageY(y) {
    document.body.scrollLeft = y;
    document.documentElement.scrollLeft = y;
}
export function getPageScrollPosition() {
    let top = window.pageYOffset || document.documentElement.scrollTop;
    let left = window.pageXOffset || document.documentElement.scrollLeft;

    let args = {
        X: top,
        Y: left
    };

    return args;
}

//HTLM page scroll events
function createScrollEventHandler(dotnetRef) {
    let eventHandler = function (event) {
        let top = window.pageYOffset || document.documentElement.scrollTop;
        let left = window.pageXOffset || document.documentElement.scrollLeft;

        let args = {
            X: top,
            Y: left
        };

        dotnetRef.invokeMethodAsync("PageScroll", args);
    }

    return eventHandler;
}

//Store eventId with event
function storeEventHandler(dict, eventId, eventCallback) {
    let elementFound = false;
    for (let i = 0; i < dict.length; i++) {
        if (dict[i].key === eventId) {
            elementFound = true;
            break;
        }
    }

    if (!elementFound) {
        dict.push({
            key: eventId,
            handler: eventCallback
        });
    }
}
//Remove eventId with event
function removeAndReturnEventHandler(dict, eventId) {
    let eventCallback;

    for (let i = 0; i < dict.length; i++) {
        if (dict[i].key === eventId) {
            eventCallback = dict[i].handler; //associate callback
            dict.splice(i, 1);
            break;
        }
    }

    return eventCallback;
}

let _scrollkHandlerDict = [];

export function addScrollEvent(dotnetRef, eventId) {
    if (!dotnetRef || !eventId ) {
        return;
    }

    let scrollHandler = createScrollEventHandler(dotnetRef);
    storeEventHandler(_scrollkHandlerDict, eventId, scrollHandler);

    window.addEventListener("scroll", scrollHandler, false);
}

export function removeScrollEvent(eventId) {
    if (!eventId) {
        return;
    }

    let eventCallback = removeAndReturnEventHandler(_scrollkHandlerDict, eventId);

    if (!eventCallback) {
        return; //No event handler found
    }

    window.removeEventListener("scroll", eventCallback, false);
}

export function dispose(eventIdArray) {
    if (eventIdArray) {
        for (var i = 0; i < eventIdArray.length; i++) {
            removeScrollEvent(eventIdArray[i]);
        }
    }
}