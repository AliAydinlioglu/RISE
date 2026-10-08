using Rise.Domain.Notifications;
using System.Text.Json;
using WebPush;

namespace Rise.Services.Notifications;

public static class PushNotification
{
    public static async Task SendPushNotificationAsync(PushNotificationChannel subscription, string message)
    {
        var publicKey = "BMxhk8SDSWBXlx0iYw9fyqAR5g-aDWHdYV62d7rObYYiK54psfyyxj0C7Gxniz3aF_An6rNM93XqO9OOR3wUY7Y";
        var privateKey = "r9OlE2nzQFPCbbGWf8J_2QF5DObpZaO7F6l7S_XfHTQ";

        var pushSubscription = new PushSubscription(subscription.Url,
            subscription.P256dh, subscription.Auth);
        var vapidDetails = new VapidDetails("mailto:admin@example.com", publicKey, privateKey);
        var webPushClient = new WebPushClient();

        try
        {
            var payload = JsonSerializer.Serialize(new
            {
                message
            });

            await webPushClient.SendNotificationAsync(pushSubscription, payload,
                vapidDetails);
        }
        catch (Exception ex)
        {
            Log.Error($"Error sending push notification: {ex.Message}");
        }
    }
}
