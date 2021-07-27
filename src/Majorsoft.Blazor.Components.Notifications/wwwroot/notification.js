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

export function showWithActions(id, options, serviceWorkerUrl, dotnetRef) {
    if (!id || !options || !serviceWorkerUrl) {
        return;
    }

    navigator.serviceWorker.register(serviceWorkerUrl);
    ////navigator.serviceWorker.ready.then(function (registration) {
    ////    registration.showNotification("Hello world", { body: "Here is the body!" });
    ////});

    navigator.serviceWorker.getRegistrations().then(function (registrations) {
        if (registrations) {

            if (dotnetRef) {
                self.addEventListener('notificationclick', function (event) {
                    event.notification.close();
                    console.log(event);

                    if (event.action) {
                        actionDotnetRef.invokeMethodAsync("ActionsCallback", event.action);
                    }
                    dotnetRef.invokeMethodAsync("OnClick");

                }, false);


                //self.onnotificationclick = function (event) {
                //    if (event.action) {
                //        actionDotnetRef.invokeMethodAsync("ActionsCallback", event.action);
                //    }
                //    dotnetRef.invokeMethodAsync("OnClick");
                //};
            }

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
        notification.onshow = (event) => { dotnetRef.invokeMethodAsync("OnOpen"); console.log(event); };
        notification.onclose = (event) => { dotnetRef.invokeMethodAsync("OnClose"); console.log(event); };
        notification.onerror = (event) => { dotnetRef.invokeMethodAsync("OnError"); console.log(event); };
        notification.onclick = (event) => { dotnetRef.invokeMethodAsync("OnClick"); console.log(event); };
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