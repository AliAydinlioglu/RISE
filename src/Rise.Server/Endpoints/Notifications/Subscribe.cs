using Rise.Shared.Identity;
using Rise.Shared.Notifications;

namespace Rise.Server.Endpoints.Notifications;

public class Subscribe(INotificationService notificationService) : Endpoint<SubscribeRequest.Subscribe, Result>
{
    public override void Configure()
    {
        Put("/api/notifications/subscribe");
        Summary(s =>
        {
            s.Summary = "Abonneer op notificaties";
            s.Description = "De student kan zich abonneren op verschillende type notificaties. Kan via verschillende kanalen verwittigd worden.";
        });
        Roles(AppRoles.DistanceStudent, AppRoles.RegularStudent);
    }

    public override async Task<Result> ExecuteAsync(SubscribeRequest.Subscribe req, CancellationToken ct)
    {
        return await notificationService.SubscribeToNotification(req, ct);
    }
}