using Rise.Domain.Notifications;

namespace Rise.Persistence.SeedData;

public static class NotificationsSubscriptionsSeeder
{
    private static string[] _users = [
        "Andy@rise2526t2campusappoutlook.onmicrosoft.com",
        "david@rise2526t2campusappoutlook.onmicrosoft.com",
        "distance@rise2526t2campusappoutlook.onmicrosoft.com",
        "klant@rise2526t2campusappoutlook.onmicrosoft.com",
        "stefaan@rise2526t2campusappoutlook.onmicrosoft.com",
        "regular@rise2526t2campusappoutlook.onmicrosoft.com",
        "wim@rise2526t2campusappoutlook.onmicrosoft.com"];

    public static async Task Seed(ApplicationDbContext dbContext)
    {

        dbContext.Notifications.AddRange(
            dbContext.Deadlines
                .Select(x => new Notification(
                    NotificationTypes.Deadline,
                    NotificationLevels.Warning,
                    new Message(x.TaskTitle, 
                        x.TaskDescription + " " + x.DeadlineTimestamp.ToString("dddd d MMMM yyyy") ?? ""
                        , ""),
                    DateTime.Now)
                ));
        dbContext.Notifications.Add(new Notification(
            NotificationTypes.Emergency,
            NotificationLevels.Urgent,
            new Message("Schoonmeersen is afgesloten", "Wegens een bommelding is Campus Schoonmeersen ontruimd voor onbepaalde tijd", ""),
            DateTime.Now));
        dbContext.Notifications.Add(new Notification(
            NotificationTypes.LectorAbsence,
            NotificationLevels.Information,
            new Message("Lector afwezig - Business Analysis", "Lector X is as. woensdag afwezig", ""),
            DateTime.Now));

        _users.ToList()
            .ForEach(u =>
            {
                dbContext.Subscriptions.Add(new Subscription(u, NotificationTypes.Deadline));
                dbContext.Subscriptions.Add(new Subscription(u, NotificationTypes.Emergency));
                dbContext.Subscriptions.Add(new Subscription(u, NotificationTypes.LectorAbsence));
            });

        await dbContext.SaveChangesAsync();
    }

}