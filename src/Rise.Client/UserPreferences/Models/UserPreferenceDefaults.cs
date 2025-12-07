using Rise.Shared.UserPreferences;

namespace Rise.Client.UserPreferences.Models
{
    public static class UserPreferenceDefaults
    {
        public static readonly Dictionary<string, object> Values = new()
    {
        { UserPreferenceKeys.Theme, "light" },
        { UserPreferenceKeys.FontSize, 16 },
        { UserPreferenceKeys.Language, "nl" },
        { UserPreferenceKeys.FavoriteResto, "1" },
        { UserPreferenceKeys.ImagesOff, false },
        { UserPreferenceKeys.IsNeutral, false },
        { UserPreferenceKeys.IsRemote, false },
        { UserPreferenceKeys.NotifyDeadline, true },
        { UserPreferenceKeys.NotifySchoolEvent, true },
        { UserPreferenceKeys.NotifyEmergencies, true },
        { UserPreferenceKeys.NotifyCancelledClass, true }
    };
    }
}
