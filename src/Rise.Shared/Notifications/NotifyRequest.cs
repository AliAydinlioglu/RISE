namespace Rise.Shared.Notifications;

public class NotifyRequest
{
    public class Message
    {
        public required string TypeOfNotification { get; set; }
        public required string NotificationLevel { get; set; }
        public required string MsgTitle { get; set; }
        public required string MsgBody { get; set; }
        public string LinkToAppPage { get; set; } = string.Empty;

    }
}
