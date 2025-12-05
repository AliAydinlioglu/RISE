using Microsoft.AspNetCore.Components;
using MudBlazor;
using Rise.Client.Components;
using Rise.Client.Layout;
using Rise.Client.UserPreferences.Models;
using Rise.Shared.UserPreferences;
using System.Text.Json;
using static Rise.Client.UserPreferences.Models.PreferenceOptions;

namespace Rise.Client.UserPreferences;

public partial class Settings
{
    [Inject] private IUserPreferenceService UserPreferenceService { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;
    [Inject] private NavigationManager Navigation{ get; set; } = default!;


    private Dictionary<string, object> CurrentPreferences { get; set; } = new();
    private Dictionary<string, object> OriginalPreferences { get; set; } = new();
    private bool PreviewCollapsed { get; set; } = false;

    private string CurrentTheme
    {
        get => GetPreference<string>(UserPreferenceKeys.Theme);
        set => SetPreference(UserPreferenceKeys.Theme, value);
    }

    private bool IsDarkTheme
    {
        get => CurrentTheme == "dark";
        set => CurrentTheme = value ? "dark" : "light";
    }

    private bool ImagesOff { get => GetBoolPref(UserPreferenceKeys.ImagesOff); set => SetBoolPref(UserPreferenceKeys.ImagesOff, value); }
    private bool IsNeutral { get => GetBoolPref(UserPreferenceKeys.IsNeutral); set => SetBoolPref(UserPreferenceKeys.IsNeutral, value); }
    private bool IsRemote { get => GetBoolPref(UserPreferenceKeys.IsRemote); set => SetBoolPref(UserPreferenceKeys.IsRemote, value); }
    private bool NotifyDeadline { get => GetBoolPref(UserPreferenceKeys.NotifyDeadline); set => SetBoolPref(UserPreferenceKeys.NotifyDeadline, value); }
    private bool NotifySchoolEvent { get => GetBoolPref(UserPreferenceKeys.NotifySchoolEvent); set => SetBoolPref(UserPreferenceKeys.NotifySchoolEvent, value); }
    private bool NotifyEmergencies { get => GetBoolPref(UserPreferenceKeys.NotifyEmergencies); set => SetBoolPref(UserPreferenceKeys.NotifyEmergencies, value); }
    private bool NotifyCancelledClass { get => GetBoolPref(UserPreferenceKeys.NotifyCancelledClass); set => SetBoolPref(UserPreferenceKeys.NotifyCancelledClass, value); }
    private void TogglePreview() => PreviewCollapsed = !PreviewCollapsed; 
    private bool GetBoolPref(string key) => GetPreference<bool>(key);
    private void SetBoolPref(string key, bool value) => SetPreference(key, value);

    private LanguageOption _currentLanguageOption = Languages[0];
    private LanguageOption CurrentLanguageOption
    {
        get => _currentLanguageOption;
        set
        {
            _currentLanguageOption = value;
            SetPreference(UserPreferenceKeys.Language, value.Code);
        }
    }

    private CampusOption _currentCampusOption = PreferenceOptions.Campuses[0];
    private CampusOption CurrentCampusOption
    {
        get => _currentCampusOption;
        set
        {
            _currentCampusOption = value;
            SetPreference(UserPreferenceKeys.FavoriteResto, value.Id);
        }
    }

    private T GetPreference<T>(string key)
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

    private T GetDefaultValue<T>(string key) =>
        UserPreferenceDefaults.Values.TryGetValue(key, out var defaultValue)
            ? TryConvert(defaultValue, default(T)!)
            : default!;

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
                JsonValueKind.String when typeof(T) == typeof(string) => (T)(object)element.GetString()!,
                JsonValueKind.Number when typeof(T) == typeof(int) => (T)(object)element.GetInt32(),
                JsonValueKind.Number when typeof(T) == typeof(double) => (T)(object)element.GetDouble(),
                JsonValueKind.True or JsonValueKind.False when typeof(T) == typeof(bool) => (T)(object)element.GetBoolean(),
                _ => JsonSerializer.Deserialize<T>(element.GetRawText())!
            };
        }
        catch { return default!; }
    }

    private void SetPreference<T>(string key, T value)
    {
        CurrentPreferences[key] = value!;
        StateHasChanged();
    }

    private bool HasUnsavedChanges =>
        CurrentPreferences.Count != OriginalPreferences.Count ||
        CurrentPreferences.Any(kvp => !OriginalPreferences.TryGetValue(kvp.Key, out var original) ||
            GetValueString(kvp.Value) != GetValueString(original));

    private string GetValueString(object value) =>
        value is JsonElement je ? je.GetRawText() : value?.ToString() ?? string.Empty;

    private bool IsSaving { get; set; }
    private bool NotificationsExpanded { get; set; }
    private (string Title, string Message, Severity Severity) Status { get; set; } = ("", "", Severity.Info);

    private List<LanguageOption> AvailableLanguages => Languages;
    private List<CampusOption> AvailableCampuses => PreferenceOptions.Campuses;

    //Preview
    private DateTime PreviewDate => new(2025, 12, 15, 14, 0, 0);
    private string PreviewTitle => "Open Campus Dag";
    private string PreviewCategory => "Campus Event";
    private TimeOnly PreviewStartTime => new(14, 0);
    private TimeOnly PreviewEndTime => new(17, 30);
    private string PreviewLocation => "Schoonmeersen";

    private string StatusTitle => Status.Title;
    private string StatusMessage => Status.Message;
    private Severity StatusSeverity => Status.Severity;

    protected override async Task OnInitializedAsync() => await LoadPreferences();

    private void ToggleNotifications() => NotificationsExpanded = !NotificationsExpanded;

    private async Task LoadPreferences()
    {
        try
        {
            var result = await UserPreferenceService.TryGetPreferencesAsync(CancellationToken.None);

            if (result.IsSuccess && result.Value != null)
            {
                var prefs = result.Value.UserPreferences.Settings;
                CurrentPreferences = new(prefs);
                OriginalPreferences = new(prefs);

                UpdateDropdownOptions();
            }
        }
        catch (Exception ex)
        {
            ShowError("Fout bij laden", $"Kon voorkeuren niet laden: {ex.Message}");
        }
    }

    private void UpdateDropdownOptions()
    {
        _currentLanguageOption = Languages.FirstOrDefault(l =>
            l.Code == GetPreference<string>(UserPreferenceKeys.Language)) ?? Languages[0];

        _currentCampusOption = PreferenceOptions.Campuses.FirstOrDefault(c =>
            c.Id == GetPreference<string>(UserPreferenceKeys.FavoriteResto)) ?? PreferenceOptions.Campuses[0];
    }

    private async Task HandleSave()
    {
        if (!HasUnsavedChanges) return;

        IsSaving = true;
        ClearStatus();

        try
        {
            var result = await UserPreferenceService.TryUpdatePreferencesAsync(
                CurrentPreferences, CancellationToken.None);

            if (result.IsSuccess)
            {
                OriginalPreferences = new(CurrentPreferences);
                ShowSuccess("Opgeslagen", "Je voorkeuren zijn succesvol bijgewerkt!");
                Snackbar.Add("Instellingen opgeslagen", Severity.Success);
            }
            else
            {
                var errors = string.Join(", ", result.Errors ?? new List<string> { "Onbekende fout" });
                ShowError("Fout bij opslaan", errors);
            }
        }
        catch (Exception ex)
        {
            ShowError("Fout bij opslaan", $"Kon voorkeuren niet opslaan: {ex.Message}");
        }
        finally
        {
            IsSaving = false;
        }
    }

    private void HandleCancel()
    {
        CurrentPreferences = new(OriginalPreferences);
        UpdateDropdownOptions();
        ClearStatus();
        Snackbar.Add("Wijzigingen geannuleerd", Severity.Info);
        StateHasChanged();
    }

    private async Task HandleRestoreDefaults()
    {
        CurrentPreferences = new(UserPreferenceDefaults.Values);
        _currentLanguageOption = Languages[0];
        _currentCampusOption = PreferenceOptions.Campuses[0];
        await HandleSave();
    }

    private void ShowSuccess(string title, string message) =>
        Status = (title, message, Severity.Success);

    private void ShowError(string title, string message) =>
        Status = (title, message, Severity.Error);

    private void ClearStatus() => Status = ("", "", Severity.Info);

    private RiseNotification.NotificationSeverity ConvertToRiseNotificationSeverity(Severity severity) =>
        severity switch
        {
            Severity.Success => RiseNotification.NotificationSeverity.Success,
            Severity.Info => RiseNotification.NotificationSeverity.Info,
            Severity.Warning => RiseNotification.NotificationSeverity.Warning,
            Severity.Error => RiseNotification.NotificationSeverity.Error,
            _ => RiseNotification.NotificationSeverity.Info
        };

    private string GetCategoryStyle() =>
        $"color: {(IsNeutral ? (CurrentTheme == "dark" ? "#ffffff" : "#000000") : "#16B0A5")}; font-weight: 500; margin-top: 8px; font-size: 14px;";
}