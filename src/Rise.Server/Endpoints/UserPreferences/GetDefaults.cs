using Rise.Shared.UserPreferences;

namespace Rise.Server.Endpoints.UserPreferences;

public class GetDefaults(IUserPreferenceService userPreferenceService)
    : EndpointWithoutRequest<Result<UserPreferenceResponse.Defaults>>
{
    public override void Configure()
    {
        Get("/api/user-preferences/defaults");
    }

    public override Task<Result<UserPreferenceResponse.Defaults>> ExecuteAsync(CancellationToken ct)
    {
        return userPreferenceService.TryGetDefaultsAsync(ct);
    }
}