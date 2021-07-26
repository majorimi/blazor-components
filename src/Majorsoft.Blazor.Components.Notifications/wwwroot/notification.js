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

//Instance methods
export function show(id, options, actionDotnetRef) {
    if (!id || !options) {
        return;
    }

    let notification = new Notification(options.title, options);

    if (actionDotnetRef) {
        notification.addEventListener('notificationclick', function (event) {
            if (event.action) {
                actionDotnetRef.invokeMethodAsync("ActionsCallback", event.action);
            }
        }, false);
    }

    return notification;
}
export function close() {

}