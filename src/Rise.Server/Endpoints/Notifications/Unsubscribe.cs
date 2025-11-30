using Rise.Shared.Identity;
using Rise.Shared.Notifications;

namespace Rise.Server.Endpoints.Notifications;

public class Unsubscribe(INotificationService notificationService) : Endpoint<UnsubscribeRequest.Unsubscribe, Result>
{
    public override void Configure()
    {
        Delete("/api/notifications/subscription/{notificationType}");
        Summary(s =>
        {
            s.Summary = "Uitschrijven abonnement";
            s.Description = "De student zich uitschrijven op verschillende type notificaties.";
        });
        Roles(AppRoles.DistanceStudent, AppRoles.RegularStudent);
    }

    public override async Task<Result> ExecuteAsync(UnsubscribeRequest.Unsubscribe req, CancellationToken ct)
    {
        return await notificationService.UnsubscribeFromNotification(req, ct);
    }
}