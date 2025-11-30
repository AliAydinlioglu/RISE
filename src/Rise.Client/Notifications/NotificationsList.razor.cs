using Microsoft.AspNetCore.Components;
using Rise.Shared.Notifications;
using static Rise.Client.Components.RiseNotification;

namespace Rise.Client.Notifications;

public partial class NotificationsList
{
    [Parameter] public IEnumerable<NotificationDto.Index> Notifications { get; set; } = [];
}
