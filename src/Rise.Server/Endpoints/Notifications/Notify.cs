using Rise.Shared.Notifications;

namespace Rise.Server.Endpoints.Notifications;

public class Notify(INotificationService notificationService) : Endpoint<NotifyRequest.Message, Result>
{
    public override void Configure()
    {
        Put("/api/notifications/notify");
        Summary(s =>
        {
            s.Summary = "Verstuur notificaties";
            s.Description = "Geabonneerde studenten krijgen notificaties via ingeschreven kanalen";
        });
    }

    public override async Task<Result> ExecuteAsync(NotifyRequest.Message msg, CancellationToken ct)
    {
        return await notificationService.Notify(msg, ct);
    }
}