using Rise.Persistence;
using Rise.Shared.Notifications;
using System.Text.Json;
using WebPush;

namespace Rise.Services.Notifications;

public class NotificationService(ApplicationDbContext dbContext) : INotificationService
{
    public async Task SubscribeToNotifications(NotificationSubscription subscription)
    {
        //var oldSubscriptions = dbContext.NotificationSubscriptions.Where(
        //    e => e.UserId == subscription.UserId);
        //dbContext.NotificationSubscriptions.RemoveRange(oldSubscriptions);

        //dbContext.NotificationSubscriptions.Add(subscription);

        await dbContext.SaveChangesAsync();
    }

   
}
