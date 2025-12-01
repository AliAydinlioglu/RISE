using Rise.Shared.Common;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rise.Shared.UserPreferences
{
    public interface IUserPreferenceService
    {
        Task<Result<UserPreferenceResponse.Preferences>> GetPreferencesAsync(CancellationToken ctx);
        Task<Result<string>> GetSinglePreferenceAsync(string key, CancellationToken ctx);
        Task<Result> UpdatePreferencesAsync(Dictionary<string, object> preferences, CancellationToken ctx);
        Task<Result> UpdateSinglePreferenceAsync(string key, object value, CancellationToken ctx);
    }
}