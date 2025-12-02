using Microsoft.EntityFrameworkCore;
using Rise.Domain.Notifications;
using Rise.Persistence;
using Rise.Services.Identity;
using Rise.Shared.Common;
using Rise.Shared.Notifications;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Channels;
using WebPush;
using static Rise.Shared.Notifications.SubscribeRequest;
using static Rise.Shared.Notifications.UnsubscribeRequest;
using NotificationChannels = Rise.Domain.Notifications.NotificationChannels;

namespace Rise.Services.Notifications;

public class NotificationService(ApplicationDbContext dbContext, ISessionContextProvider sep) 
    : INotificationService
{
    public async Task<Result<SubscriptionsResponse.Get>> SubscriptionSettings(CancellationToken ct = default)
    {
        var subs = await dbContext.Subscriptions
            .Where(s => s.UserName == GetUser() && !s.IsDeleted)
            .ToListAsync();
           
        var settings = Enum.GetNames<NotificationTypes>()
            .Select(t =>
                new SubscriptionDto.Settings
                {
                    TypeOfNotification = t,
                    Channels = Enum.GetValues<NotificationChannels>()
                            .Select(n => new SubscriptionDto.NotificationChannelDto
                            {
                                Name = n.ToString(),
                                IsSubscribed = subs.Any(s => s.TypeOfNotification.ToString().Contains(t) 
                                    && s.Channels.Contains(n))
                            })
                            .ToList()
                });

        return Result.Success(new SubscriptionsResponse.Get { Settings = [.. settings] });
    }

    public async Task<Result> Notify(NotifyRequest.Message msg, CancellationToken ct = default)
    {
        var subscription = await dbContext.Subscriptions
            .Where(
                e => e.UserName == GetUser() && e.TypeOfNotification == ParseNT(msg.TypeOfNotification) && !e.IsDeleted
            ).SingleOrDefaultAsync(ct);

        if (subscription == null)
        {
            Log.Error($"Subscription on '{GetUser()}' for '{msg.TypeOfNotification}' was not found.");
            return Result.NotFound(
                $"Subscription on '{GetUser()}' for '{msg.TypeOfNotification}' was not found.");
        }

        var pushNotificationSettings = await dbContext.PushNotificationChannels
            .Where(c => c.UserName == GetUser())
            .SingleOrDefaultAsync(ct);

        if(pushNotificationSettings == null)
            return Result.Success();

        await SendNotificationAsync(pushNotificationSettings, msg.MsgBody);

        return Result.Success();
    }

    private static async Task SendNotificationAsync(PushNotificationChannel subscription, string message)
    {
        var publicKey = "BMxhk8SDSWBXlx0iYw9fyqAR5g-aDWHdYV62d7rObYYiK54psfyyxj0C7Gxniz3aF_An6rNM93XqO9OOR3wUY7Y";
        var privateKey = "r9OlE2nzQFPCbbGWf8J_2QF5DObpZaO7F6l7S_XfHTQ";

        var pushSubscription = new PushSubscription(subscription.Url,
            subscription.P256dh, subscription.Auth);
        var vapidDetails = new VapidDetails("mailto:admin@example.com", publicKey, privateKey);
        var webPushClient = new WebPushClient();

        try
        {
            var payload = JsonSerializer.Serialize(new
            {
                message
            });

            await webPushClient.SendNotificationAsync(pushSubscription, payload,
                vapidDetails);
        }
        catch (Exception ex)
        {
            Log.Error($"Error sending push notification: {ex.Message}");
        }
    }

    public async Task<Result> SubscribeToNotification(
        SubscribeRequest.Subscribe subscribe, CancellationToken ctx = default)
    {
        var channels = await HandleChannels(subscribe);
        var subscription = await dbContext.Subscriptions
            .Where(
                e => e.UserName == GetUser() && e.TypeOfNotification == ParseNT(subscribe.NotificationType)
            ).SingleOrDefaultAsync(ctx);

        if (subscription == null)
        {
            subscription = new Subscription(
                GetUser(), 
                ParseNT(subscribe.NotificationType), 
                channels);
            dbContext.Subscriptions.Add(subscription);
        }  else
        {
            subscription.IsDeleted = false;
            channels.ToList().ForEach(c => subscription.AddChannel(c));
        }


        await dbContext.SaveChangesAsync(ctx);

        return Result.Success();
    }

    private async Task<IList<NotificationChannels>> HandleChannels(Subscribe subscribe)
    {
        List<NotificationChannels> channels = [];
        if (subscribe.Channels.Push != null)
        {
            var channel = new PushNotificationChannel(
                GetUser(),
                subscribe.Channels.Push.Url,
                subscribe.Channels.Push.P256dh,
                subscribe.Channels.Push.Auth);

            var fChannel = dbContext.PushNotificationChannels.Where( c => c.Id.Contains(channel.Id)).SingleOrDefault();
            if (fChannel == null)
                dbContext.PushNotificationChannels.Add(channel);
            else
                fChannel.IsDeleted = false;

            channels.Add(NotificationChannels.PushNotification);
            await SendNotificationAsync(channel, $"je bent ingeschreven voor {subscribe.NotificationType}");
        }
        if (subscribe.Channels.InApp)
        {
            channels.Add(NotificationChannels.InApp);
        }

        return channels;
    }

    public async Task<Result> UnsubscribeFromNotification(
        UnsubscribeRequest.Unsubscribe unsubscribe, CancellationToken ctx = default)
    {
        var subscription = await dbContext.Subscriptions
            .Where(
                e => e.UserName == GetUser() && e.TypeOfNotification == ParseNT(unsubscribe.NotificationType)
            ).SingleOrDefaultAsync(ctx);

        if (subscription == null)
        {
            Log.Error($"Subscription on '{GetUser()}' for '{unsubscribe.NotificationType}' was not found.");
            return Result.NotFound(
                $"Subscription on '{GetUser()}' for '{unsubscribe.NotificationType}' was not found.");
        }
        
        var removeChannel = Enum.Parse<NotificationChannels>(unsubscribe.NotificationChannel);
        if(subscription.Channels.Remove(removeChannel) && !subscription.Channels.Any())
        {
            dbContext.Subscriptions.Remove(subscription);
        }

        await dbContext.SaveChangesAsync(ctx);

        return Result.Success();
    }
    public async Task<Result<NotificationResponse.Get>> GetNotifications(
        QueryRequest.SkipTake request, CancellationToken ct)
    {
        var notificationTypes = dbContext.Subscriptions
            .Where(p => p.UserName.Contains(GetUser()))
            .Select(s => s.TypeOfNotification);

        var query = dbContext.Notifications.AsQueryable();

        query = query.Where(p => notificationTypes.Contains(p.TypeOfNotification) && !p.IsDeleted);

        var totalCount = await query.CountAsync(ct);

        // Apply ordering
        if (!string.IsNullOrWhiteSpace(request.OrderBy))
        {
            query = request.OrderDescending
                ? query.OrderByDescending(e => EF.Property<object>(e, request.OrderBy))
                : query.OrderBy(e => EF.Property<object>(e, request.OrderBy));
        }
        else
        {
            // Default order
            query = query.OrderBy(p => p.SentOn);
        }

        var result = await query.AsNoTracking()
            .Skip(request.Skip)
            .Take(request.Take)
            .Select(p => new NotificationDto.Index
            {
                IsRead = false,
                NotificationId = p.Id,
                MsgBody = p.MsgDetail.Description,
                MsgTitle = p.MsgDetail.Title,
                NotificationLevel = p.NotificationLevel.ToString(),
                TypeOfNotification = p.TypeOfNotification.ToString(),
                LinkToAppPage = p.MsgDetail.UrlDetailPage
            })
            .ToListAsync(ct);

        return Result.Success(new NotificationResponse.Get
        {
            Notifications = result,
            TotalCount = totalCount
        });
    }

    private string GetUser()
    {
        return (sep.User?.Identity?.Name) ?? throw new UnauthorizedAccessException();
    }
    private static NotificationTypes ParseNT(string type)
    {
        return Enum.Parse<NotificationTypes>(type);
    }

}
