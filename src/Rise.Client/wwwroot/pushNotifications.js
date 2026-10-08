(function () {
    const applicationServerPublicKey = "BMxhk8SDSWBXlx0iYw9fyqAR5g-aDWHdYV62d7rObYYiK54psfyyxj0C7Gxniz3aF_An6rNM93XqO9OOR3wUY7Y";
    //r9OlE2nzQFPCbbGWf8J_2QF5DObpZaO7F6l7S_XfHTQ
    window.blazorPushNotifications = {
        requestSubscription: async () => {
            const worker = await navigator.serviceWorker.getRegistration();
            if (!worker) {
                console.error("Geen service worker geregistreerd");
                return null;
            }

            let subscription = await worker.pushManager.getSubscription();
            if (!subscription) {
                subscription = await subscribe(worker); // zorg dat subscribe returnt!
            }

            if (!subscription) {
                console.error("Kon geen subscription maken");
                return null;
            }

            return {
                url: subscription.endpoint,
                p256dh: arrayBufferToBase64(subscription.getKey('p256dh')),
                auth: arrayBufferToBase64(subscription.getKey('auth'))
            };
        }
    };

    async function subscribe(worker) {
        try {
            return await worker.pushManager.subscribe({
                userVisibleOnly: true,
                applicationServerKey: applicationServerPublicKey
            });
        } catch (error) {
            if (error.name === 'NotAllowedError') {
                return null;
            }
            throw error;
        }
    }

    function arrayBufferToBase64(buffer) {
        var binary = '';
        var bytes = new Uint8Array(buffer);
        var len = bytes.byteLength;
        for (var i = 0; i < len; i++) {
            binary += String.fromCharCode(bytes[i]);
        }
        return window.btoa(binary);
    }
})();