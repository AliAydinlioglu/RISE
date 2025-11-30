using Ardalis.Result;
using Rise.Domain.Notifications;
using Rise.Shared.Common;
using Rise.Shared.Notifications;

namespace Rise.Client.Shared;

public class FakeNotificationService(HttpClient httpClient) : INotificationService
{
    public Task<Result<NotificationResponse.Get>> GetNotifications(QueryRequest.SkipTake request, CancellationToken ct)
    {
        throw new NotImplementedException();
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
