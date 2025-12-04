using Rise.Shared.Common;
using Rise.Shared.Notifications;
using System.Net.Http.Json;
using static Rise.Shared.Notifications.SubscribeRequest;

namespace Rise.Client.Shared;

public class NotificationService(HttpClient httpClient) : INotificationService
{
    public async Task<Result> AcknowledgeAsRead(AcknowledgeRequest.Post req, CancellationToken ct = default)
    {
        var response = await httpClient.PostAsJsonAsync("/api/notifications/acknowledge", req, ct);
        var result = await response.Content.ReadFromJsonAsync<Result>(cancellationToken: ct);
        return result!;
    }

    public async Task<Result<NotificationResponse.Get>> GetNotifications(QueryRequest.SkipTake request, CancellationToken ct)
    {
        var result = await httpClient.GetFromJsonAsync<Result<NotificationResponse.Get>>(
            $"/api/notifications", cancellationToken: ct);
        return result!;
    }

    public Task<Result> Notify(NotifyRequest.Message m, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> SubscribeToNotification(SubscribeRequest.Subscribe subscribe, CancellationToken ct = default)
    {
        var response = await httpClient.PutAsJsonAsync("/api/notifications/subscribe", subscribe, ct);
        var result = await response.Content.ReadFromJsonAsync<Result>(cancellationToken: ct);
        return result!;
    }

    public async Task<Result<SubscriptionsResponse.Get>> SubscriptionSettings(CancellationToken ct = default)
    {
        var result = await httpClient.GetFromJsonAsync<Result<SubscriptionsResponse.Get>>(
            $"/api/notifications/subscriptions", cancellationToken: ct);
        return result!;
    }

    public async Task<Result> UnsubscribeFromNotification(UnsubscribeRequest.Unsubscribe unsubscribe, CancellationToken ct = default)
    {
        var response = await httpClient.PutAsJsonAsync("/api/notifications/unsubscribe", unsubscribe, ct);
        var result = await response.Content.ReadFromJsonAsync<Result>(cancellationToken: ct);
        return result!;
    }
}
