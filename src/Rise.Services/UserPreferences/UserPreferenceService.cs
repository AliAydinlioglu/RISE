using Microsoft.EntityFrameworkCore;
using Rise.Domain.UserPreferences;
using Rise.Persistence;
using Rise.Shared.Common;
using Rise.Shared.Identity;
using Rise.Shared.UserPreferences;
using System.Text.Json;

namespace Rise.Services.UserPreferences;

public class UserPreferenceService(
    ApplicationDbContext dbContext,
    IUserService userService)
    : IUserPreferenceService
{

        var userId = await userService.TryGetCurrentUserIdAsync();
        return userId.Value;
    }

    private async Task<Dictionary<string, object>> TryLoadPreferencesAsync(Guid userId, CancellationToken ctx)
    {
        try
        {
            var entity = await dbContext.UserPreferences
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.UserId == userId, ctx);

            if (entity == null || string.IsNullOrWhiteSpace(entity.PreferencesJson) || entity.PreferencesJson == "{}")
                return new Dictionary<string, object>();

            return JsonSerializer.Deserialize<Dictionary<string, object>>(entity.PreferencesJson)
                ?? new Dictionary<string, object>();
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load user preferences.", ex);
        }
    }

    public async Task<Result<UserPreferenceResponse.Preferences>> TryGetPreferencesAsync(CancellationToken ctx)
    {
        try
        {
            var userIdResult = await userService.TryGetCurrentUserIdAsync();

            if (!userIdResult.IsSuccess)
                return Result<UserPreferenceResponse.Preferences>.Unauthorized("User not authenticated.");

            var preferences = await TryLoadPreferencesAsync(userIdResult.Value, ctx);

            return Result.Success(new UserPreferenceResponse.Preferences
            {
                UserPreferences = new UserPreferenceDto.Preferences
                {
                    Settings = preferences,
                    LastUpdated = null
                }
            });
        }
        catch (UnauthorizedAccessException)
        {
            return Result<UserPreferenceResponse.Preferences>.Unauthorized("User not authenticated.");
        }
        catch (Exception ex)
        {
            return Result<UserPreferenceResponse.Preferences>.Error($"Failed to get preferences: {ex.Message}");
        }
    }

    public async Task<Result<string>> TryGetSinglePreferenceAsync(string key, CancellationToken ctx)
    {
        try
        {
            var userIdResult = await userService.TryGetCurrentUserIdAsync();

            if (!userIdResult.IsSuccess)
                return Result<string>.Unauthorized("User not authenticated.");

            if (!UserPreferenceKeys.AllKeys.Contains(key))
                return Result<string>.Error($"Invalid preference key: {key}");

            var preferences = await TryLoadPreferencesAsync(userIdResult.Value, ctx);

            if (preferences.TryGetValue(key, out var value))
            {
                if (value is JsonElement jsonElement)
                    return Result<string>.Success(jsonElement.GetRawText());

                return Result<string>.Success(value?.ToString() ?? UserPreferenceKeys.Defaults[key].ToString()!);
            }

            return Result<string>.Success(UserPreferenceKeys.Defaults[key].ToString()!);
        }
        catch (UnauthorizedAccessException)
        {
            return Result<string>.Unauthorized("User not authenticated.");
        }
        catch (Exception ex)
        {
            return Result<string>.Error($"Failed to get single preference: {ex.Message}");
        }
    }

    public async Task<Result> TryUpdatePreferencesAsync(
        Dictionary<string, object> updates,
        CancellationToken ctx)
    {
        try
        {
            var userIdResult = await userService.TryGetCurrentUserIdAsync();

            if (!userIdResult.IsSuccess)
                return Result.Unauthorized("User not authenticated.");

            foreach (var key in updates.Keys)
            {
                if (!UserPreferenceKeys.AllKeys.Contains(key))
                    return Result.Error($"Invalid preference key: {key}");
            }

            var entity = await dbContext.UserPreferences
                .FirstOrDefaultAsync(p => p.UserId == userIdResult.Value, ctx);

            if (entity == null)
            {
                entity = new UserPreference(userIdResult.Value, updates);
                dbContext.UserPreferences.Add(entity);
            }
            else
            {
                entity.UpdatePreferences(updates);
            }

            await dbContext.SaveChangesAsync(ctx);
            return Result.Success();
        }
        catch (UnauthorizedAccessException)
        {
            return Result.Unauthorized("User not authenticated.");
        }
        catch (Exception ex)
        {
            return Result.Error($"Failed to update preferences: {ex.Message}");
        }
    }

    public Task<Result<UserPreferenceResponse.Defaults>> TryGetDefaultsAsync(CancellationToken ctx)
    {
        try
        {
            var defaults = new UserPreferenceResponse.Defaults
            {
                DefaultPreferences = new Dictionary<string, object>(UserPreferenceKeys.Defaults)
            };

            return Task.FromResult(Result.Success(defaults));
        }
        catch (Exception ex)
        {
            return Task.FromResult(Result<UserPreferenceResponse.Defaults>.Error($"Failed to get defaults: {ex.Message}"));
        }
    }

    public async Task<Result> UpdateSinglePreferenceAsync(string key, object value, CancellationToken ctx)
    {
        return await TryUpdatePreferencesAsync(new Dictionary<string, object> { [key] = value }, ctx);
    }
}