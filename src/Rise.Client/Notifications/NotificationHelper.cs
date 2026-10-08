using MudBlazor;

namespace Rise.Client.Notifications
{
    internal static class NotificationHelper
    {
        public static Severity ToSeverity(string from) => from.ToLower() switch
        {
            "information" => Severity.Success,
            "warning" => Severity.Warning,
            "urgent" => Severity.Error,
            _ => Severity.Info,
        };
    }
}
