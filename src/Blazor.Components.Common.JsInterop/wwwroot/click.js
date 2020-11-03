//Create event handler
function createClickEventHandler(dotnetRef, element) {
    let eventCallback = function (e) {
        let args = convertMouseEventArgs(e);

        if (!element.contains(e.target)) {
            //console.log('outside');
            dotnetRef.invokeMethodAsync("ClickOutside", args);
        }
        else {
            //console.log('inside');
            dotnetRef.invokeMethodAsync("ClickInside", args);
        }
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

let _clickHandlerDict = [];

//Click Boundaries
export function addClickBoundariesHandler(element, dotnetRef) {
    if (!element || !dotnetRef) {
        return;
    }

    for (let i = 0; i < _clickHandlerDict.length; i++) {
        if (_clickHandlerDict[i].key === element) {
            return; //Click already registered for this element..
        }
    }

    let clickHandler = createClickEventHandler(dotnetRef, element);
    storeEventHandler(_clickHandlerDict, element, clickHandler);

    document.addEventListener("click", clickHandler);
}
export function removeClickBoundariesHandler(element) {
    if (!element) {
        return;
    }

    let eventCallback = removeAndReturnEventHandler(_clickHandlerDict, element);

    if (!eventCallback) {
        return; //No event handler found
    }

    document.removeEventListener("click", eventCallback);
}

export function dispose(elementsWithPropArray) {
    if (elementsWithPropArray) {
        for (var i = 0; i < elementsWithPropArray.length; i++) {
            removeClickBoundariesHandler(elementsWithPropArray[i]);
        }
    }
}