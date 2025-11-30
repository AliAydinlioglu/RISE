namespace Rise.Shared.Notifications;

public class UnsubscribeRequest
{
    public class Unsubscribe
    {
        public string NotificationType { get; set; } = null!; // bv. "Deadline"
    }
}
