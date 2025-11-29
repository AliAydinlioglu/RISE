namespace Rise.Domain.Notifications;

public class Message(string title, string description, string urlDetailPage): ValueObject
{
    public string Title { get; private set; } = title;
    public string Description { get; private set; } = description;
    public string UrlDetailPage { get; private set; } = urlDetailPage;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Title;
        yield return Description;
    }
}
