using Rise.Domain.UserPreferences;
using System.Text.Json;

namespace Rise.Domain.Tests.UserPreferences;

public class UserPreferenceShould
{
    private readonly Guid _validUserId = Guid.NewGuid();

    [Fact]
    public void BeCreated()
    {
        var preferencesJson = JsonSerializer.Serialize(new Dictionary<string, object>
        {
            { "theme", "dark" }
        });

        var preference = new UserPreference(
            _validUserId,
            preferencesJson
        );

        preference.UserId.ShouldBe(_validUserId);
        preference.PreferencesJson.ShouldBe(preferencesJson);
        preference.UpdatedAt.ShouldBeInRange(
            DateTime.UtcNow.AddSeconds(-1),
            DateTime.UtcNow.AddSeconds(1)
        );
    }

    [Fact]
    public void BeCreatedWithEmptyJson()
    {
        var preference = new UserPreference(
            _validUserId,
            "{}"
        );

        preference.PreferencesJson.ShouldBe("{}");
    }

    [Fact]
    public void UpdatePreferences()
    {
        var initialJson = JsonSerializer.Serialize(new Dictionary<string, object>
        {
            { "theme", "light" }
        });

        var preference = new UserPreference(
            _validUserId,
            initialJson
        );

        var originalUpdatedAt = preference.UpdatedAt;

        // Wacht een klein beetje zodat UpdatedAt anders is
        Thread.Sleep(10);

        var newJson = JsonSerializer.Serialize(new Dictionary<string, object>
        {
            { "theme", "dark" }
        });

        preference.UpdatePreferences(newJson);

        preference.PreferencesJson.ShouldBe(newJson);
        preference.UpdatedAt.ShouldBeGreaterThan(originalUpdatedAt);
    }

    [Fact]
    public void MergePreferences()
    {
        var initialJson = JsonSerializer.Serialize(new Dictionary<string, object>
        {
            { "theme", "light" },
            { "fontSize", 14 },
            { "language", "nl" }
        });

        var preference = new UserPreference(
            _validUserId,
            initialJson
        );

        var updates = new Dictionary<string, object>
        {
            { "theme", "dark" }
        };

        preference.MergePreferences(updates);

        var result = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(preference.PreferencesJson);
        result.ShouldNotBeNull();
        result["theme"].GetString().ShouldBe("dark");
        result["fontSize"].GetInt32().ShouldBe(14);
        result["language"].GetString().ShouldBe("nl");
    }

    [Fact]
    public void MergePreferences_AddsNewKeys()
    {
        var initialJson = JsonSerializer.Serialize(new Dictionary<string, object>
        {
            { "theme", "light" }
        });

        var preference = new UserPreference(
            _validUserId,
            initialJson
        );

        var updates = new Dictionary<string, object>
        {
            { "fontSize", 16 },
            { "language", "en" }
        };

        preference.MergePreferences(updates);

        var result = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(preference.PreferencesJson);
        result.ShouldNotBeNull();
        result["theme"].GetString().ShouldBe("light");
        result["fontSize"].GetInt32().GetType().ShouldBe(typeof(int));
        result["language"].GetString().ShouldBe("en");
        result.Count.ShouldBe(3);
    }

    [Fact]
    public void MergePreferences_UpdatesExistingKeys()
    {
        var initialJson = JsonSerializer.Serialize(new Dictionary<string, object>
        {
            { "theme", "light" },
            { "fontSize", 14 }
        });

        var preference = new UserPreference(
            _validUserId,
            initialJson
        );

        var updates = new Dictionary<string, object>
        {
            { "theme", "dark" },
            { "fontSize", 18 }
        };

        preference.MergePreferences(updates);

        var result = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(preference.PreferencesJson);
        result.ShouldNotBeNull();
        result["theme"].GetString().ShouldBe("dark");
        result["fontSize"].GetInt32().ShouldBe(18);
    }

    [Fact]
    public void MergePreferences_HandlesEmptyInitialJson()
    {
        var preference = new UserPreference(
            _validUserId,
            "{}"
        );

        var updates = new Dictionary<string, object>
        {
            { "theme", "dark" },
            { "fontSize", 16 }
        };

        preference.MergePreferences(updates);

        var result = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(preference.PreferencesJson);
        result.ShouldNotBeNull();
        result["theme"].GetString().ShouldBe("dark");
        result["fontSize"].GetInt32().ShouldBe(16);
    }

    [Fact]
    public void GetSinglePreference_ReturnsValue()
    {
        var preferencesJson = JsonSerializer.Serialize(new Dictionary<string, object>
        {
            { "theme", "dark" },
            { "fontSize", 16 }
        });

        var preference = new UserPreference(
            _validUserId,
            preferencesJson
        );

        var themeValue = preference.GetSinglePreference("theme");
        themeValue.ShouldNotBeNull();

        var fontSizeValue = preference.GetSinglePreference("fontSize");
        fontSizeValue.ShouldNotBeNull();
    }

