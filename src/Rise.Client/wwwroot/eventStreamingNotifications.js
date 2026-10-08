(function () {
    window.blazorEventStreamingNotifications = {
        start: function (url, dotNetRef) {
            const es = new EventSource(url, { withCredentials: true });

            es.addEventListener("Notification", (e) => {
                console.log(e.data);
                dotNetRef.invokeMethodAsync("ReceiveMessage", e.data);
            });

            return es;
        },

        stop: function (eventSource) {
            if (eventSource) eventSource.close();
        }
    };
})();