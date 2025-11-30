namespace Rise.Domain.Notifications;

public class Subscription: Entity
{
    public string UserName { get; private set; }
    //public DateTime LastRun { get; set; } = DateTime.MinValue;
    public NotificationTypes TypeOfNotification { get; private set; }

    public IList<NotificationChannels> Channels { get; private set; } = [];

    private Subscription() { }

    public Subscription(string username, NotificationTypes typeOfNotification)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("UserName cannot be empty.", nameof(username));

        UserName = username;
        TypeOfNotification = typeOfNotification;
    }

    public void AddChannel(NotificationChannels channel)
    {
        Channels.Add(channel);
    }

    public void RemoveChannel(NotificationChannels channel)
    {
        Channels.Remove(channel);
    }
}
