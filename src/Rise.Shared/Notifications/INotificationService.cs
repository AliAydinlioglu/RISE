using Rise.Domain.Notifications;
using Rise.Shared.Common;

namespace Rise.Shared.Notifications;

public interface INotificationService
{
    Task<Result> SubscribeToNotification(SubscribeRequest.Subscribe subscribe, CancellationToken ct = default);
	List<Subscription> GetSubsciptionsByType(string typeOfNotification);
	Task<Result> UnsubscribeFromNotification(UnsubscribeRequest.Unsubscribe unsubscribe, CancellationToken ct = default);
	Task<Result> Notify(string typeOfNotification, string notificationLevel, string description, string title, string[] channels);
    Task<Result<NotificationResponse.Get>> GetNotifications(QueryRequest.SkipTake request, CancellationToken ct = default);
}