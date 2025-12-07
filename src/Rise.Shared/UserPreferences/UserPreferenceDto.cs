using System.Text.Json;

namespace Rise.Shared.UserPreferences
{
    public static class UserPreferenceDto
    {
        public class Preferences
        {
            public Dictionary<string, object> Settings { get; set; } = new();
            public DateTime? LastUpdated { get; set; }

            public T GetValue<T>(string key)
            {
                if (Settings.TryGetValue(key, out var value))
                {
                    if (value is JsonElement jsonElement)
                    {
                        return JsonSerializer.Deserialize<T>(jsonElement.ToString()!)!;
                    }
                    return (T)Convert.ChangeType(value, typeof(T));
                }

                if (UserPreferenceKeys.Defaults.TryGetValue(key, out var defaultValue))
                {
                    return (T)Convert.ChangeType(defaultValue, typeof(T));
                }

                return default!;
            }

            public void SetValue<T>(string key, T value)
            {
                Settings[key] = value!;
            }
        }
    }
}