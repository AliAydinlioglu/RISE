using Rise.Domain.Notifications;
using Rise.Shared.Common;
using Rise.Shared.Notifications;
using System.Net.Http.Json;

namespace Rise.Client.Notifications;

public class NotificationService(HttpClient httpClient) : INotificationService
{
    public Task<Result<NotificationResponse.Get>> GetNotifications(QueryRequest.SkipTake request, CancellationToken ct)
    {
        var result = httpClient.GetFromJsonAsync<Result<NotificationResponse.Get>>(
            $"/api/notifications", cancellationToken: ct);
        return result!;
    }

    public List<Subscription> GetSubsciptionsByType(string typeOfNotification)
    {
        throw new NotImplementedException();
    }

    public Task<Result> Notify(string typeOfNotification, string notificationLevel, string description, string title, string[] channels)
    {
        throw new NotImplementedException();
    }

    public Task<Result> SubscribeToNotification(SubscribeRequest.Subscribe subscribe, CancellationToken ctx)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UnsubscribeFromNotification(UnsubscribeRequest.Unsubscribe unsubscribe, CancellationToken ctx)
    {
        throw new NotImplementedException();
    }
}
