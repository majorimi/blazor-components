self.addEventListener('notificationclick', event => {
    let action = event.action;
    let customData = event.notification.data;

    if (action) {
        console.log("Custom action was clicked: " + action);
    }

    let notification = event.notification;
    console.log("Notification clicked: " +  notification);
});

self.addEventListener('notificationclose', event => {
    let notification = event.notification;

    console.log("Notification closed: " + notification);
});