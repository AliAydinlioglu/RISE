namespace Rise.Shared.Notifications;

public class NotificationResponse
{
    public class Get()
    {
        public IEnumerable<NotificationDto.Index> Notifications { get; set; } = [];
        public int TotalCount { get; set; }
    }
}
