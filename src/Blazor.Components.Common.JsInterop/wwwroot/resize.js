//Create event handler
function createClickEventHandler(dotnetRef, element) {
    let eventCallback = function (e) {
        //let args = convertMouseEventArgs(e);

        //if (!element.contains(e.target)) {
        //    //console.log('outside');
        //    dotnetRef.invokeMethodAsync("ClickOutside", args);
        //}
        //else {
        //    //console.log('inside');
        //    dotnetRef.invokeMethodAsync("ClickInside", args);
        //}
    }

    return eventCallback;
}

//Store element with event
function storeEventHandler(dict, element, eventCallback) {
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
            handler: eventCallback
        });
    }
}
//Remove element with event
function removeAndReturnEventHandler(dict, element) {
    let eventCallback;

    for (let i = 0; i < dict.length; i++) {
        if (dict[i].key === element) {
            eventCallback = dict[i].handler; //associate callback
            dict.splice(i, 1);
            break;
        }
    }

    return eventCallback;
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

    let resizeHandler = createClickEventHandler(dotnetRef, element);
    storeEventHandler(_resizeEventDict, element, resizeHandler);

    document.addEventListener("onresize", resizeHandler);
}
export function removeResizeEventHandler(element) {
    if (!element) {
        return;
    }

    let eventCallback = removeAndReturnEventHandler(_resizeEventDict, element);

    if (!eventCallback) {
        return; //No event handler found
    }

    document.removeEventListener("onresize", eventCallback);
}

export function dispose(elementsWithPropArray) {
    if (elementsWithPropArray) {
        for (var i = 0; i < elementsWithPropArray.length; i++) {
            removeClickBoundariesHandler(elementsWithPropArray[i]);
        }
    }
}


//HTLM page resize events
function createResizeEventHandler(dotnetRef) {
    let eventHandler = function () {
        let args = {
            Height: window.innerHeight,
            Width: window.innerWidth
        };

        dotnetRef.invokeMethodAsync("PageResize", args);
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

let _resizekHandlerDict = [];

export function addGlobalResizeEvent(dotnetRef, eventId) {
    if (!dotnetRef || !eventId) {
        return;
    }

    let resizeHandler = createResizeEventHandler(dotnetRef);
    storeEventHandler(_resizekHandlerDict, eventId, resizeHandler);

    window.addEventListener("resize", resizeHandler);
}

export function removeGlobalResizeEvent(eventId) {
    if (!eventId) {
        return;
    }

    let eventCallback = removeAndReturnEventHandler(_resizekHandlerDict, eventId);

    if (!eventCallback) {
        return; //No event handler found
    }

    window.removeEventListener("resize", eventCallback);
}

export function disposeGlobal(eventIdArray) {
    if (eventIdArray) {
        for (var i = 0; i < eventIdArray.length; i++) {
            removeResizeEvent(eventIdArray[i]);
        }
    }
}