using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rise.Persistence;
using Rise.Shared.Notifications;
using System.Text.Json;
using WebPush;
using static Rise.Shared.Notifications.SubscribeRequest;

namespace Rise.Services.Notifications;

public class NotificationBackgroundService(
    IServiceScopeFactory scopeFactory, ILogger<NotificationBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("NotificationBackgroundService started.");

        //while (!stoppingToken.IsCancellationRequested)
        //{
        //    using var scope = scopeFactory.CreateScope();
        //    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        //    var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

        //    var subscriptions = await dbContext.NotificationSubscriptions.ToListAsync(stoppingToken);

        //    foreach (var s in subscriptions)
        //    {
        //        try
        //        {
        //            await SendNotificationAsync(s, "Je bericht hier");
        //        }
        //        catch (Exception ex)
        //        {
        //            logger.LogError(ex, "Failed to send notification to {Endpoint}", s.Url);
        //        }
        //    }
        //    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        //}
        logger.LogInformation("NotificationBackgroundService stopping.");
    }

    //private static async Task SendNotificationAsync(PushNotification subscription, string message)
    //{
    //    var publicKey = "BMxhk8SDSWBXlx0iYw9fyqAR5g-aDWHdYV62d7rObYYiK54psfyyxj0C7Gxniz3aF_An6rNM93XqO9OOR3wUY7Y";
    //    var privateKey = "r9OlE2nzQFPCbbGWf8J_2QF5DObpZaO7F6l7S_XfHTQ";

    //    var pushSubscription = new PushSubscription("www.campus.app",
    //        subscription.P256dh, subscription.Auth);
    //    var vapidDetails = new VapidDetails("mailto:admin@example.com", publicKey, privateKey);
    //    var webPushClient = new WebPushClient();

    //    try
    //    {
    //        var payload = JsonSerializer.Serialize(new
    //        {
    //            message
    //        });

    //        await webPushClient.SendNotificationAsync(pushSubscription, payload,
    //            vapidDetails);
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.Error.WriteLine($"Error sending push notification: {ex.Message}");
    //    }
    //}
}
