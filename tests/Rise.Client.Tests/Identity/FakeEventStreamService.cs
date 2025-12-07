using Rise.Client.Shared;
using Rise.Shared.Notifications;

namespace Rise.Client.Faker;

public class FakeEventStreamService : IEventStreamService
{
    public event Action<NotificationDto.Index>? OnMessage;

    public bool IsStarted { get; private set; }
    public string? LastAccessToken { get; private set; }

    public Task StartAsync(string accessToken)
    {
        IsStarted = true;
        LastAccessToken = accessToken;
        return Task.CompletedTask;
    }

    public void ReceiveMessage(string message)
    {
        // No-op for the fake, or could log it if needed.
    }

    // Helper method for tests to simulate incoming notifications
    public void SimulateNotification(NotificationDto.Index notification)
    {
        OnMessage?.Invoke(notification);
    }
}
