using Ardalis.Result;
using Rise.Client.UserPreferences.Models;
using Rise.Client.UserPreferences.Services;
using Rise.Shared.UserPreferences;

namespace Rise.Client.Faker;

public class FakeUserPreferenceStateService : IUserPreferenceStateService
{
    public Dictionary<string, object> CurrentPreferences { get; private set; } = new();

    public bool HasUnsavedChanges { get; set; }
    public bool IsSaving { get; set; }
    public bool IsLoadingRestos { get; set; }

    public List<PreferenceOptions.LanguageOption> AvailableLanguages => PreferenceOptions.Languages;
    public List<PreferenceOptions.RestoOption> AvailableRestos { get; set; } = new()
    {
        new() { Id = 1, Name = "Test Resto", IsFavorite = true },
        new() { Id = 2, Name = "New Test Resto", IsFavorite = false }
    };

    public event Action? OnStateChanged;

    public string Theme
    {
        get => GetPreference<string>(UserPreferenceKeys.DarkMode);
        set => SetPreference(UserPreferenceKeys.DarkMode, value);
    }

    public bool IsDarkTheme
    {
        get => Theme == "dark";
        set => Theme = value ? "dark" : "light";
    }

    public bool ImagesOff
    {
        get => GetPreference<bool>(UserPreferenceKeys.ImagesOff);
        set => SetPreference(UserPreferenceKeys.ImagesOff, value);
    }

    public bool IsNeutral
    {
        get => GetPreference<bool>(UserPreferenceKeys.IsNeutral);
        set => SetPreference(UserPreferenceKeys.IsNeutral, value);
    }

    public bool IsRemote
    {
        get => GetPreference<bool>(UserPreferenceKeys.IsRemote);
        set => SetPreference(UserPreferenceKeys.IsRemote, value);
    }

    public string Language
    {
        get => GetPreference<string>(UserPreferenceKeys.Language);
        set => SetPreference(UserPreferenceKeys.Language, value);
    }

    public int FavoriteResto
    {
        get => GetPreference<int>(UserPreferenceKeys.FavoriteResto);
        set => SetPreference(UserPreferenceKeys.FavoriteResto, value);
    }

    public bool NotifyDeadline
    {
        get => GetPreference<bool>(UserPreferenceKeys.NotifyDeadline);
        set => SetPreference(UserPreferenceKeys.NotifyDeadline, value);
    }

    public bool NotifySchoolEvent
    {
        get => GetPreference<bool>(UserPreferenceKeys.NotifySchoolEvent);
        set => SetPreference(UserPreferenceKeys.NotifySchoolEvent, value);
    }

    public bool NotifyEmergencies
    {
        get => GetPreference<bool>(UserPreferenceKeys.NotifyEmergencies);
        set => SetPreference(UserPreferenceKeys.NotifyEmergencies, value);
    }

    public bool NotifyCancelledClass
    {
        get => GetPreference<bool>(UserPreferenceKeys.NotifyCancelledClass);
        set => SetPreference(UserPreferenceKeys.NotifyCancelledClass, value);
    }

    public FakeUserPreferenceStateService()
    {
        // Initialize with defaults
        CurrentPreferences = new Dictionary<string, object>(UserPreferenceKeys.Defaults);
    }

    public Task<Result> InitializeAsync(CancellationToken cancellationToken = default)
    {
        IsLoadingRestos = false;
        OnStateChanged?.Invoke();
        return Task.FromResult(Result.Success());
    }

    public Task<Result> LoadPreferencesAsync(CancellationToken cancellationToken = default)
    {
        OnStateChanged?.Invoke();
        return Task.FromResult(Result.Success());
    }

    public Task<Result> SavePreferencesAsync(CancellationToken cancellationToken = default)
    {
        HasUnsavedChanges = false;
        OnStateChanged?.Invoke();
        return Task.FromResult(Result.Success());
    }

    public void CancelChanges()
    {
        HasUnsavedChanges = false;
        OnStateChanged?.Invoke();
    }

    public void RestoreDefaults()
    {
        CurrentPreferences = new Dictionary<string, object>(UserPreferenceKeys.Defaults);
        HasUnsavedChanges = true;
        OnStateChanged?.Invoke();
    }

    public Task<Result> LoadRestosAsync(CancellationToken cancellationToken = default)
    {
        IsLoadingRestos = false;
        OnStateChanged?.Invoke();
        return Task.FromResult(Result.Success());
    }

    public T GetPreference<T>(string key)
    {
        if (CurrentPreferences.TryGetValue(key, out var value))
        {
            if (value is T typedValue)
                return typedValue;

            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return default!;
            }
        }

        return default!;
    }

    public void SetPreference<T>(string key, T value)
    {
        CurrentPreferences[key] = value!;
        HasUnsavedChanges = true;
        OnStateChanged?.Invoke();
    }

    Task IUserPreferenceStateService.InitializeAsync(CancellationToken cancellationToken)
    {
        return InitializeAsync(cancellationToken);
    }
}