// Rise.Client.UserPreferences/Services/UserPreferenceStateService.cs
using Ardalis.Result;
using Rise.Client.UserPreferences.Models;
using Rise.Shared.Menu;
using Rise.Shared.UserPreferences;
using System.Text.Json;

namespace Rise.Client.UserPreferences.Services;

public class UserPreferenceStateService : IUserPreferenceStateService
{
    private readonly IUserPreferenceService _userPreferenceService;
    private readonly IRestoService _restoService;

    public Dictionary<string, object> CurrentPreferences { get; private set; } = new();
    private Dictionary<string, object> OriginalPreferences { get; set; } = new();
    private Dictionary<string, object>? _defaultPreferences { get; set; } = new();

    public bool HasUnsavedChanges => !PreferencesEqual(CurrentPreferences, OriginalPreferences);
    public bool IsSaving { get; private set; }
    public bool IsLoadingRestos { get; private set; }

    public List<PreferenceOptions.LanguageOption> AvailableLanguages => PreferenceOptions.Languages;
    public List<PreferenceOptions.RestoOption> AvailableRestos { get; private set; } = new();


    public event Action? OnStateChanged;

    public UserPreferenceStateService(
            IUserPreferenceService userPreferenceService,
            IRestoService restoService) 
    {
        _userPreferenceService = userPreferenceService;
        _restoService = restoService;
    }

    public string Theme
    {
        get => GetPreference<string>(UserPreferenceKeys.Theme);
        set => SetPreference(UserPreferenceKeys.Theme, value);
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

    public string FavoriteResto
    {
        get => GetPreference<string>(UserPreferenceKeys.FavoriteResto);
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

    public async Task<Result> LoadPreferencesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _userPreferenceService.TryGetPreferencesAsync(cancellationToken);

            if (result.IsSuccess && result.Value != null)
            {
                var prefs = result.Value.UserPreferences.Settings;
                CurrentPreferences = new(prefs);
                OriginalPreferences = new(prefs);
                NotifyStateChanged();
                return Result.Success();
            }

            var errors = result.Errors ?? new List<string> { "Kon voorkeuren niet laden" };
            return Result.Error(string.Join("; ", errors));
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }

    public async Task<Result> SavePreferencesAsync(CancellationToken cancellationToken = default)
    {
        if (!HasUnsavedChanges)
            return Result.Success();

        IsSaving = true;
        NotifyStateChanged();

        try
        {
            var result = await _userPreferenceService.TryUpdatePreferencesAsync(
                CurrentPreferences, cancellationToken);

            if (result.IsSuccess)
            {
                OriginalPreferences = new(CurrentPreferences);
            }

            return result;
        }
        finally
        {
            IsSaving = false;
            NotifyStateChanged();
        }
    }

    public void CancelChanges()
    {
        CurrentPreferences = new(OriginalPreferences);
        NotifyStateChanged();
    }
    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        var defaultsResult = await _userPreferenceService.TryGetDefaultsAsync(cancellationToken);

        if (defaultsResult.IsSuccess && defaultsResult.Value?.DefaultPreferences != null)
        {
            _defaultPreferences = new Dictionary<string, object>(defaultsResult.Value.DefaultPreferences);
        }
        else
        {
            _defaultPreferences = new Dictionary<string, object>();
        }

        await LoadRestosAsync(cancellationToken);

        NotifyStateChanged();
    }

    public async Task<Result> LoadRestosAsync(CancellationToken cancellationToken = default)
    {
        IsLoadingRestos = true;
        NotifyStateChanged();

        try
        {
            var result = await _restoService.GetOverviewAsync(
                new Rise.Shared.Common.QueryRequest.SkipTake { Skip = 0, Take = 100 },
                cancellationToken);

            if (result.IsSuccess && result.Value?.Restos != null)
            {
                AvailableRestos = result.Value.Restos
                    .Select(r => new PreferenceOptions.RestoOption
                    {
                        Id = r.Name, 
                        Name = r.Name,
                        IsFavorite = r.IsFavorite
                    })
                    .ToList();

                NotifyStateChanged();
                return Result.Success();
            }

            var errorMessage = result.Errors != null && result.Errors.Any()
                ? string.Join(", ", result.Errors)
                : "Kon resto's niet laden";

            return Result.Error(errorMessage);
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
        finally
        {
            IsLoadingRestos = false;
            NotifyStateChanged();
        }
    }
    public void RestoreDefaults()
    {
        CurrentPreferences = new Dictionary<string, object>(_defaultPreferences);

        NotifyStateChanged();
    }


    public T GetPreference<T>(string key)
    {
        if (!CurrentPreferences.TryGetValue(key, out var value))
            return GetDefaultValue<T>(key);

        return value switch
        {
            JsonElement je => DeserializeJsonElement<T>(je),
            T typedValue => typedValue,
            _ => TryConvert(value, GetDefaultValue<T>(key))
        };
    }

    public void SetPreference<T>(string key, T value)
    {
        CurrentPreferences[key] = value!;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnStateChanged?.Invoke();

    private bool PreferencesEqual(Dictionary<string, object> a, Dictionary<string, object> b)
    {
        if (a.Count != b.Count) return false;

        return !a.Any(kvp => !b.TryGetValue(kvp.Key, out var original) ||
            GetValueString(kvp.Value) != GetValueString(original));
    }

    private string GetValueString(object value) =>
        value is JsonElement je ? je.GetRawText() : value?.ToString() ?? string.Empty;

    private T GetDefaultValue<T>(string key)
    {
        if (_defaultPreferences.TryGetValue(key, out var serverDefault))
            return TryConvert(serverDefault, default(T)!);

        return default!;
    }

    private T TryConvert<T>(object value, T fallback)
    {
        try { return (T)Convert.ChangeType(value, typeof(T)); }
        catch { return fallback; }
    }

    private T DeserializeJsonElement<T>(JsonElement element)
    {
        try
        {
            return element.ValueKind switch
            {
                JsonValueKind.String when typeof(T) == typeof(string) =>
                    (T)(object)element.GetString()!,
                JsonValueKind.Number when typeof(T) == typeof(int) =>
                    (T)(object)element.GetInt32(),
                JsonValueKind.Number when typeof(T) == typeof(double) =>
                    (T)(object)element.GetDouble(),
                JsonValueKind.True or JsonValueKind.False when typeof(T) == typeof(bool) =>
                    (T)(object)element.GetBoolean(),
                _ => JsonSerializer.Deserialize<T>(element.GetRawText())!
            };
        }
        catch { return default!; }
    }
}