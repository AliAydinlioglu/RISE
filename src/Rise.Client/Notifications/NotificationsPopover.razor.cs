using Microsoft.AspNetCore.Components;
using Rise.Shared.Notifications;
using Rise.Shared.Common;

namespace Rise.Client.Notifications;

public partial class NotificationsPopover
{
    public IEnumerable<NotificationDto.Index> Notifications { get; set; } = [];
    public bool HasNotifications => Notifications.Any();

    [Inject] public required INotificationService NotificationService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var request = new QueryRequest.SkipTake
        {
            Skip = 0,
            Take = 50,
        };

        var result = await NotificationService.GetNotifications(request);
        Notifications = result.Value.Notifications;
    }
}
