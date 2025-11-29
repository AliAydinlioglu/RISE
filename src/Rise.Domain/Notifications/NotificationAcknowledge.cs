
namespace Rise.Domain.Notifications;

public class NotificationAcknowledge(string userName, DateTime readOn): ValueObject
{
    public string UserName { get; private set; } = userName;
    public DateTime ReadOn { get; set; } = readOn;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return UserName;
        yield return ReadOn;
    }
}
