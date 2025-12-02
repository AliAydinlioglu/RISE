namespace Rise.Shared.Notifications;

public partial class SubscribeRequest
{
    public class Subscribe
    {
        public string NotificationType { get; set; } = null!; // bv. "Deadline"

        public NotificationChannels Channels { get; set; } = new NotificationChannels();
    }
    public class PushNotification
    {
        public string Url { get; set; }
        public string P256dh { get; set; }
        public string Auth { get; set; }
    }

    // Channels container
    public class NotificationChannels
    {
        //public bool Mail { get; set; } = false;
        public bool InApp { get; set; } = false;
        public PushNotification? Push { get; set; } // null als niet gebruikt
    }

}

