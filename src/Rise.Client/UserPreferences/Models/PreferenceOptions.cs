namespace Rise.Client.UserPreferences.Models
{
    public static class PreferenceOptions
    {
        public static readonly List<LanguageOption> Languages = new()
    {
        new() { Code = "nl", DisplayName = "Nederlands" },
        new() { Code = "en", DisplayName = "English" },
        new() { Code = "fr", DisplayName = "Français" }
    };

       
        public record LanguageOption
        {
            public string Code { get; set; } = string.Empty;
            public string DisplayName { get; set; } = string.Empty;
        }

        public record RestoOption
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public bool IsFavorite { get; set; }
        }

        public static string GetNotificationTypeDisplayName(string notificationType) =>
            notificationType switch
            {
                "Deadline" => "Deadlines",
                "SchoolEvent" => "School evenementen",
                "Emergency" => "Noodmeldingen",
                "CancelledClass" => "Geannuleerde lessen",
                "LectorAbsence" => "Afwezigheid lectoren",
                _ => notificationType
            };

        public static string GetChannelDisplayName(string channelName) =>
            channelName switch
            {
                "InApp" => "In-app notificatie",
                "PushNotification" => "Push notificatie",
                _ => channelName
            };
    }
}
