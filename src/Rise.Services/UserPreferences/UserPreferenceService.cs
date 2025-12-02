using Microsoft.EntityFrameworkCore;
using Rise.Domain.UserPreferences;
using Rise.Persistence;
using Rise.Services.Identity;
using Rise.Shared.Common;
using Rise.Shared.Identity;
using Rise.Shared.UserPreferences;
using System.Security.Claims;

namespace Rise.Services.UserPreferences
{
    public class UserPreferenceService(
        ApplicationDbContext dbContext,
        ISessionContextProvider sessionContextProvider,
        IUserService userService)
        : IUserPreferenceService
    {
        private async Task<Guid?> GetCurrentUserIdAsync()
        {
            var user = sessionContextProvider.User;
            if (user == null)
                throw new UnauthorizedAccessException("User not authenticated: session user is null.");

            var ssoClaim = user.FindFirstValue("http://schemas.microsoft.com/identity/claims/objectidentifier");

            var result = await userService.GetOrCreateUserAsync(ssoClaim);

            if (!result.IsSuccess)
                throw new UnauthorizedAccessException("Could not resolve user from SSO claim.");

            var userResult = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == result.Value.Email);
            if (userResult == null)
                throw new KeyNotFoundException("User not found in database.");

            return userResult.Id;
        }

        private async Task<PreferenceBag> LoadPreferences(Guid userId, CancellationToken ctx)
        {
            try
            {
                var entity = await dbContext.UserPreferences
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.UserId == userId, ctx);

                return new PreferenceBag(entity?.PreferencesJson);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to load user preferences.", ex);
            }
        }

        public async Task<Result<UserPreferenceResponse.Preferences>> GetPreferencesAsync(CancellationToken ctx)
        {
            try
            {
                var userId = await GetCurrentUserIdAsync();
                var bag = await LoadPreferences(userId.Value, ctx);

                return Result.Success(new UserPreferenceResponse.Preferences
                {
                    UserPreferences = new UserPreferenceDto.Preferences
                    {
                        Settings = bag.Values,
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

        public async Task<Result<string>> GetSinglePreferenceAsync(string key, CancellationToken ctx)
        {
            try
            {
                var userId = await GetCurrentUserIdAsync();

                if (!UserPreferenceKeys.AllKeys.Contains(key))
                    return Result<string>.Error($"Invalid preference key: {key}");

                var bag = await LoadPreferences(userId.Value, ctx);
                var value = bag.GetSingle(key);
                return Result<string>.Success(value ?? UserPreferenceKeys.Defaults[key].ToString()!);
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

        public async Task<Result> UpdatePreferencesAsync(
            Dictionary<string, object> updates,
            CancellationToken ctx)
        {
            try
            {
                var userId = await GetCurrentUserIdAsync();

                foreach (var key in updates.Keys)
                {
                    if (!UserPreferenceKeys.AllKeys.Contains(key))
                        return Result.Error($"Invalid preference key: {key}");
                }

                var entity = await dbContext.UserPreferences
                    .FirstOrDefaultAsync(p => p.UserId == userId.Value, ctx);

                PreferenceBag bag;
                if (entity == null)
                {
                    bag = new PreferenceBag(null);
                    bag.Merge(updates);

                    entity = new UserPreference(userId.Value, bag.ToJson());
                    dbContext.UserPreferences.Add(entity);
                }
                else
                {
                    bag = new PreferenceBag(entity.PreferencesJson);
                    bag.Merge(updates);

                    entity.UpdatePreferences(bag.ToJson());
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

        public Task<Result> UpdateSinglePreferenceAsync(string key, object value, CancellationToken ctx)
        {
            return UpdatePreferencesAsync(new Dictionary<string, object> { [key] = value }, ctx);
        }
    }
}
