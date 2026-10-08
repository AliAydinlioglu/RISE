using Microsoft.AspNetCore.Components;
using Rise.Shared.Notifications;
using static Rise.Shared.Notifications.SubscribeRequest;
using static Rise.Shared.Notifications.SubscriptionDto;

namespace Rise.Client.Notifications;

public partial class NotificationsSettings
{
    [Inject] public required INotificationService NotificationService { get; set; }
    [Parameter] public IEnumerable<SubscriptionDto.Settings> NotificationSettings { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        var result = await NotificationService.SubscriptionSettings();
        NotificationSettings = result.Value.Settings;
    }

    protected async Task OnChannelChanged(string notificationType,
        NotificationChannelDto channel, bool subscribe)
    {
        channel.IsSubscribed = subscribe;
        if (channel.IsSubscribed)
        {
            await NotificationService.SubscribeToNotification(new SubscribeRequest.Subscribe
            {
                Channels = new SubscribeRequest.NotificationChannels
                {
                    InApp = channel.Name == "InApp",
                    Push = channel.Name == "PushNotification" ? new PushNotification() : null
                },
                NotificationType = notificationType,
            });
        }
        else
        {
            await NotificationService.UnsubscribeFromNotification(new UnsubscribeRequest.Unsubscribe
            {
                NotificationChannel = channel.Name,
                NotificationType = notificationType
            });
        }
    }

}
