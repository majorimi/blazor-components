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
export function show(id, options, dotnetRef) {
    if (!id || !options) {
        return;
    }

    let notification = new Notification(options.title, options);

    if (dotnetRef) {
        //notification.addEventListener('notificationclick', function (event) {
        //    if (event.action) {
        //        actionDotnetRef.invokeMethodAsync("ActionsCallback", event.action);
        //    }
        //}, false);

        notification.onshow = (event) => { dotnetRef.invokeMethodAsync("OnOpen"); };
        notification.onclose = (event) => { dotnetRef.invokeMethodAsync("OnClose"); };
        notification.onerror = (event) => { dotnetRef.invokeMethodAsync("OnError"); };
        notification.onclick = (event) => {
            if (event.action) {
                actionDotnetRef.invokeMethodAsync("ActionsCallback", event.action);
            }

            dotnetRef.invokeMethodAsync("OnClick");
        };
    }
    //TODO: store notification

    return notification;
}
export function close(id) {
    if (!id) {
        return;
    }

    //TODO: close and remove notification
}

export function dispose() {

}