
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

export function addResizeEvent(dotnetRef, eventId) {
    if (!dotnetRef || !eventId) {
        return;
    }

    let resizeHandler = createResizeEventHandler(dotnetRef);
    storeEventHandler(_resizekHandlerDict, eventId, resizeHandler);

    window.addEventListener("resize", resizeHandler);
}

export function removeResizeEvent(eventId) {
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