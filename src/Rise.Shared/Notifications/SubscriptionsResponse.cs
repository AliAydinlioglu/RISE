namespace Rise.Shared.Notifications;

public class SubscriptionsResponse
{
    public class Get()
    {
        public required IList<SubscriptionDto.Settings> Settings { get; set; }
    }
}
