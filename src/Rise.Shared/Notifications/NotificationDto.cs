namespace Rise.Shared.Notifications;

public class NotificationDto
{
    public class Index
    {
        public int NotificationId { get; set; }
        public required string TypeOfNotification { get; set; }
        public required string NotificationLevel { get; set; }
        public required string MsgTitle { get; set; }
        public required string MsgBody { get; set; }
        public required bool IsRead { get; set; } 
        public string LinkToAppPage { get; set; } = string.Empty;

    }
}
