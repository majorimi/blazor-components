//Create event handler
function createGlobalEventHandler(dotnetRef, functionName) {
    let eventCallback = function (e) {
        let args = convertMouseEventArgs(e);

        //console.log(functionName, e);
        dotnetRef.invokeMethodAsync(functionName, args);
    }

    return eventCallback;
}
function convertMouseEventArgs(e) {
    let args = {
        Detail: e.detail,
        ScreenX: e.screenX,
        ScreenY: e.screenY,
        ClientX: e.clientX,
        ClientY: e.clientY,
        OffsetX: e.offsetX,
        OffsetY: e.offsetY,
        Button: e.button,
        Buttons: e.buttons,
        CtrlKey: e.ctrlKey,
        ShiftKey: e.shiftKey,
        AltKey: e.altKey,
        MetaKey: e.metaKey,
        Type: e.type,
    };

    return args;
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

let _globalMouseMoveDict = [];
let _globalMouseDownDict = [];
let _globalMouseUpDict = [];

//Global Mouse move
export function addGlobalMouseMoveHandler(dotnetRef, eventId) {
    if (!dotnetRef || !eventId) {
        return;
    }

    let moveHandler = createGlobalEventHandler(dotnetRef, "GlobalMouseMove");
    storeEventHandler(_globalMouseMoveDict, eventId, moveHandler);

    document.addEventListener("mousemove", moveHandler);
}
export function removeGlobalMouseMoveHandler(eventId) {
    if (!eventId) {
        return;
    }

    let eventCallback = removeAndReturnEventHandler(_globalMouseMoveDict, eventId);

    if (!eventCallback) {
        return; //No event handler found
    }

    document.removeEventListener("mousemove", eventCallback);
}

//Global Mouse down
export function addGlobalMouseDownHandler(dotnetRef, eventId) {
    if (!dotnetRef || !eventId) {
        return;
    }

    let moveHandler = createGlobalEventHandler(dotnetRef, "GlobalMouseDown");
    storeEventHandler(_globalMouseDownDict, eventId, moveHandler);

    document.addEventListener("mousedown", moveHandler);
}
export function removeGlobalMouseDownHandler(eventId) {
    if (!eventId) {
        return;
    }

    let eventCallback = removeAndReturnEventHandler(_globalMouseDownDict, eventId);

    if (!eventCallback) {
        return; //No event handler found
    }

    document.removeEventListener("mousedown", eventCallback);
}

//Global Mouse up
export function addGlobalMouseUpHandler(dotnetRef, eventId) {
    if (!dotnetRef || !eventId) {
        return;
    }

    let moveHandler = createGlobalEventHandler(dotnetRef, "GlobalMouseUp");
    storeEventHandler(_globalMouseUpDict, eventId, moveHandler);

    document.addEventListener("mouseup", moveHandler);
}
export function removeGlobalMouseUpHandler(eventId) {
    if (!eventId) {
        return;
    }

    let eventCallback = removeAndReturnEventHandler(_globalMouseUpDict, eventId);

    if (!eventCallback) {
        return; //No event handler found
    }

    document.removeEventListener("mouseup", eventCallback);
}

export function dispose(eventIdArray) {
    if (eventIdArray) {
        for (var i = 0; i < eventIdArray.length; i++) {
            //Call all remove functions, they handle correct removes...
            removeGlobalMouseMoveHandler(eventIdArray[i]);
            removeGlobalMouseDownHandler(eventIdArray[i]);
            removeGlobalMouseUpHandler(eventIdArray[i]);
        }
    }
}