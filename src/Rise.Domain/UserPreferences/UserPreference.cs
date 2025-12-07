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

        private UserPreference() { }

        public UserPreference(Guid userId, Dictionary<string, object> preferences)
        {
            UserId = Guard.Against.Default(userId, nameof(userId));
            Guard.Against.Null(preferences, nameof(preferences));
            PreferencesJson = JsonSerializer.Serialize(preferences);
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePreferences(Dictionary<string, object> updates)
        {
            Guard.Against.Null(updates, nameof(updates));

            Dictionary<string, object> existing;
            try
            {
                existing = string.IsNullOrWhiteSpace(PreferencesJson) || PreferencesJson == "{}"
                    ? new Dictionary<string, object>()
                    : JsonSerializer.Deserialize<Dictionary<string, object>>(PreferencesJson)
                      ?? new Dictionary<string, object>();
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException("Stored preferences JSON is corrupted.", ex);
            }

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

            try
            {
                var preferences = JsonSerializer.Deserialize<Dictionary<string, object>>(PreferencesJson);
                if (preferences != null && preferences.TryGetValue(key, out var value))
                {
                    if (value is JsonElement jsonElement)
                        return jsonElement.GetRawText();
                    return value?.ToString();
                }
                return null;
            }
            catch (JsonException)
            {
                return null;
            }
        }
    }
}