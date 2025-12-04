using Rise.Shared.UserPreferences;

namespace Rise.Server.Endpoints.UserPreferences;

    public class Get(IUserPreferenceService userPreferenceService)
        : EndpointWithoutRequest<Result<UserPreferenceResponse.Preferences>>
    {
        public override void Configure()
        {
            Get("/api/user-preferences");
        }

        public override Task<Result<UserPreferenceResponse.Preferences>> ExecuteAsync(CancellationToken ct)
        {
            return userPreferenceService.TryGetPreferencesAsync(ct);
        }
    }
