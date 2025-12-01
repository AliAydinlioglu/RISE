using Rise.Shared.UserPreferences;
using System.Text.Json;

namespace Rise.Services.UserPreferences
{
    public class PreferenceBag
    {
        public Dictionary<string, object> Values { get; }

        public PreferenceBag(string? json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                Values = new Dictionary<string, object>(UserPreferenceKeys.Defaults);
            }
            else
            {
                Values = JsonSerializer.Deserialize<Dictionary<string, object>>(json)
                    ?? new Dictionary<string, object>();

                foreach (var (key, def) in UserPreferenceKeys.Defaults)
                    Values.TryAdd(key, def);
            }
        }

        public string ToJson()
            => JsonSerializer.Serialize(Values);

        public string? GetSingle(string key)
        {
            if (!Values.TryGetValue(key, out var value))
                return null;

            return value?.ToString();
        }

        public void Merge(Dictionary<string, object> update)
        {
            foreach (var (key, value) in update)
                Values[key] = value;
        }
    }
}
