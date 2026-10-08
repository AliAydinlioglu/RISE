using Microsoft.AspNetCore.Components;
using MudBlazor;
using Rise.Shared.Notifications;

namespace Rise.Client.Pages;

public partial class PrivateSendNotification
{
    private MudForm? _form;
    private bool submitted = false;

    [Inject] public required INotificationService NotificationService { get; set; }

    private NotificationDto.Index notification = new NotificationDto.Index { 
        IsRead = false,
        MsgBody = string.Empty,
        MsgTitle = string.Empty,
        NotificationLevel = string.Empty,
        TypeOfNotification = string.Empty
    };

    private async Task Submit()
    {
        var result = await NotificationService.Notify(new NotifyRequest.Message
        {
            LinkToAppPage = notification.LinkToAppPage,
            MsgBody = notification.MsgBody,
            MsgTitle = notification.MsgTitle,
            NotificationLevel = notification.NotificationLevel,
            TypeOfNotification = notification.TypeOfNotification,
        });

        submitted = result.IsSuccess;

        if (submitted)
        {
            notification = new NotificationDto.Index
            {
                IsRead = false,
                MsgBody = string.Empty,
                MsgTitle = string.Empty,
                NotificationLevel = string.Empty,
                TypeOfNotification = string.Empty
            };
            await _form.ResetAsync();
        }
    }

}
