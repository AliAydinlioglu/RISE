using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Rise.Persistence;
using Rise.Services.Identity;
using Rise.Services.UserPreferences;
using Rise.Shared.UserPreferences;
using System.Security.Claims;
using System.Text.Json;
using Ardalis.Result;
using Rise.Shared.Identity;

namespace Rise.Services.Tests.UserPreferences;

public class UserPreferenceServiceShould
{
    private readonly Guid _testUserId = Guid.Parse("08de294a-2705-4195-860d-ff69f2452f67");
    private readonly Guid _testSsoId = Guid.Parse("28a27ff1-c348-44d2-a102-4b46e29bbff6");
    private readonly string _testUserEmail = "test@example.com";
    private readonly ISessionContextProvider _sessionContextProvider;
    private readonly IUserService userService;


    public UserPreferenceServiceShould()
    {
        _sessionContextProvider = Substitute.For<ISessionContextProvider>();
        userService = Substitute.For<IUserService>(); 

        userService.GetOrCreateUserAsync(Arg.Any<string>())
            .Returns(Task.FromResult(Result.Success(new Shared.Identity.Accounts.AccountResponse.LoginCallback
            {
                Email = _testUserEmail,
                Roles = Array.Empty<string>()
            })));
    }

    private ApplicationDbContext CreateDbContext(string databaseName)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: databaseName)
            .Options;

        return new ApplicationDbContext(options);
    }

    private Rise.Persistence.Models.Identity.ApplicationUser CreateTestUser()
    {
        return new Rise.Persistence.Models.Identity.ApplicationUser(
            email: _testUserEmail,
            firstName: "Test",
            lastName: "User",
            classGroup: null,
            lastLogin: DateTimeOffset.UtcNow,
            ssoId: _testSsoId,
            ssoProvider: "MicrosoftEntra"
        );
    }

    private void SetupAuthenticatedUser(string email)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "test-sso-id"),
            new Claim(ClaimTypes.Email, email),
            new Claim("preferred_username", email)
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        _sessionContextProvider.User.Returns(claimsPrincipal);
    }

    [Fact]
    public async Task GetPreferences_ReturnsDefaultValuesWhenNoPreferencesExist()
    {
        // Arrange
        using var dbContext = CreateDbContext(nameof(GetPreferences_ReturnsDefaultValuesWhenNoPreferencesExist));

        var user = CreateTestUser();
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        SetupAuthenticatedUser(_testUserEmail);

        var service = new UserPreferenceService(dbContext, _sessionContextProvider,userService);

        // Act
        var result = await service.GetPreferencesAsync(CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.UserPreferences.Settings[UserPreferenceKeys.Theme].ToString().ShouldBe("light");
        result.Value.UserPreferences.Settings[UserPreferenceKeys.FontSize].ToString().ShouldBe("14");
        result.Value.UserPreferences.Settings[UserPreferenceKeys.Language].ToString().ShouldBe("nl");
        result.Value.UserPreferences.Settings[UserPreferenceKeys.NotifyDeadline].ToString().ShouldBe("True");
        result.Value.UserPreferences.LastUpdated.ShouldBeNull();
    }

    [Fact]
    public async Task GetPreferences_ReturnsUserPreferences()
    {
        // Arrange
        using var dbContext = CreateDbContext(nameof(GetPreferences_ReturnsUserPreferences));

        var user = CreateTestUser();
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        var savedUser = await dbContext.Users.FirstAsync(u => u.Email == _testUserEmail);

        // Create JSON preference
        var preferencesJson = JsonSerializer.Serialize(new Dictionary<string, object>
        {
            { UserPreferenceKeys.Theme, "dark" },
            { UserPreferenceKeys.FontSize, 18 },
            { UserPreferenceKeys.Language, "en" },
            { UserPreferenceKeys.NotifyDeadline, false }
        });

        var preference = new Rise.Domain.UserPreferences.UserPreference(savedUser.Id, preferencesJson);
        dbContext.UserPreferences.Add(preference);
        await dbContext.SaveChangesAsync();

        SetupAuthenticatedUser(_testUserEmail);

        var service = new UserPreferenceService(dbContext, _sessionContextProvider, userService);

        // Act
        var result = await service.GetPreferencesAsync(CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();

        var settings = result.Value.UserPreferences.Settings;

        // Handle JsonElement deserialization
        GetSettingValue<string>(settings, UserPreferenceKeys.Theme).ShouldBe("dark");
        GetSettingValue<int>(settings, UserPreferenceKeys.FontSize).ShouldBe(18);
        GetSettingValue<string>(settings, UserPreferenceKeys.Language).ShouldBe("en");
        GetSettingValue<bool>(settings, UserPreferenceKeys.NotifyDeadline).ShouldBe(false);
    }

    [Fact]
    public async Task GetPreferences_ReturnsUnauthorizedWhenUserNotAuthenticated()
    {
        // Arrange
        using var dbContext = CreateDbContext(nameof(GetPreferences_ReturnsUnauthorizedWhenUserNotAuthenticated));

        _sessionContextProvider.User.Returns((ClaimsPrincipal)null);

        var service = new UserPreferenceService(dbContext, _sessionContextProvider, userService);

        // Act
        var result = await service.GetPreferencesAsync(CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Unauthorized);
    }

    [Fact]
    public async Task UpdatePreferences_CreatesNewPreferences()
    {
        // Arrange
        using var dbContext = CreateDbContext(nameof(UpdatePreferences_CreatesNewPreferences));

        var user = CreateTestUser();
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        var savedUser = await dbContext.Users.FirstAsync(u => u.Email == _testUserEmail);

        SetupAuthenticatedUser(_testUserEmail);

        var service = new UserPreferenceService(dbContext, _sessionContextProvider, userService);

        var settings = new Dictionary<string, object>
        {
            { UserPreferenceKeys.Theme, "dark" },
            { UserPreferenceKeys.FontSize, 16 },
            { UserPreferenceKeys.Language, "en" },
            { UserPreferenceKeys.NotifyDeadline, false }
        };

        // Act
        var result = await service.UpdatePreferencesAsync(settings, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();

        var savedPreference = await dbContext.UserPreferences
            .FirstOrDefaultAsync(p => p.UserId == savedUser.Id);

        savedPreference.ShouldNotBeNull();

        // Parse JSON and verify
        var savedSettings = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(savedPreference.PreferencesJson);
        savedSettings.ShouldNotBeNull();
        savedSettings[UserPreferenceKeys.Theme].GetString().ShouldBe("dark");
        savedSettings[UserPreferenceKeys.FontSize].GetInt32().ShouldBe(16);
        savedSettings[UserPreferenceKeys.Language].GetString().ShouldBe("en");
        savedSettings[UserPreferenceKeys.NotifyDeadline].GetBoolean().ShouldBe(false);
    }

    [Fact]
    public async Task UpdatePreferences_UpdatesExistingPreferences()
    {
        // Arrange
        using var dbContext = CreateDbContext(nameof(UpdatePreferences_UpdatesExistingPreferences));

        var user = CreateTestUser();
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        var savedUser = await dbContext.Users.FirstAsync(u => u.Email == _testUserEmail);

        var initialJson = JsonSerializer.Serialize(new Dictionary<string, object>
        {
            { UserPreferenceKeys.Theme, "light" },
            { UserPreferenceKeys.FontSize, 14 }
        });

        var existingPreference = new Rise.Domain.UserPreferences.UserPreference(savedUser.Id, initialJson);
        dbContext.UserPreferences.Add(existingPreference);
        await dbContext.SaveChangesAsync();

        SetupAuthenticatedUser(_testUserEmail);

        var service = new UserPreferenceService(dbContext, _sessionContextProvider, userService);

        var settings = new Dictionary<string, object>
        {
            { UserPreferenceKeys.Theme, "dark" }
        };

        // Act
        var result = await service.UpdatePreferencesAsync(settings, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();

        var updatedPreference = await dbContext.UserPreferences
            .FirstOrDefaultAsync(p => p.UserId == savedUser.Id);

        updatedPreference.ShouldNotBeNull();

        var updatedSettings = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(updatedPreference.PreferencesJson);
        updatedSettings.ShouldNotBeNull();
        updatedSettings[UserPreferenceKeys.Theme].GetString().ShouldBe("dark");
        updatedSettings[UserPreferenceKeys.FontSize].GetInt32().ShouldBe(14); // Should be preserved
    }

    [Fact]
    public async Task UpdatePreferences_ReturnsUnauthorizedWhenUserNotAuthenticated()
    {
        // Arrange
        using var dbContext = CreateDbContext(nameof(UpdatePreferences_ReturnsUnauthorizedWhenUserNotAuthenticated));

        _sessionContextProvider.User.Returns((ClaimsPrincipal)null);

        var service = new UserPreferenceService(dbContext, _sessionContextProvider, userService);

        var settings = new Dictionary<string, object>
        {
            { UserPreferenceKeys.Theme, "dark" }
        };

        // Act
        var result = await service.UpdatePreferencesAsync(settings, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Unauthorized);
    }

    [Fact]
    public async Task UpdateSinglePreference_CreatesNewPreference()
    {
        // Arrange
        using var dbContext = CreateDbContext(nameof(UpdateSinglePreference_CreatesNewPreference));

        var user = CreateTestUser();
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        var savedUser = await dbContext.Users.FirstAsync(u => u.Email == _testUserEmail);

        SetupAuthenticatedUser(_testUserEmail);

        var service = new UserPreferenceService(dbContext, _sessionContextProvider, userService);

        // Act
        var result = await service.UpdateSinglePreferenceAsync(UserPreferenceKeys.Theme, "dark", CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();

        var savedPreference = await dbContext.UserPreferences
            .FirstOrDefaultAsync(p => p.UserId == savedUser.Id);

        savedPreference.ShouldNotBeNull();

        var savedSettings = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(savedPreference.PreferencesJson);
        savedSettings.ShouldNotBeNull();
        savedSettings[UserPreferenceKeys.Theme].GetString().ShouldBe("dark");
    }

    [Fact]
    public async Task UpdateSinglePreference_UpdatesExistingPreference()
    {
        // Arrange
        using var dbContext = CreateDbContext(nameof(UpdateSinglePreference_UpdatesExistingPreference));

        var user = CreateTestUser();
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        var savedUser = await dbContext.Users.FirstAsync(u => u.Email == _testUserEmail);

        var initialJson = JsonSerializer.Serialize(new Dictionary<string, object>
        {
            { UserPreferenceKeys.Theme, "light" }
        });

        var existingPreference = new Rise.Domain.UserPreferences.UserPreference(savedUser.Id, initialJson);
        dbContext.UserPreferences.Add(existingPreference);
        await dbContext.SaveChangesAsync();

        SetupAuthenticatedUser(_testUserEmail);

        var service = new UserPreferenceService(dbContext, _sessionContextProvider, userService);

        // Act
        var result = await service.UpdateSinglePreferenceAsync(UserPreferenceKeys.Theme, "dark", CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();

        var updatedPreference = await dbContext.UserPreferences
            .FirstOrDefaultAsync(p => p.UserId == savedUser.Id);

        updatedPreference.ShouldNotBeNull();

        var updatedSettings = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(updatedPreference.PreferencesJson);
        updatedSettings.ShouldNotBeNull();
        updatedSettings[UserPreferenceKeys.Theme].GetString().ShouldBe("dark");
    }

    [Theory]
    [InlineData("invalidKey")]
    [InlineData("random")]
    public async Task UpdateSinglePreference_ReturnsErrorForInvalidKey(string invalidKey)
    {
        // Arrange
        using var dbContext = CreateDbContext($"{nameof(UpdateSinglePreference_ReturnsErrorForInvalidKey)}_{invalidKey}");

        var user = CreateTestUser();
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        SetupAuthenticatedUser(_testUserEmail);

        var service = new UserPreferenceService(dbContext, _sessionContextProvider, userService);

        // Act
        var result = await service.UpdateSinglePreferenceAsync(invalidKey, "value", CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Error);
    }

    [Fact]
    public async Task UpdateSinglePreference_ReturnsUnauthorizedWhenUserNotAuthenticated()
    {
        // Arrange
        using var dbContext = CreateDbContext(nameof(UpdateSinglePreference_ReturnsUnauthorizedWhenUserNotAuthenticated));

        _sessionContextProvider.User.Returns((ClaimsPrincipal)null);

        var service = new UserPreferenceService(dbContext, _sessionContextProvider, userService);

        // Act
        var result = await service.UpdateSinglePreferenceAsync(UserPreferenceKeys.Theme, "dark", CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Unauthorized);
    }

    [Theory]
    [InlineData(UserPreferenceKeys.Theme, "dark")]
    [InlineData(UserPreferenceKeys.FontSize, "20")]
    [InlineData(UserPreferenceKeys.Language, "fr")]
    [InlineData(UserPreferenceKeys.NotifyDeadline, "true")]
    [InlineData(UserPreferenceKeys.ImagesOff, "true")]
    [InlineData(UserPreferenceKeys.IsNeutral, "false")]
    [InlineData(UserPreferenceKeys.IsRemote, "true")]
    [InlineData(UserPreferenceKeys.NotifySchoolEvent, "false")]
    [InlineData(UserPreferenceKeys.NotifyEmergencies, "true")]
    [InlineData(UserPreferenceKeys.NotifyCancelledClass, "false")]
    [InlineData(UserPreferenceKeys.FavoriteResto, "2")]
    public async Task UpdateSinglePreference_SupportsAllValidKeys(string key, string value)
    {
        // Arrange
        using var dbContext = CreateDbContext($"{nameof(UpdateSinglePreference_SupportsAllValidKeys)}_{key}");

        var user = CreateTestUser();
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        var savedUser = await dbContext.Users.FirstAsync(u => u.Email == _testUserEmail);

        SetupAuthenticatedUser(_testUserEmail);

        var service = new UserPreferenceService(dbContext, _sessionContextProvider, userService);

        // Act
        var result = await service.UpdateSinglePreferenceAsync(key, value, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();

        var savedPreference = await dbContext.UserPreferences
            .FirstOrDefaultAsync(p => p.UserId == savedUser.Id);

        savedPreference.ShouldNotBeNull();
        savedPreference.PreferencesJson.ShouldContain(key);
    }

    [Fact]
    public async Task UpdatePreferences_HandlesAllPreferenceKeys()
    {
        // Arrange
        using var dbContext = CreateDbContext(nameof(UpdatePreferences_HandlesAllPreferenceKeys));

        var user = CreateTestUser();
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        var savedUser = await dbContext.Users.FirstAsync(u => u.Email == _testUserEmail);

        SetupAuthenticatedUser(_testUserEmail);

        var service = new UserPreferenceService(dbContext, _sessionContextProvider, userService);

        var settings = new Dictionary<string, object>
        {
            { UserPreferenceKeys.Theme, "dark" },
            { UserPreferenceKeys.FontSize, 18 },
            { UserPreferenceKeys.Language, "en" },
            { UserPreferenceKeys.FavoriteResto, "2" },
            { UserPreferenceKeys.ImagesOff, true },
            { UserPreferenceKeys.IsNeutral, false },
            { UserPreferenceKeys.IsRemote, true },
            { UserPreferenceKeys.NotifyDeadline, true },
            { UserPreferenceKeys.NotifySchoolEvent, false },
            { UserPreferenceKeys.NotifyEmergencies, true },
            { UserPreferenceKeys.NotifyCancelledClass, false }
        };

        // Act
        var result = await service.UpdatePreferencesAsync(settings, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();

        var savedPreference = await dbContext.UserPreferences
            .FirstOrDefaultAsync(p => p.UserId == savedUser.Id);

        savedPreference.ShouldNotBeNull();

        var savedSettings = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(savedPreference.PreferencesJson);
        savedSettings.ShouldNotBeNull();
        savedSettings.Count.ShouldBe(11);
    }

    [Fact]
    public async Task UpdatePreferences_MergesPartialUpdates()
    {
        // Arrange
        using var dbContext = CreateDbContext(nameof(UpdatePreferences_MergesPartialUpdates));

        var user = CreateTestUser();
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        var savedUser = await dbContext.Users.FirstAsync(u => u.Email == _testUserEmail);

        var initialJson = JsonSerializer.Serialize(new Dictionary<string, object>
        {
            { UserPreferenceKeys.Theme, "light" },
            { UserPreferenceKeys.FontSize, 14 },
            { UserPreferenceKeys.Language, "nl" }
        });

        var existingPreference = new Rise.Domain.UserPreferences.UserPreference(savedUser.Id, initialJson);
        dbContext.UserPreferences.Add(existingPreference);
        await dbContext.SaveChangesAsync();

        SetupAuthenticatedUser(_testUserEmail);

        var service = new UserPreferenceService(dbContext, _sessionContextProvider, userService);

        // Only update theme
        var settings = new Dictionary<string, object>
        {
            { UserPreferenceKeys.Theme, "dark" }
        };

        // Act
        var result = await service.UpdatePreferencesAsync(settings, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();

        var updatedPreference = await dbContext.UserPreferences
            .FirstOrDefaultAsync(p => p.UserId == savedUser.Id);

        updatedPreference.ShouldNotBeNull();

        var updatedSettings = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(updatedPreference.PreferencesJson);
        updatedSettings.ShouldNotBeNull();

        // Theme should be updated
        updatedSettings[UserPreferenceKeys.Theme].GetString().ShouldBe("dark");

        // Other settings should be preserved
        updatedSettings[UserPreferenceKeys.FontSize].GetInt32().ShouldBe(14);
        updatedSettings[UserPreferenceKeys.Language].GetString().ShouldBe("nl");
    }

    // Helper method to extract typed values from Settings dictionary
    private T GetSettingValue<T>(Dictionary<string, object> settings, string key)
    {
        var value = settings[key];

        if (value is JsonElement jsonElement)
        {
            if (typeof(T) == typeof(string))
                return (T)(object)jsonElement.GetString()!;
            if (typeof(T) == typeof(int))
                return (T)(object)jsonElement.GetInt32();
            if (typeof(T) == typeof(bool))
                return (T)(object)jsonElement.GetBoolean();
        }

        return (T)Convert.ChangeType(value, typeof(T));
    }
}