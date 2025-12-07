using Rise.Domain.UserPreferences;
using System.Text.Json;

namespace Rise.Domain.Tests.UserPreferences;

public class UserPreferenceShould
{
    private readonly Guid _validUserId = Guid.NewGuid();

    [Fact]
    public void BeCreated()
    {
        var preferences = new Dictionary<string, object>
        {
            { "theme", "dark" }
        };

        var preference = new UserPreference(_validUserId, preferences);

        preference.UserId.ShouldBe(_validUserId);

        var result = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(preference.PreferencesJson);
        result.ShouldNotBeNull();
        result["theme"].GetString().ShouldBe("dark");

        preference.UpdatedAt.ShouldBeInRange(
            DateTime.UtcNow.AddSeconds(-1),
            DateTime.UtcNow.AddSeconds(1)
        );
    }

    [Fact]
    public void BeCreatedWithEmptyDictionary()
    {
        var preference = new UserPreference(_validUserId, new Dictionary<string, object>());

        preference.PreferencesJson.ShouldBe("{}");
    }

    [Fact]
    public void UpdatePreferences()
    {
        var initialPreferences = new Dictionary<string, object>
        {
            { "theme", "light" }
        };

        var preference = new UserPreference(_validUserId, initialPreferences);
        var originalUpdatedAt = preference.UpdatedAt;

        Thread.Sleep(10);

        var updates = new Dictionary<string, object>
        {
            { "theme", "dark" }
        };

        preference.UpdatePreferences(updates);

        var result = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(preference.PreferencesJson);
        result.ShouldNotBeNull();
        result["theme"].GetString().ShouldBe("dark");
        preference.UpdatedAt.ShouldBeGreaterThan(originalUpdatedAt);
    }

    [Fact]
    public void MergePreferences()
    {
        var initialPreferences = new Dictionary<string, object>
        {
            { "theme", "light" },
            { "fontSize", 14 },
            { "language", "nl" }
        };

        var preference = new UserPreference(_validUserId, initialPreferences);

        var updates = new Dictionary<string, object>
        {
            { "theme", "dark" }
        };

        preference.UpdatePreferences(updates);

        var result = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(preference.PreferencesJson);
        result.ShouldNotBeNull();
        result["theme"].GetString().ShouldBe("dark");
        result["fontSize"].GetInt32().ShouldBe(14);
        result["language"].GetString().ShouldBe("nl");
    }

    [Fact]
    public void MergePreferences_AddsNewKeys()
    {
        var initialPreferences = new Dictionary<string, object>
        {
            { "theme", "light" }
        };

        var preference = new UserPreference(_validUserId, initialPreferences);

        var updates = new Dictionary<string, object>
        {
            { "fontSize", 16 },
            { "language", "en" }
        };

        preference.UpdatePreferences(updates);

        var result = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(preference.PreferencesJson);
        result.ShouldNotBeNull();
        result["theme"].GetString().ShouldBe("light");
        result["fontSize"].GetInt32().ShouldBe(16);
        result["language"].GetString().ShouldBe("en");
        result.Count.ShouldBe(3);
    }

    [Fact]
    public void MergePreferences_UpdatesExistingKeys()
    {
        var initialPreferences = new Dictionary<string, object>
        {
            { "theme", "light" },
            { "fontSize", 14 }
        };

        var preference = new UserPreference(_validUserId, initialPreferences);

        var updates = new Dictionary<string, object>
        {
            { "theme", "dark" },
            { "fontSize", 18 }
        };

        preference.UpdatePreferences(updates);

        var result = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(preference.PreferencesJson);
        result.ShouldNotBeNull();
        result["theme"].GetString().ShouldBe("dark");
        result["fontSize"].GetInt32().ShouldBe(18);
    }

    [Fact]
    public void MergePreferences_HandlesEmptyInitialDictionary()
    {
        var preference = new UserPreference(_validUserId, new Dictionary<string, object>());

        var updates = new Dictionary<string, object>
        {
            { "theme", "dark" },
            { "fontSize", 16 }
        };

        preference.UpdatePreferences(updates);

        var result = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(preference.PreferencesJson);
        result.ShouldNotBeNull();
        result["theme"].GetString().ShouldBe("dark");
        result["fontSize"].GetInt32().ShouldBe(16);
    }

    [Fact]
    public void GetSinglePreference_ReturnsValue()
    {
        var preferences = new Dictionary<string, object>
        {
            { "theme", "dark" },
            { "fontSize", 16 }
        };

        var preference = new UserPreference(_validUserId, preferences);

        var themeValue = preference.GetSinglePreference("theme");
        themeValue.ShouldNotBeNull();

        var fontSizeValue = preference.GetSinglePreference("fontSize");
        fontSizeValue.ShouldNotBeNull();
    }

    [Fact]
    public void GetSinglePreference_ReturnsNullForNonExistentKey()
    {
        var preferences = new Dictionary<string, object>
        {
            { "theme", "dark" }
        };

        var preference = new UserPreference(_validUserId, preferences);

        var value = preference.GetSinglePreference("nonExistentKey");
        value.ShouldBeNull();
    }

