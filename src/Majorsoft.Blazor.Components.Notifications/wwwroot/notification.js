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
export function show(id, title, options, actionDotnetRef) {
    if (!id || !options) {
        return;
    }

    return new Notification(title);


    //navigator.serviceWorker.register('_content/Majorsoft.Blazor.Components.Notifications/sw.js');
    //navigator.serviceWorker.ready.then(function (registration) {
    //    registration.showNotification("Hello world", { body: "Here is the body!" });
    //});


    //navigator.serviceWorker.getRegistrations().then(function (registrations) {
    //    registrations[0].showNotification(title, { body: "Here is the body!" });
    //});

    //let notification = new Notification(options.title, options);

    //if (actionDotnetRef) {
    //    notification.addEventListener('notificationclick', function (event) {
    //        if (event.action) {
    //            actionDotnetRef.invokeMethodAsync("ActionsCallback", event.action);
    //        }
    //    }, false);
    //}

    //return notification;
}
export function close() {

}