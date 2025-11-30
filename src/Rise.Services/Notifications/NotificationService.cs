using Microsoft.EntityFrameworkCore;
using Rise.Domain.Notifications;
using Rise.Persistence;
using Rise.Services.Identity;
using Rise.Shared.Common;
using Rise.Shared.Notifications;
using static Rise.Shared.Notifications.SubscribeRequest;
using NotificationChannels = Rise.Domain.Notifications.NotificationChannels;

namespace Rise.Services.Notifications;

public class NotificationService(ApplicationDbContext dbContext, ISessionContextProvider sep) : INotificationService
{
    public List<Subscription> GetSubsciptionsByType(string typeOfNotification)
    {
        throw new NotImplementedException();
    }

    public Task<Result> Notify(string typeOfNotification, string notificationLevel, string description, string title, string[] channels)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> SubscribeToNotification(
        SubscribeRequest.Subscribe subscribe, CancellationToken ctx = default)
    {
        var subscription = new Subscription(GetUser(), ParseNT(subscribe.NotificationType));
        HandleChannels(subscribe, subscription);
        
        dbContext.Subscriptions.Add(subscription);

        await dbContext.SaveChangesAsync(ctx);

        return Result.Success();
    }

    private void HandleChannels(Subscribe subscribe, Subscription subscription)
    {
        if (subscribe.Channels.Push != null)
        {
            var channel = new PushNotificationChannel(
                subscribe.Channels.Push.SubscriptionId,
                GetUser(),
                "www.campus.app",
                subscribe.Channels.Push.P256dh,
                subscribe.Channels.Push.Auth);
            dbContext.PushNotificationChannels.Add(channel);

            subscription.Channels.Add(NotificationChannels.PushNotification);
        }
        if (subscribe.Channels.InApp)
        {
            subscription.Channels.Add(NotificationChannels.InApp);
        }
    }

    public async Task<Result> UnsubscribeFromNotification(
        UnsubscribeRequest.Unsubscribe unsubscribe, CancellationToken ctx = default)
    {
        var subscription = await dbContext.Subscriptions
            .Where(
                e => e.UserName == GetUser() && e.TypeOfNotification == ParseNT(unsubscribe.NotificationType)
            ).SingleOrDefaultAsync(ctx);

        if (subscription == null)
            return Result.NotFound(
                $"Subscription on '{GetUser()}' for '{unsubscribe.NotificationType}' was not found.");

        dbContext.Subscriptions.Remove(subscription);

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

        query = query.Where(p => notificationTypes.Contains(p.TypeOfNotification));

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