    [Fact]
    public void GetSinglePreference_ReturnsNullForNonExistentKey()
    {
        var preferencesJson = JsonSerializer.Serialize(new Dictionary<string, object>
        {
            { "theme", "dark" }
        });

        var preference = new UserPreference(
            _validUserId,
            preferencesJson
        );

        var value = preference.GetSinglePreference("nonExistentKey");
        value.ShouldBeNull();
    }

    [Fact]
    public void GetSinglePreference_ReturnsNullForEmptyJson()
    {
        var preference = new UserPreference(
            _validUserId,
            "{}"
        );

        var value = preference.GetSinglePreference("theme");
        value.ShouldBeNull();
    }

    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    public void ThrowExceptionWhenUserIdIsEmpty(string userIdStr)
    {
        var userId = Guid.Parse(userIdStr);
        var json = JsonSerializer.Serialize(new Dictionary<string, object> { { "theme", "dark" } });

        var exception = Should.Throw<ArgumentException>(() => new UserPreference(
            userId,
            json
        ));

        exception.GetType().ShouldBe(typeof(ArgumentException));
    }

    [Theory]
    [InlineData(null, typeof(ArgumentNullException))]
    [InlineData("", typeof(ArgumentException))]
    [InlineData("   ", typeof(ArgumentException))]
    public void ThrowExceptionWhenPreferencesJsonIsNullOrWhiteSpace(string? json, Type expectedExceptionType)
    {
        var exception = Should.Throw<Exception>(() => new UserPreference(
            _validUserId,
            json!
        ));

        exception.GetType().ShouldBe(expectedExceptionType);
    }

    [Fact]
    public void ThrowExceptionWhenUpdatePreferencesJsonIsNull()
    {
        var preference = new UserPreference(
            _validUserId,
            "{}"
        );

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
        var preferencesJson = JsonSerializer.Serialize(new Dictionary<string, object>
        {
            { key, value }
        });

        var preference = new UserPreference(
            _validUserId,
            preferencesJson
        );

        preference.PreferencesJson.ShouldContain(key);
    }

    [Fact]
    public void SupportArrayPreferences()
    {
        var preferencesJson = JsonSerializer.Serialize(new Dictionary<string, object>
        {
            { "notifyDeadline", new[] { "push", "email" } }
        });

        var preference = new UserPreference(
            _validUserId,
            preferencesJson
        );

        preference.PreferencesJson.ShouldContain("notifyDeadline");
        preference.PreferencesJson.ShouldContain("push");
        preference.PreferencesJson.ShouldContain("email");
    }

    [Fact]
    public void MaintainUserIdAfterUpdate()
    {
        var initialJson = JsonSerializer.Serialize(new Dictionary<string, object>
        {
            { "theme", "light" }
        });

        var preference = new UserPreference(
            _validUserId,
            initialJson
        );

        var newJson = JsonSerializer.Serialize(new Dictionary<string, object>
        {
            { "theme", "dark" }
        });

        preference.UpdatePreferences(newJson);

        preference.UserId.ShouldBe(_validUserId);
    }

    [Fact]
    public void MaintainUserIdAfterMerge()
    {
        var initialJson = JsonSerializer.Serialize(new Dictionary<string, object>
        {
            { "theme", "light" }
        });

        var preference = new UserPreference(
            _validUserId,
            initialJson
        );

        var updates = new Dictionary<string, object>
        {
            { "theme", "dark" }
        };

        preference.MergePreferences(updates);

        preference.UserId.ShouldBe(_validUserId);
    }

    [Fact]
    public void UpdateTimestampOnMerge()
    {
        var initialJson = JsonSerializer.Serialize(new Dictionary<string, object>
        {
            { "theme", "light" }
        });

        var preference = new UserPreference(
            _validUserId,
            initialJson
        );

        var originalUpdatedAt = preference.UpdatedAt;

        Thread.Sleep(10);

        var updates = new Dictionary<string, object>
        {
            { "theme", "dark" }
        };

        preference.MergePreferences(updates);

        preference.UpdatedAt.ShouldBeGreaterThan(originalUpdatedAt);
    }

    [Fact]
    public void HandleComplexNestedPreferences()
    {
        var preferencesJson = JsonSerializer.Serialize(new Dictionary<string, object>
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
        });

        var preference = new UserPreference(
            _validUserId,
            preferencesJson
        );

        preference.PreferencesJson.ShouldContain("notifications");
        preference.PreferencesJson.ShouldContain("deadline");
    }
}