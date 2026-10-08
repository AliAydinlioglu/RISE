namespace Rise.Shared.Notifications;

public class SubscriptionDto
{
    public class Settings()
    {
        public required string TypeOfNotification { get; set; }
        public required IList<NotificationChannelDto> Channels { get; set; }
    }
    public class NotificationChannelDto
    {
        public required string Name { get; set; }
        public bool IsSubscribed { get; set; }
    }
}
