//Create event handler
function createResizeEventHandler(dotnetRef, element) {
    let eventCallback = function () {
        var rect = element.getBoundingClientRect();
        let args = {
            Height: rect.height,
            Width: rect.width
        };

        dotnetRef.invokeMethodAsync("ResizeEvent", args);
    }

    return eventCallback;
}

//Store element with event
function storeObserver(dict, element, observer) {
    let elementFound = false;
    for (let i = 0; i < dict.length; i++) {
        if (dict[i].key === element) {
            elementFound = true;
            break;
        }
    }

    if (!elementFound) {
        dict.push({
            key: element,
            handler: observer
        });
    }
}
//Remove element with event
function removeAndReturnObserver(dict, element) {
    let observer;

    for (let i = 0; i < dict.length; i++) {
        if (dict[i].key === element) {
            observer = dict[i].handler; //associate callback
            dict.splice(i, 1);
            break;
        }
    }

    return observer;
}

let _resizeEventDict = [];

//Element resize
export function addResizeEventHandler(element, dotnetRef) {
    if (!element || !dotnetRef) {
        return;
    }

    for (let i = 0; i < _resizeEventDict.length; i++) {
        if (_resizeEventDict[i].key === element) {
            return; //Click already registered for this element..
        }
    }

    let resizeHandler = createResizeEventHandler(dotnetRef, element);

    let observer = new ResizeObserver(resizeHandler);
    observer.observe(element);

    storeObserver(_resizeEventDict, element, observer);
}

export function removeResizeEventHandler(element) {
    if (!element) {
        return;
    }

    let observer = removeAndReturnObserver(_resizeEventDict, element);

    if (!observer) {
        return; //No event handler found
    }

    observer.unobserve(element);
    observer.disconnect();
}

export function dispose(elementsWithPropArray) {
    if (elementsWithPropArray) {
        for (var i = 0; i < elementsWithPropArray.length; i++) {
            removeResizeEventHandler(elementsWithPropArray[i]);
        }
    }
}

///////////////////////////////////////////////////////////////////////////

//HTLM page resize events
function createWindowResizeEventHandler(dotnetRef) {
    let eventHandler = function () {
        dotnetRef.invokeMethodAsync("ResizeEvent", getPageSize());
    }

    return eventHandler;
}

//Store eventId with event
function storeWindowEventHandler(dict, eventId, eventCallback) {
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
function removeAndReturnWindowEventHandler(dict, eventId) {
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

let _resizekHandlerDict = [];

export function addGlobalResizeEvent(dotnetRef, eventId) {
    if (!dotnetRef || !eventId) {
        return;
    }

    let resizeHandler = createWindowResizeEventHandler(dotnetRef);
    storeWindowEventHandler(_resizekHandlerDict, eventId, resizeHandler);

    window.addEventListener("resize", resizeHandler);
}

export function removeGlobalResizeEvent(eventId) {
    if (!eventId) {
        return;
    }

    let eventCallback = removeAndReturnWindowEventHandler(_resizekHandlerDict, eventId);

    if (!eventCallback) {
        return; //No event handler found
    }

    window.removeEventListener("resize", eventCallback);
}

export function disposeGlobal(eventIdArray) {
    if (eventIdArray) {
        for (var i = 0; i < eventIdArray.length; i++) {
            removeGlobalResizeEvent(eventIdArray[i]);
        }
    }
}

///////////////////////////////////////////////////////////////////////////
//HTLM page size
export function getPageSize() {
    let args = {
        Height: window.innerHeight,
        Width: window.innerWidth
    }

    return args;
}
export function getScreenSize() {
    let args = {
        Height: window.screen.height,
        Width: window.screen.width
    }

    return args;
}