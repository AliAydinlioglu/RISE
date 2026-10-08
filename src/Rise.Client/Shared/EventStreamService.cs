using Microsoft.JSInterop;
using Rise.Shared.Notifications;
using System.Text.Json;

namespace Rise.Client.Shared;

public interface IEventStreamService
{
    public Task StartAsync(string accessToken);
    public void ReceiveMessage(string message);
    public event Action<NotificationDto.Index>? OnMessage;
}

public class EventStreamService(HttpClient httpClient, IJSRuntime js) : IEventStreamService, IAsyncDisposable
{
    private IJSObjectReference? _instance;
    private DotNetObjectReference<EventStreamService>? _objRef;

    public event Action<NotificationDto.Index>? OnMessage;

    public async Task StartAsync(string accessToken)
    {
        var url = httpClient.BaseAddress + "/api/notifications/stream?access_token=" + accessToken;
        _objRef = DotNetObjectReference.Create(this);
        _instance = await js.InvokeAsync<IJSObjectReference>(
            "blazorEventStreamingNotifications.start",
            url,
            _objRef
        );
    }

    [JSInvokable]
    public void ReceiveMessage(string message)
    {
        var notification = JsonSerializer.Deserialize<NotificationDto.Index>(
            message,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );

        if (notification != null)
            OnMessage?.Invoke(notification);
    }

    public async ValueTask DisposeAsync()
    {
        if (_instance != null)
            await js.InvokeVoidAsync("blazorEventStreamingNotifications.stop", _instance);

        _objRef?.Dispose();
    }
}
