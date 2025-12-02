using Rise.Shared.UserPreferences;

namespace Rise.Server.Endpoints.UserPreferences
{
    public class UpdateSingle(IUserPreferenceService userPreferenceService)
        : Endpoint<UserPreferenceRequest.UpdateSingle, Result>
    {
        public override void Configure()
        {
            Patch("/api/user-preferences/{Key}");
        }

        public override async Task<Result> ExecuteAsync(UserPreferenceRequest.UpdateSingle req, CancellationToken ct)
        {
            return await userPreferenceService.UpdateSinglePreferenceAsync(req.Key, req.Value, ct);
        }
    }
}