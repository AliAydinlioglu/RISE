using Rise.Shared.UserPreferences;
using Rise.Shared.Common;
using Rise.Services.UserPreferences;

namespace Rise.Server.Endpoints.UserPreferences;

    public class Update(IUserPreferenceService userPreferenceService)
        : Endpoint<Dictionary<string, object>, Result>
    {
        public override void Configure()
        {
            Put("/api/user-preferences");
        }

        public override Task<Result> ExecuteAsync(Dictionary<string, object> req, CancellationToken ct)
        {
            return userPreferenceService.TryUpdatePreferencesAsync(req, ct);
        }
    }
