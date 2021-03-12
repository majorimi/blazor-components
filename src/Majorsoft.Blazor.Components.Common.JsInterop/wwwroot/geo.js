//Create event handler
function createSuccessEventHandler(dotnetRef) {
    let eventCallback = function (pos) {
        let args = {
            Coordinates: {
                Latitude: pos.coords.latitude,
                Longitude: pos.coords.longitude,
                Accuracy: pos.coords.accuracy,
                Altitude: pos.coords.altitude,
                AltitudeAccuracy: pos.coords.altitudeAccuracy,
                Speed: pos.coords.speed,
                Heading: pos.coords.heading,
            },
            Error: null,
            TimeStamp: new Date(pos.timestamp).toISOString()
        };

        dotnetRef.invokeMethodAsync("GeolocationEvent", args);
    }

    return eventCallback;
}
function createErrorEventHandler(dotnetRef) {
    let eventCallback = function (error) {
        let args = {
            Coordinates: null,
            Error: {
                ErrorCode: error.code,
                ErrorMessage: error.message
            },
            TimeStamp: new Date().toISOString()
        };

        dotnetRef.invokeMethodAsync("GeolocationEvent", args);
    };

    return eventCallback;
}

//Current location
export function getCurrentPosition(dotnetRef, highAccuracy, timeout, cacheTime) {
    if (!dotnetRef) {
        return;
    }

    let options = {
        enableHighAccuracy: highAccuracy, //false,
        timeout: timeout, //5000,
        maximumAge: cacheTime, //0
    };

    navigator.geolocation.getCurrentPosition(createSuccessEventHandler(dotnetRef), createErrorEventHandler(dotnetRef), options);
}

//Location watch
export function addGeolocationWatcher(dotnetRef, highAccuracy, timeout, cacheTime) {
    if (!dotnetRef) {
        return;
    }

    let options = {
        enableHighAccuracy: highAccuracy, //false,
        timeout: timeout, //5000,
        maximumAge: cacheTime, //0
    };

    let handler = navigator.geolocation.watchPosition(createSuccessEventHandler(dotnetRef), createErrorEventHandler(dotnetRef), options);
    return handler;
}
export function removeGeolocationWatcher(handler) {
    if (!handler) {
        return;
    }

    navigator.geolocation.clearWatch(handler);
}

export function dispose(handlerArray) {
    if (handlerArray) {
        for (var i = 0; i < handlerArray.length; i++) {
            removeGeolocationWatcher(handlerArray[i]);
        }
    }
}