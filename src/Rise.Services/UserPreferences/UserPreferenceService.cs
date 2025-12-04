using Microsoft.EntityFrameworkCore;
using Rise.Domain.UserPreferences;
using Rise.Persistence;
using Rise.Shared.Common;
using Rise.Shared.Identity;
using Rise.Shared.UserPreferences;
using System.Security.Claims;
using System.Text.Json;

namespace Rise.Services.UserPreferences;

    public class UserPreferenceService(
        ApplicationDbContext dbContext,
        IUserService userService)
        : IUserPreferenceService
    {

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
                var userId = await userService.TryGetCurrentUserIdAsync();
                var preferences = await TryLoadPreferencesAsync(userId.Value, ctx);

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
                return Result.Unauthorized("User not authenticated.");
            }
            catch (Exception ex)
            {
                return Result.Error($"Failed to get preferences: {ex.Message}");
            }
        }

        public async Task<Result<string>> TryGetSinglePreferenceAsync(string key, CancellationToken ctx)
        {
            try
            {
                var userId = await userService.TryGetCurrentUserIdAsync();
                if (!UserPreferenceKeys.AllKeys.Contains(key))
                    return Result<string>.Error($"Invalid preference key: {key}");

                var preferences = await TryLoadPreferencesAsync(userId.Value, ctx);

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
                var userId = await userService.TryGetCurrentUserIdAsync();

                foreach (var key in updates.Keys)
                {
                    if (!UserPreferenceKeys.AllKeys.Contains(key))
                        return Result.Error($"Invalid preference key: {key}");
                }

                var entity = await dbContext.UserPreferences
                    .FirstOrDefaultAsync(p => p.UserId == userId.Value, ctx);

                if (entity == null)
                {
                    entity = new UserPreference(userId.Value, updates);
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

        public async Task<Result> UpdateSinglePreferenceAsync(string key, object value, CancellationToken ctx)
        {
            return await TryUpdatePreferencesAsync(new Dictionary<string, object> { [key] = value }, ctx);
        }
    }