    [Fact]
    public void GetSinglePreference_ReturnsNullForEmptyDictionary()
    {
        var preference = new UserPreference(_validUserId, new Dictionary<string, object>());

        var value = preference.GetSinglePreference("theme");
        value.ShouldBeNull();
    }

    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    public void ThrowExceptionWhenUserIdIsEmpty(string userIdStr)
    {
        var userId = Guid.Parse(userIdStr);
        var preferences = new Dictionary<string, object> { { "theme", "dark" } };

        var exception = Should.Throw<ArgumentException>(() => new UserPreference(userId, preferences));

        exception.GetType().ShouldBe(typeof(ArgumentException));
    }

    [Fact]
    public void ThrowExceptionWhenPreferencesDictionaryIsNull()
    {
        var exception = Should.Throw<ArgumentNullException>(() => new UserPreference(_validUserId, null!));

        exception.GetType().ShouldBe(typeof(ArgumentNullException));
    }

    [Fact]
    public void ThrowExceptionWhenUpdatePreferencesDictionaryIsNull()
    {
        var preference = new UserPreference(_validUserId, new Dictionary<string, object>());

        var exception = Should.Throw<ArgumentNullException>(() => preference.UpdatePreferences(null!));

        exception.GetType().ShouldBe(typeof(ArgumentNullException));
    }

    [Theory]
    [InlineData("theme", "light")]
    [InlineData("fontSize", 16)]
    [InlineData("language", "nl")]
    [InlineData("notifications", true)]
    public void SupportDifferentPreferenceTypes(string key, object value)
    {
        var preferences = new Dictionary<string, object>
        {
            { key, value }
        };

        var preference = new UserPreference(_validUserId, preferences);

        preference.PreferencesJson.ShouldContain(key);
    }

    [Fact]
    public void SupportArrayPreferences()
    {
        var preferences = new Dictionary<string, object>
        {
            { "notifyDeadline", new[] { "push", "email" } }
        };

        var preference = new UserPreference(_validUserId, preferences);

        preference.PreferencesJson.ShouldContain("notifyDeadline");
        preference.PreferencesJson.ShouldContain("push");
        preference.PreferencesJson.ShouldContain("email");
    }

    [Fact]
    public void MaintainUserIdAfterUpdate()
    {
        var initialPreferences = new Dictionary<string, object>
        {
            { "theme", "light" }
        };

        var preference = new UserPreference(_validUserId, initialPreferences);

        var updates = new Dictionary<string, object>
        {
            { "theme", "dark" }
        };

        preference.UpdatePreferences(updates);

        preference.UserId.ShouldBe(_validUserId);
    }

    [Fact]
    public void UpdateTimestampOnUpdate()
    {
        var initialPreferences = new Dictionary<string, object>
        {
            { "theme", "light" }
        };

        var preference = new UserPreference(_validUserId, initialPreferences);
        var originalUpdatedAt = preference.UpdatedAt;

        Thread.Sleep(10);

        var updates = new Dictionary<string, object>
        {
            { "theme", "dark" }
        };

        preference.UpdatePreferences(updates);

        preference.UpdatedAt.ShouldBeGreaterThan(originalUpdatedAt);
    }

    [Fact]
    public void HandleComplexNestedPreferences()
    {
        var preferences = new Dictionary<string, object>
        {
            { "theme", "dark" },
            { "fontSize", 16 },
            { "language", "nl" },
            { "notifications", new Dictionary<string, object>
                {
                    { "deadline", new[] { "push", "email" } },
                    { "events", new[] { "push" } }
                }
            }
        };

        var preference = new UserPreference(_validUserId, preferences);

        preference.PreferencesJson.ShouldContain("notifications");
        preference.PreferencesJson.ShouldContain("deadline");
    }

    [Fact]
    public void ThrowExceptionWhenJsonIsCorrupted_OnUpdate()
    {
        var preference = new UserPreference(_validUserId, new Dictionary<string, object> { { "theme", "light" } });

        var field = typeof(UserPreference).GetField("<PreferencesJson>k__BackingField",
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        field!.SetValue(preference, "{invalid json}");

        var updates = new Dictionary<string, object> { { "fontSize", 16 } };

        var exception = Should.Throw<InvalidOperationException>(() => preference.UpdatePreferences(updates));
        exception.Message.ShouldContain("corrupted");
    }

    [Fact]
    public void GetSinglePreference_ReturnsNullWhenJsonIsCorrupted()
    {
        var preference = new UserPreference(_validUserId, new Dictionary<string, object> { { "theme", "light" } });

        var field = typeof(UserPreference).GetField("<PreferencesJson>k__BackingField",
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        field!.SetValue(preference, "{invalid json}");

        var value = preference.GetSinglePreference("theme");
        value.ShouldBeNull();
    }
}