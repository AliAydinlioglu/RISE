namespace Rise.Domain.Notifications;

public class Notification : Entity
{
    private Notification() { }
    public Notification(
        NotificationTypes typeOfNotification,
        NotificationLevels notificationLevel,
        Message msgDetail,
        DateTime sentOn
    )
    {
        TypeOfNotification = typeOfNotification;
        NotificationLevel = notificationLevel;
        MsgDetail = msgDetail;
        SentOn = sentOn;
        _acknowledgements = [];
    }

    public NotificationTypes TypeOfNotification { get; private set; }
    public NotificationLevels NotificationLevel { get; private set; }
    public Message MsgDetail { get; private set; }
    public DateTime SentOn { get; private set; }

    private readonly List<NotificationAcknowledge> _acknowledgements;
    public IReadOnlyCollection<NotificationAcknowledge> Acknowledgements => _acknowledgements;

    public bool IsAcknowledgedByUser(string userName) =>
        _acknowledgements.Any(x => x.UserName == userName);

    public void AddAcknowledge(NotificationAcknowledge ack) =>
        _acknowledgements.Add(ack);
}