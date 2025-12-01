using System;
using System.Collections.Generic;
using System.Text.Json;
using Ardalis.GuardClauses;

namespace Rise.Domain.UserPreferences
{
    public class UserPreference : Entity
    {
        public Guid UserId { get; private set; }
        public string PreferencesJson { get; private set; } = "{}";
        public DateTime UpdatedAt { get; private set; }

        private UserPreference() { }

        public UserPreference(Guid userId, string preferencesJson)
        {
            UserId = Guard.Against.Default(userId, nameof(userId));
            PreferencesJson = Guard.Against.NullOrWhiteSpace(preferencesJson);
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePreferences(string preferencesJson)
        {
            PreferencesJson = Guard.Against.NullOrWhiteSpace(preferencesJson);
            UpdatedAt = DateTime.UtcNow;
        }

        public void MergePreferences(Dictionary<string, object> updates)
        {
            var existing = string.IsNullOrWhiteSpace(PreferencesJson) || PreferencesJson == "{}"
                ? new Dictionary<string, object>()
                : JsonSerializer.Deserialize<Dictionary<string, object>>(PreferencesJson)
                  ?? new Dictionary<string, object>();

            foreach (var (key, value) in updates)
            {
                existing[key] = value;
            }

            PreferencesJson = JsonSerializer.Serialize(existing);
            UpdatedAt = DateTime.UtcNow;
        }

        public string? GetSinglePreference(string key)
        {
            if (string.IsNullOrWhiteSpace(PreferencesJson) || PreferencesJson == "{}")
                return null;

            var preferences = JsonSerializer.Deserialize<Dictionary<string, object>>(PreferencesJson);
            if (preferences != null && preferences.TryGetValue(key, out var value))
            {
                if (value is JsonElement jsonElement)
                    return jsonElement.GetRawText();
                return value?.ToString();
            }

            return null;
        }
    }
}