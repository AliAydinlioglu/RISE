using Rise.Shared.UserPreferences;
using Rise.Shared.Common;

namespace Rise.Server.Endpoints.UserPreferences
{
    public class GetSingle(IUserPreferenceService userPreferenceService)
        : Endpoint<string, Result<string>>
    {
        public override void Configure()
        {
            Get("/api/user-preferences/{key}");
        }

        public override async Task<Result<string>> ExecuteAsync(string key, CancellationToken ct)
        {
            return await userPreferenceService.GetSinglePreferenceAsync(key, ct);
        }
    }
}