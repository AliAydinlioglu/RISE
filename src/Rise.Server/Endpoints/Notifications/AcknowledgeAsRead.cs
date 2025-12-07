using Rise.Domain.Notifications;
using Rise.Shared.Notifications;

namespace Rise.Server.Endpoints.Notifications;

public class AcknowledgeAsRead(INotificationService notificationService) : Endpoint<AcknowledgeRequest.Post, Result>
{
    public override void Configure()
    {
        Post("/api/notifications/acknowledge");
        Summary(s =>
        {
            s.Summary = "Bevestig als gelezen";
            s.Description = "De student bevestigd dat de notificatie gelezen is.";
        });
    }

    public override async Task<Result> ExecuteAsync(AcknowledgeRequest.Post req, CancellationToken ct)
    {
        return await notificationService.AcknowledgeAsRead(req, ct);
    }
}