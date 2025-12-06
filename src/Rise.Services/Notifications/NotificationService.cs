using Microsoft.EntityFrameworkCore;
using Rise.Domain.Notifications;
using Rise.Persistence;
using Rise.Services.Identity;
using Rise.Shared.Common;
using Rise.Shared.Notifications;
using static Rise.Shared.Notifications.SubscribeRequest;
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
        var notification = new Notification(
            Enum.Parse<NotificationTypes>(msg.TypeOfNotification),
            Enum.Parse<NotificationLevels>(msg.NotificationLevel),
            new Message(msg.MsgTitle, msg.MsgBody, msg.LinkToAppPage??""),
            DateTime.Now
        );

        dbContext.Notifications.Add(notification);
        
        await dbContext.SaveChangesAsync();

        var subscriptions = await dbContext.Subscriptions
            .Where(
                e => e.TypeOfNotification == ParseNT(msg.TypeOfNotification) && !e.IsDeleted
            ).ToListAsync(ct);

        if (subscriptions.Count == 0)
        {
            return Result.Success();
        }

        foreach(var subscription in subscriptions){
            var channels = subscription.Channels.ToList();
            foreach (var channel in channels) {
                switch (channel)
                {
                    case NotificationChannels.InApp:
                        EventStreamNotification.Publish(subscription.UserName, new NotificationDto.Index
                        {
                            IsRead = false,
                            MsgBody = notification.MsgDetail.Description,
                            MsgTitle = notification.MsgDetail.Title,
                            NotificationLevel = notification.NotificationLevel.ToString(),
                            TypeOfNotification = notification.TypeOfNotification.ToString(),
                            LinkToAppPage = notification.MsgDetail.UrlDetailPage,
                            NotificationId = notification.Id,
                        });
                        break;
                    case NotificationChannels.PushNotification:
                        var pushNotificationSettings = await dbContext.PushNotificationChannels
                            .FirstOrDefaultAsync(c => c.UserName == subscription.UserName, ct);
                        if (pushNotificationSettings is not null)
                            await PushNotification.SendPushNotificationAsync(pushNotificationSettings, msg.MsgBody);
                        break;
                    default:
                        break;
                }
            }
        }

        return Result.Success();
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
            await PushNotification.SendPushNotificationAsync(channel, $"je bent ingeschreven voor {subscribe.NotificationType}");
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
                IsRead = p.IsAcknowledgedByUser(GetUser()),
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

    public async Task<Result> AcknowledgeAsRead(AcknowledgeRequest.Post req, CancellationToken ct = default)
    {
        var notification = await dbContext.Notifications
            .Where( n => n.Id == req.NotificationId)
            .SingleOrDefaultAsync(ct);
        if (notification is null)
            return Result.NotFound($"Notification with Id '{req.NotificationId}' was not found.");

        if (!notification.IsAcknowledgedByUser(GetUser()))
            notification.AddAcknowledge(new NotificationAcknowledge(GetUser(), DateTime.Now));

        await dbContext.SaveChangesAsync(ct);

        return Result.Success();
    }
}
