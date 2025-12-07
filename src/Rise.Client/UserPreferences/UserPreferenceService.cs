using System.Net.Http.Json;
using Rise.Shared.Common;
using Rise.Shared.UserPreferences;

namespace Rise.Client.UserPreferences
{
    public class UserPreferenceService(HttpClient httpClient) : IUserPreferenceService
    {
        public async Task<Result<UserPreferenceResponse.Preferences>> TryGetPreferencesAsync(CancellationToken ctx)
        {
            var result = await httpClient.GetFromJsonAsync<Result<UserPreferenceResponse.Preferences>>(
                "/api/user-preferences",
                cancellationToken: ctx);
            return result!;
        }
        public async Task<Result<UserPreferenceResponse.Defaults>> TryGetDefaultsAsync(CancellationToken ctx)
        {
            var result = await httpClient.GetFromJsonAsync<Result<UserPreferenceResponse.Defaults>>(
                "/api/user-preferences/defaults",
                cancellationToken: ctx);
            return result!;
        }

        public async Task<Result<string>> TryGetSinglePreferenceAsync(string key, CancellationToken ctx)
        {
            var result = await httpClient.GetFromJsonAsync<Result<string>>(
                $"/api/user-preferences/{key}",
                cancellationToken: ctx);
            return result!;
        }

        public async Task<Result> TryUpdatePreferencesAsync(Dictionary<string, object> preferences, CancellationToken ctx)
        {
            var response = await httpClient.PutAsJsonAsync(
                "/api/user-preferences",
                preferences,
                cancellationToken: ctx);
            var result = await response.Content.ReadFromJsonAsync<Result>(cancellationToken: ctx);
            return result!;
        }

        public async Task<Result> UpdateSinglePreferenceAsync(string key, object value, CancellationToken ctx)
        {
            var request = new UserPreferenceRequest.UpdateSingle
            {
                Key = key,
                Value = value
            };

            var response = await httpClient.PatchAsJsonAsync(
                $"/api/user-preferences/{key}",
                request,
                cancellationToken: ctx);
            var result = await response.Content.ReadFromJsonAsync<Result>(cancellationToken: ctx);
            return result!;
        }
    }
}