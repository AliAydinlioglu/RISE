namespace Rise.Shared.Notifications;

public class UnsubscribeRequest
{
    public class Unsubscribe
    {
        public required string NotificationType { get; set; }
        public required string NotificationChannel { get; set; }
    }
}
