namespace Rise.Shared.UserPreferences
{
    public static class UserPreferenceKeys
    {
        public const string Theme = "theme";
        public const string FontSize = "fontSize";
        public const string Language = "language";
        public const string ImagesOff = "imagesOff";
        public const string IsNeutral = "isNeutral";
        public const string IsRemote = "isRemote";

        public const string NotifyCancelledClass = "notifyCancelledClass";
        public const string NotifyDeadline = "notifyDeadline";
        public const string NotifyEmergencies = "notifyEmergencies";
        public const string NotifySchoolEvent = "notifySchoolEvent";

        public const string FavoriteResto = "favoriteResto";

        public static readonly Dictionary<string, object> Defaults = new()
        {
            [Theme] = "light",
            [FontSize] = 14,
            [Language] = "nl",
            [ImagesOff] = false,
            [IsNeutral] = true,
            [IsRemote] = false,
            [NotifyDeadline] = true,
            [NotifySchoolEvent] = true,
            [NotifyEmergencies] = true,
            [NotifyCancelledClass] = true,
            [FavoriteResto] = "Schoonmeersen B"
        };

        public static IEnumerable<string> AllKeys => Defaults.Keys;
    }
}
