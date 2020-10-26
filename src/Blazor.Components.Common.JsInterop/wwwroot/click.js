//Create event handler
function createEventHandler(dotnetRef, element) {
    let eventCallback = function (e) {
        if (!element.contains(e.target)) {
            //console.log('outside');
            dotnetRef.invokeMethodAsync("ClickOutside", e);
        }
        else {
            //console.log('inside');
            dotnetRef.invokeMethodAsync("ClickInside", e);
        }
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

let _clickHandlerDict = [];

export function addClickBoundariesHandler(element, dotnetRef) {
    if (!element || !dotnetRef) {
        return;
    }

    for (let i = 0; i < _clickHandlerDict.length; i++) {
        if (_clickHandlerDict[i].key === element) {
            return; //Click already registered for this element..
        }
    }

    let clickHandler = createEventHandler(dotnetRef, element);
    storeEventHandler(_clickHandlerDict, element, clickHandler);

    document.addEventListener("click", clickHandler);
}

export function removeClickBoundariesHandler(element) {
    if (!element || !dotnetRef) {
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