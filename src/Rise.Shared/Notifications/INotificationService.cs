namespace Rise.Shared.Notifications;

public interface INotificationService
{
	Task SubscribeToNotifications(NotificationSubscription subscription);
}
