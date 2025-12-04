using Rise.Domain.Notifications;
using Rise.Shared.Common;

namespace Rise.Shared.Notifications;

public interface INotificationService
{
    Task<Result> SubscribeToNotification(SubscribeRequest.Subscribe subscribe, CancellationToken ct = default);
    Task<Result<SubscriptionsResponse.Get>> SubscriptionSettings(CancellationToken ct = default);
	Task<Result> UnsubscribeFromNotification(UnsubscribeRequest.Unsubscribe unsubscribe, CancellationToken ct = default);
    Task<Result> Notify(NotifyRequest.Message msg, CancellationToken ct = default);
    Task<Result<NotificationResponse.Get>> GetNotifications(QueryRequest.SkipTake request, CancellationToken ct = default);
    Task<Result> AcknowledgeAsRead(AcknowledgeRequest.Post req, CancellationToken ct = default);
}