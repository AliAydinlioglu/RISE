using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.JSInterop;
using Rise.Shared.Notifications;
using static Rise.Shared.Notifications.SubscribeRequest;
using static Rise.Shared.Notifications.SubscriptionDto;

namespace Rise.Client.Notifications;

public partial class NotificationsSettings
{
    [Inject] public required INotificationService NotificationService { get; set; }
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
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
        if (subscribe)
        {
            switch (channel.Name)
            {
                case ("PushNotification"):
                    await RequestPushNotificationSubscriptionAsync(notificationType);
                    break;
                case ("InApp"):
                    _ = await NotificationService.SubscribeToNotification(new SubscribeRequest.Subscribe
                    {
                        Channels = new SubscribeRequest.NotificationChannels { InApp = true },
                        NotificationType = notificationType,
                    });
                    break;
                default:
                    throw new ArgumentException("Invalid channel");
            }
        }
        else
        {
            _ = await NotificationService.UnsubscribeFromNotification(new UnsubscribeRequest.Unsubscribe
            {
                NotificationChannel = channel.Name,
                NotificationType = notificationType
            });
        }
    }

    protected async Task RequestPushNotificationSubscriptionAsync(string notificationType)
    {
        var pnChannel = await JSRuntime.InvokeAsync<PushNotification>(
            "blazorPushNotifications.requestSubscription");
        await JSRuntime.InvokeVoidAsync("console.log", pnChannel);
        if (pnChannel is not null)
        {
            try
            {
                await NotificationService.SubscribeToNotification(new SubscribeRequest.Subscribe
                {
                    Channels = new SubscribeRequest.NotificationChannels { InApp = false, Push = pnChannel },
                    NotificationType = notificationType,
                });
            }
            catch (AccessTokenNotAvailableException ex)
            {
                ex.Redirect();
            }
        }
    }
}
