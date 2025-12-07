using Rise.Shared.Notifications;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace Rise.Services.Notifications;

public static class EventStreamNotification
{
    // Per user: lijst van alle actieve client channels
    private static readonly ConcurrentDictionary<string, ConcurrentBag<Channel<NotificationDto.Index>>> _userChannels
        = new();

    // Subscribe: een nieuwe SSE-verbinding krijgt zijn eigen channel
    public static Channel<NotificationDto.Index> Subscribe(string userName)
    {
        var channel = Channel.CreateUnbounded<NotificationDto.Index>();

        var bag = _userChannels.GetOrAdd(userName, _ => new ConcurrentBag<Channel<NotificationDto.Index>>());
        bag.Add(channel);

        return channel;
    }

    // Publish: stuur notificatie naar alle actieve clients van deze user
    public static void Publish(string userName, NotificationDto.Index notification)
    {
        if (_userChannels.TryGetValue(userName, out var channels))
        {
            foreach (var channel in channels)
            {
                // schrijf asynchroon, negeer als channel gesloten is
                channel.Writer.TryWrite(notification);
            }
        }
    }

    // Optional: cleanup gesloten channels
    public static void CleanupClosedChannels()
    {
        foreach (var kvp in _userChannels)
        {
            var alive = kvp.Value.Where(c => !c.Writer.TryComplete()).ToList();
            _userChannels[kvp.Key] = new ConcurrentBag<Channel<NotificationDto.Index>>(alive);
        }
    }
}
