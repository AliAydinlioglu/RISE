using Rise.Client.UserPreferences.Models;

namespace Rise.Client.UserPreferences.Services;

public interface IUserPreferenceStateService
{
    Dictionary<string, object> CurrentPreferences { get; }
    bool HasUnsavedChanges { get; }
    bool IsSaving { get; }
    bool IsLoadingRestos { get; }

    T GetPreference<T>(string key);
    void SetPreference<T>(string key, T value);

    string Theme { get; set; }
    bool IsDarkTheme { get; set; }
    bool ImagesOff { get; set; }
    bool IsNeutral { get; set; }
    bool IsRemote { get; set; }
    string Language { get; set; }
    string FavoriteResto { get; set; }

    bool NotifyDeadline { get; set; }
    bool NotifySchoolEvent { get; set; }
    bool NotifyEmergencies { get; set; }
    bool NotifyCancelledClass { get; set; }

    List<PreferenceOptions.RestoOption> AvailableRestos { get; } 


    Task<Result> LoadPreferencesAsync(CancellationToken cancellationToken = default);
    Task<Result> SavePreferencesAsync(CancellationToken cancellationToken = default);
    Task InitializeAsync(CancellationToken cancellationToken = default);
    Task<Result> LoadRestosAsync(CancellationToken cancellationToken = default);
    void CancelChanges();
    void RestoreDefaults();

    event Action? OnStateChanged;
}