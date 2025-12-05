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
            public string Id { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public bool IsFavorite { get; set; }
        }
    }
}
