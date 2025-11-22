using Rise.Shared.Notifications;
using System.Net.Http.Json;

namespace Rise.Client.Shared;

public class NotificationService(HttpClient httpClient) : INotificationService
{
    public async Task SubscribeToNotifications(NotificationSubscription subscription)
    {
        var response = await httpClient.PutAsJsonAsync("notifications/subscribe",
            subscription);
        response.EnsureSuccessStatusCode();
    }
}
