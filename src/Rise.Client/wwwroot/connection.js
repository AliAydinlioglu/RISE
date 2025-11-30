// Source - https://stackoverflow.com/a
// Posted by Kebechet, modified by community. See post 'Timeline' for change history
// Retrieved 2025-11-30, License - CC BY-SA 4.0

// https://stackoverflow.com/questions/69670507/how-to-check-client-connection-status-in-blazor-web-assembly

let handler;

window.Connection = {
    Initialize: function (interop) {

        handler = function () {
            interop.invokeMethodAsync("Connection.StatusChanged", navigator.onLine);
        }

        window.addEventListener("online", handler);
        window.addEventListener("offline", handler);

        handler(navigator.onLine);
    },
    Dispose: function () {

        if (handler != null) {

            window.removeEventListener("online", handler);
            window.removeEventListener("offline", handler);
        }
    }
};
