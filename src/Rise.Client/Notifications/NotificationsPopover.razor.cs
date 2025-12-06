using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using MudBlazor;
using Rise.Client.Shared;
using Rise.Shared.Common;
using Rise.Shared.Notifications;

namespace Rise.Client.Notifications;

public partial class NotificationsPopover
{
    public List<NotificationDto.Index> Notifications { get; set; } = [];
    public bool HasNotifications => Notifications.Any();
    private bool visible;

    [Inject] public required INotificationService NotificationService { get; set; }
    [Inject] public required IEventStreamService EventStreamService { get; set; }
    [Inject] public required IAccessTokenProvider TokenProvider { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await InitNotificationList();
        await InitEventStreaming();
    }

    public void ToggleOverlay(bool value)
    {
        visible = value;
    }

    private async void AckowledgeMe(NotificationDto.Index not)
    {
        var result = await NotificationService.AcknowledgeAsRead(new AcknowledgeRequest.Post
        {
            NotificationId = not.NotificationId
        });
        if(result.IsSuccess) { 
            Notifications.RemoveAll(n => n.NotificationId == not.NotificationId);
            if (Notifications.Count == 0)
                ToggleOverlay(false);
            StateHasChanged();
        }
    }

    private async Task InitNotificationList()
    {
        var request = new QueryRequest.SkipTake
        {
            Skip = 0,
            Take = 50,
        };

        var result = await NotificationService.GetNotifications(request);
        Notifications = [.. result.Value.Notifications.Where(n => !n.IsRead)];
    }

    private async Task InitEventStreaming()
    {
        var result = await TokenProvider.RequestAccessToken();
        if (result.TryGetToken(out var token))
        {            
            EventStreamService.OnMessage += msg =>
                InvokeAsync(() =>
                {
                    Notifications.Insert(0, msg);
                    StateHasChanged();
                });

            await EventStreamService.StartAsync(token.Value);
        }

    }
}
