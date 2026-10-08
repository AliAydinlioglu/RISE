namespace Rise.Domain.Notifications;

public class PushNotificationChannel : Entity<String>
{
    public string UserName { get; private set; }
    public string Url { get; private set; }
    public string P256dh { get; private set; }
    public string Auth { get; private set; }

    private PushNotificationChannel() { }

    public PushNotificationChannel(string notificationSubId, string userName,
        string url, string p256dh, string auth) : base(notificationSubId)
    {
        UserName = userName;
        Url = url;
        P256dh = p256dh;
        Auth = auth;
    }

    public PushNotificationChannel(string userName, string url, string p256dh, string auth)
            : this(Hashing.ToMd5String(userName + url + p256dh + auth), userName, url, p256dh, auth)
    {
    }
}
