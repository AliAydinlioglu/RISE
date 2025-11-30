using Microsoft.AspNetCore.Components;
using MudBlazor;
using Rise.Client.Components;
using Rise.Client.UserPreferences.Models;
using Rise.Client.UserPreferences.Services;
using static Rise.Client.UserPreferences.Models.PreferenceOptions;

namespace Rise.Client.UserPreferences;

public partial class Settings : IDisposable
{
    [Inject] private IUserPreferenceStateService PreferenceState { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;

    private bool PreviewCollapsed { get; set; }
    private bool NotificationsExpanded { get; set; }
    private (string Title, string Message, Severity Severity) Status { get; set; } = ("", "", Severity.Info);

    private LanguageOption _currentLanguageOption = Languages[0];
    private RestoOption? _currentRestoOption;

    private string CurrentTheme => PreferenceState.Theme; 

    private bool IsDarkTheme
    {
        get => PreferenceState.IsDarkTheme;
        set => PreferenceState.IsDarkTheme = value;
    }

    private bool ImagesOff
    {
        get => PreferenceState.ImagesOff;
        set => PreferenceState.ImagesOff = value;
    }

    private bool IsNeutral
    {
        get => PreferenceState.IsNeutral;
        set => PreferenceState.IsNeutral = value;
    }

    private bool IsRemote
    {
        get => PreferenceState.IsRemote;
        set => PreferenceState.IsRemote = value;
    }

    private bool NotifyDeadline
    {
        get => PreferenceState.NotifyDeadline;
        set => PreferenceState.NotifyDeadline = value;
    }

    private bool NotifySchoolEvent
    {
        get => PreferenceState.NotifySchoolEvent;
        set => PreferenceState.NotifySchoolEvent = value;
    }

    private bool NotifyEmergencies
    {
        get => PreferenceState.NotifyEmergencies;
        set => PreferenceState.NotifyEmergencies = value;
    }

    private bool NotifyCancelledClass
    {
        get => PreferenceState.NotifyCancelledClass;
        set => PreferenceState.NotifyCancelledClass = value;
    }

    private LanguageOption CurrentLanguageOption
    {
        get => _currentLanguageOption;
        set
        {
            _currentLanguageOption = value;
            PreferenceState.Language = value.Code;
        }
    }

    private RestoOption? CurrentRestoOption
    {
        get => _currentRestoOption;
        set
        {
            _currentRestoOption = value;
            if (value != null)
            {
                PreferenceState.FavoriteResto = value.Id;
            }
        }
    }

    private bool HasUnsavedChanges => PreferenceState.HasUnsavedChanges;
    private bool IsSaving => PreferenceState.IsSaving;

    private List<LanguageOption> AvailableLanguages => Languages;
    private List<RestoOption> AvailableRestos => PreferenceState.AvailableRestos;
    private bool IsLoadingRestos => PreferenceState.IsLoadingRestos;

    private string StatusTitle => Status.Title;
    private string StatusMessage => Status.Message;
    private Severity StatusSeverity => Status.Severity;

    private DateTime PreviewDate => new(2025, 12, 15, 14, 0, 0);
    private string PreviewTitle => "Open Campus Dag";
    private string PreviewCategory => "Campus Event";
    private TimeOnly PreviewStartTime => new(14, 0);
    private TimeOnly PreviewEndTime => new(17, 30);
    private string PreviewLocation => "Schoonmeersen";
    private bool PreviewExpanded { get; set; } = true;

    private void TogglePreview() => PreviewExpanded = !PreviewExpanded;

    protected override async Task OnInitializedAsync()
    {
        PreferenceState.OnStateChanged += StateHasChanged;
        await PreferenceState.InitializeAsync();
        await LoadPreferences();
    }

    private async Task LoadPreferences()
    {
        var result = await PreferenceState.LoadPreferencesAsync();

        if (result.IsSuccess)
        {
            UpdateDropdownOptions();
        }
        else
        {
            var errorMessage = result.Errors.Any()
                ? string.Join(", ", result.Errors)
                : "Onbekende fout";

            ShowError("Fout bij laden", $"Kon voorkeuren niet laden: {errorMessage}");
        }
    }

    private void UpdateDropdownOptions()
    {
        _currentLanguageOption = Languages.FirstOrDefault(l =>
            l.Code == PreferenceState.Language) ?? Languages[0];

        _currentRestoOption = AvailableRestos.FirstOrDefault(r =>
            r.Id == PreferenceState.FavoriteResto);
    }

    private async Task HandleSave()
    {
        ClearStatus();

        var result = await PreferenceState.SavePreferencesAsync();

        if (result.IsSuccess)
        {
            ShowSuccess("Opgeslagen", "Je voorkeuren zijn succesvol bijgewerkt!");
            Snackbar.Add("Instellingen opgeslagen", Severity.Success);
        }
        else
        {
            var errorMessage = result.Errors.Any()
                ? string.Join(", ", result.Errors)
                : "Onbekende fout";

            ShowError("Fout bij opslaan", errorMessage);
        }
    }

    private void HandleCancel()
    {
        PreferenceState.CancelChanges();
        UpdateDropdownOptions();
        ClearStatus();
        Snackbar.Add("Wijzigingen geannuleerd", Severity.Info);
    }

    private async Task HandleRestoreDefaults()
    {
        PreferenceState.RestoreDefaults();
        UpdateDropdownOptions();
        await HandleSave();
    }

    private void TogglePreview() => PreviewCollapsed = !PreviewCollapsed;
    private void ToggleNotifications() => NotificationsExpanded = !NotificationsExpanded;

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

    public void Dispose()
    {
        PreferenceState.OnStateChanged -= StateHasChanged;
    }
}