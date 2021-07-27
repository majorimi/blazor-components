//Global methods
export function requestPermission(dotnetRef) {
    Notification.requestPermission()
        .then(function (permission) {
            if (dotnetRef) {
                dotnetRef.invokeMethodAsync("PermissionResult", permission);
            }
        })
        .catch();
}
export function checkPermission() {
    return Notification.permission;
}
export function checkMaxActions() {
    return Notification.maxActions;
}
export function isBrowserSupported() {
    if ("Notification" in window)
        return true;
    return false;
}

export function showWithServiceWorker(options, serviceWorkerUrl) {
    if (!options || !serviceWorkerUrl) {
        return;
    }

    navigator.serviceWorker.register(serviceWorkerUrl);

    navigator.serviceWorker.getRegistrations().then(registrations => {
        if (registrations) {
            registrations[0].showNotification(options.title, options);
        }
    });
}

//Instance methods
export function showSimple(id, options, dotnetRef) {
    if (!id || !options) {
        return;
    }

    let notification = new Notification(options.title, options);

    if (dotnetRef) {
        notification.onshow = (event) => { dotnetRef.invokeMethodAsync("OnOpen"); };
        notification.onclose = (event) => { dotnetRef.invokeMethodAsync("OnClose"); };
        notification.onerror = (event) => { dotnetRef.invokeMethodAsync("OnError"); };
        notification.onclick = (event) => { dotnetRef.invokeMethodAsync("OnClick"); };
    }

    return notification;
}