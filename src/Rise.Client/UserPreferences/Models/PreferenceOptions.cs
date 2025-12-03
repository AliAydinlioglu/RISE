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

        public static readonly List<CampusOption> Campuses = new()
    {
        new() { Id = "1", Name = "Ledeganck" },
        new() { Id = "2", Name = "Mercator" },
        new() { Id = "3", Name = "Schoonmeersen B" },
        new() { Id = "4", Name = "Bijloke" },
        new() { Id = "3", Name = "Schoonmeersen P" },
        new() { Id = "5", Name = "Schoonmeersen D" }

    };

        public record LanguageOption
        {
            public string Code { get; set; } = string.Empty;
            public string DisplayName { get; set; } = string.Empty;
        }

        public record CampusOption
        {
            public string Id { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
        }
    }
}
