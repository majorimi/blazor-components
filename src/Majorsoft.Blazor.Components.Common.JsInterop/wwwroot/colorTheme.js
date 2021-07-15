export function getColorTheme() {
    return getTheme();
}
export function addColorThemeEvent(dotnetRef, eventId) {
    if (!dotnetRef || !eventId) {
        return;
    }

    let eventHandler = createThemeEventHandler(dotnetRef);
    storeEventHandler(_colorThemekHandlerDict, eventId, eventHandler);

    window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', eventHandler);
}
export function removeColorThemeEvent(eventId) {
    if (!eventId) {
        return;
    }

    let eventCallback = removeAndReturnEventHandler(_colorThemekHandlerDict, eventId);

    if (!eventCallback) {
        return; //No event handler found
    }

    window.matchMedia('(prefers-color-scheme: dark)').removeEventListener('change', eventCallback);
}

export function dispose(eventIdArray) {
    if (eventIdArray) {
        for (var i = 0; i < eventIdArray.length; i++) {
            removeColorThemeEvent(eventIdArray[i]);
        }
    }
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
let _colorThemekHandlerDict = [];


function getTheme() {
    if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
        return 0;
    }

    return 1;
}
function createThemeEventHandler(dotnetRef) {
    let eventHandler = function () {
        var theme = getTheme();
        dotnetRef.invokeMethodAsync("BrowserThemeChanged", theme);
    }

    return eventHandler;
}