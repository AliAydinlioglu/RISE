using Rise.Shared.Notifications;

namespace Rise.Client.Shared;

public class FakeNotificationService(HttpClient httpClient) : INotificationService
{
    public Task SubscribeToNotifications(NotificationSubscription subscription)
    {
        return Task.CompletedTask;
    }   
}
