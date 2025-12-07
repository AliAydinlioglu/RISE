namespace Rise.Domain.Notifications;

public class Subscription: Entity
{
    public string UserName { get; private set; }
    //public DateTime LastRun { get; set; } = DateTime.MinValue;
    public NotificationTypes TypeOfNotification { get; private set; }

    public IList<NotificationChannels> Channels { get; private set; }

    private Subscription() { }

    public Subscription(string username, NotificationTypes typeOfNotification, IList<NotificationChannels> channels)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("UserName cannot be empty.", nameof(username));

        if (!channels.Any())
            throw new ArgumentException("Channels connact be empty", nameof(channels));

        UserName = username;
        TypeOfNotification = typeOfNotification;
        Channels = channels;
    }

    public void AddChannel(NotificationChannels channel)
    {
        if(!Channels.Contains(channel))  
            Channels.Add(channel);
    }

    public void RemoveChannel(NotificationChannels channel)
    {
        Channels.Remove(channel);
    }

}
