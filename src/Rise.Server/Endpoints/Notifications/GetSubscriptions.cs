using Rise.Shared.Notifications;

namespace Rise.Server.Endpoints.Notifications;

public class GetSubscriptions(INotificationService notificationService) 
    : EndpointWithoutRequest<Result<SubscriptionsResponse.Get>>
{
    public override void Configure()
    {
        Get("/api/notifications/subscriptions");
        Summary(s =>
        {
            s.Summary = "Lijst van mogelijk abonnementen, gebruik voor settings";
            s.Description = "De student krijgt een lijst van mogelijke abonnementen en via welke kanalen de student notificaties wenst. Daarin staat de status, inschreven of niet.";
        });
    }

    public override async Task<Result<SubscriptionsResponse.Get>> ExecuteAsync(CancellationToken ct)
    {
        return await notificationService.SubscriptionSettings(ct);
    }
}
