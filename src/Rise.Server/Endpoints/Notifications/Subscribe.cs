using Microsoft.AspNetCore.Authorization;
using Rise.Shared.Notifications;

namespace Rise.Server.Endpoints.Notifications;

public class SubscribeRequest
{
    public string Endpoint { get; set; } = default!;
    public string P256dh { get; set; } = default!;
    public string Auth { get; set; } = default!;
}

[Authorize]
public class Subscribe(INotificationService notificationService, IHttpContextAccessor httpContextAccessor) : Endpoint<SubscribeRequest, NotificationSubscription>
{
    public override void Configure()
    {
        Put("/notifications/subscribe");
        Summary(s =>
        {
            s.Summary = "Abonneer de huidige gebruiker op push-notificaties";
            s.Description = "Vervangt bestaande subscriptions en bewaart de nieuwe voor de ingelogde gebruiker.";
        });
    }

    public override async Task HandleAsync(SubscribeRequest req, CancellationToken ct)
    {
        var userId = GetUserId();

        if (userId is null)
        {
            HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await HttpContext.Response.CompleteAsync();
            return;
        }
        
        var subscription = new NotificationSubscription
        {
            UserId = userId,
            Url = req.Endpoint,
            P256dh = req.P256dh,
            Auth = req.Auth
        };

        await notificationService.SubscribeToNotifications(subscription);

        Response = subscription;
        HttpContext.Response.StatusCode = StatusCodes.Status200OK;
    }

    private string? GetUserId()
    {
        return httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;
        // Pas eventueel aan op jouw claim type (bijv. ClaimTypes.NameIdentifier)
    }
}