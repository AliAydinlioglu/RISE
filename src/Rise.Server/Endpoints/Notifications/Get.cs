using Rise.Shared.Notifications;
using Rise.Shared.Common;

namespace Rise.Server.Endpoints.Notifications;

public class Get(INotificationService notificationService) 
    : Endpoint<QueryRequest.SkipTake, Result<NotificationResponse.Get>>
{
    public override void Configure()
    {
        Get("/api/notifications");
        Summary(s =>
        {
            s.Summary = "Lijst van verzonden en onbevestigde notificaties";
            s.Description = "De student kan eigen onbevestigde notificaties bekijken";
        });
    }

    public override async Task<Result<NotificationResponse.Get>> ExecuteAsync(
        QueryRequest.SkipTake req, CancellationToken ct)
    {
        return await notificationService.GetNotifications(req, ct);
    }
}
