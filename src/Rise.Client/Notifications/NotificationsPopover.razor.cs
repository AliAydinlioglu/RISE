using Microsoft.AspNetCore.Components;
using Rise.Shared.Notifications;
using Rise.Shared.Common;

namespace Rise.Client.Notifications;

public partial class NotificationsPopover
{
    public List<NotificationDto.Index> Notifications { get; set; } = [];
    public bool HasNotifications => Notifications.Any();
    private bool visible;

    [Inject] public required INotificationService NotificationService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var request = new QueryRequest.SkipTake
        {
            Skip = 0,
            Take = 50,
        };

        var result = await NotificationService.GetNotifications(request);
        Notifications = [.. result.Value.Notifications];
    }

    public void ToggleOverlay(bool value)
    {
        visible = value;
    }

    public void AckowledgeMe(NotificationDto.Index not)
    {
        Notifications.RemoveAll(n => n.NotificationId == not.NotificationId);
        if (Notifications.Count == 0)
            ToggleOverlay(false);
        StateHasChanged();
    }
}
