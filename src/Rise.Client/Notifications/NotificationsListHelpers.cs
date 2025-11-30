using static Rise.Client.Components.RiseNotification;

namespace Rise.Client.Notifications
{
    internal static class NotificationsListHelpers
    {
        public static NotificationSeverity ToSeverity(string from)
        {
            return from.ToLower() switch
            {
                "information" => NotificationSeverity.Success,
                "warning" => NotificationSeverity.Warning,
                "urgent" => NotificationSeverity.Error,
                _ => NotificationSeverity.Info,
            };
        }
    }
}