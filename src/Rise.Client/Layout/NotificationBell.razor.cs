using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.JSInterop;
using Rise.Shared.Notifications;

namespace Rise.Client.Layout;

public partial class NotificationBell
{
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
    [Inject] private INotificationService NotificationService { get; set; } = default!;

    protected override void OnInitialized()
    {
        _ = RequestNotificationSubscriptionAsync();
    }

    private async Task RequestNotificationSubscriptionAsync()
    {
        var subscription = await JSRuntime.InvokeAsync<NotificationSubscription>(
            "blazorPushNotifications.requestSubscription");
        await JSRuntime.InvokeVoidAsync("console.log", subscription);
        if (subscription is not null)
        {
            try
            {
                await NotificationService.SubscribeToNotifications(subscription);
            }
            catch (AccessTokenNotAvailableException ex)
            {
                ex.Redirect();
            }
        }
    }
}