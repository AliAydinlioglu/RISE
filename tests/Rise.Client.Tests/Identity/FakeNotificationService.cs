using Ardalis.Result;
using Rise.Domain.Notifications;
using Rise.Shared.Common;
using Rise.Shared.Notifications;

namespace Rise.Client.Shared;

public class FakeNotificationService(HttpClient httpClient) : INotificationService
{
    public Task<Result<NotificationResponse.Get>> GetNotifications(QueryRequest.SkipTake request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result> Notify(NotifyRequest.Message msg, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<Result> SubscribeToNotification(SubscribeRequest.Subscribe subscribe, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<SubscriptionsResponse.Get>> SubscriptionSettings(CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UnsubscribeFromNotification(UnsubscribeRequest.Unsubscribe unsubscribe, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
