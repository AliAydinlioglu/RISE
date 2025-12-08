using Microsoft.AspNetCore.Components;
using MudBlazor;
using Rise.Client.Components;
using Rise.Client.Layout;
using Rise.Client.Theme;
using Rise.Client.UserPreferences.Models;
using Rise.Client.UserPreferences.Services;
using Rise.Shared.Notifications;
using Rise.Shared.UserPreferences;
using System.Text.Json;
using static Rise.Client.UserPreferences.Models.PreferenceOptions;
using static Rise.Shared.Notifications.SubscribeRequest;
using static Rise.Shared.Notifications.SubscriptionDto;

namespace Rise.Client.UserPreferences;

public partial class Settings : IDisposable
{
    [Inject] private IUserPreferenceStateService PreferenceState { get; set; } = default!;
    [Inject] private IThemingService ThemingService { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private INotificationService NotificationService { get; set; } = default!;

    private IEnumerable<SubscriptionDto.Settings> NotificationSettings { get; set; } = [];
    private bool IsLoadingNotifications { get; set; } = true;
    private bool PreviewCollapsed { get; set; }
    private bool NotificationsExpanded { get; set; }
    private (string Title, string Message, Severity Severity) Status { get; set; } = ("", "", Severity.Info);

    private LanguageOption _currentLanguageOption = Languages[0];
    private RestoOption? _currentRestoOption;

    private string CurrentTheme => PreferenceState.Theme;

    private bool IsDarkTheme
    {
        get => PreferenceState.IsDarkTheme;
        set
        {
            PreferenceState.IsDarkTheme = value;
            ThemingService.IsDarkMode = value;
        }
    }

    private bool ImagesOff
    {
        get => PreferenceState.ImagesOff;
        set
        {
            PreferenceState.ImagesOff = value;
            ThemingService.ImagesOff = value;
        }
    }

    private bool IsNeutral
    {
        get => PreferenceState.IsNeutral;
        set
        {
            PreferenceState.IsNeutral = value;
            ThemingService.IsNeutral = value;
        }
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

    protected override async Task OnInitializedAsync()
    {
        ThemingService.Subscribe += OnThemingServiceChanged;

        PreferenceState.OnStateChanged += StateHasChanged;

        await PreferenceState.InitializeAsync();
        await LoadPreferences();
        await LoadNotificationSettings();

        UpdateThemingServiceFromPreferences();
    }

    /// <summary>
    /// Event handler for ThemingService changes.
    /// Updates preferences when theme changes externally (e.g., from localStorage).
    /// </summary>
    private void OnThemingServiceChanged(object? sender, IThemingService themingService)
    {
        // Update preferences when ThemingService changes externally
        PreferenceState.IsDarkTheme = themingService.IsDarkMode;
        PreferenceState.ImagesOff = themingService.ImagesOff;
        PreferenceState.IsNeutral = themingService.IsNeutral;
        StateHasChanged();
    }

    /// <summary>
    /// Syncs ThemingService state with current user preferences.
    /// Called after loading or saving preferences.
    /// </summary>
    private void UpdateThemingServiceFromPreferences()
    {
        ThemingService.IsDarkMode = PreferenceState.IsDarkTheme;
        ThemingService.ImagesOff = PreferenceState.ImagesOff;
        ThemingService.IsNeutral = PreferenceState.IsNeutral;
    }

    private async Task LoadPreferences()
    {
        var result = await PreferenceState.LoadPreferencesAsync();

        if (result.IsSuccess)
        {
            UpdateDropdownOptions();
            UpdateThemingServiceFromPreferences();
        }
        else
        {
            var errorMessage = result.Errors.Any()
                ? string.Join(", ", result.Errors)
                : "Onbekende fout";

            ShowError("Fout bij laden", $"Kon voorkeuren niet laden: {errorMessage}");
        }
    }

    private async Task LoadNotificationSettings()
    {
        IsLoadingNotifications = true;
        try
        {
            var result = await NotificationService.SubscriptionSettings();
            if (result.IsSuccess && result.Value != null)
            {
                NotificationSettings = result.Value.Settings;
            }
            else
            {
                ShowError("Fout bij laden", "Kon notificatie-instellingen niet laden");
            }
        }
        catch (Exception ex)
        {
            ShowError("Fout bij laden", $"Fout bij laden van notificaties: {ex.Message}");
        }
        finally
        {
            IsLoadingNotifications = false;
            StateHasChanged();
        }
    }

    private async Task OnChannelChanged(string notificationType, NotificationChannelDto channel, bool subscribe)
    {
        channel.IsSubscribed = subscribe;
        StateHasChanged();

        try
        {
            if (subscribe)
            {
                var result = await NotificationService.SubscribeToNotification(new Subscribe
                {
                    Channels = new NotificationChannels
                    {
                        InApp = channel.Name == "InApp",
                        Push = channel.Name == "PushNotification" ? new PushNotification() : null
                    },
                    NotificationType = notificationType,
                });

                if (result.IsSuccess)
                {
                    Snackbar.Add($"Geabonneerd op {GetChannelDisplayName(channel.Name)}", Severity.Success);
                }
                else
                {
                    channel.IsSubscribed = false;
                    var errorMessage = result.Errors.Any() ? string.Join(", ", result.Errors) : "Onbekende fout";
                    ShowError("Fout", $"Kon niet abonneren: {errorMessage}");
                }
            }
            else
            {
                var result = await NotificationService.UnsubscribeFromNotification(new UnsubscribeRequest.Unsubscribe
                {
                    NotificationChannel = channel.Name,
                    NotificationType = notificationType
                });

                if (result.IsSuccess)
                {
                    Snackbar.Add($"Uitgeschreven van {GetChannelDisplayName(channel.Name)}", Severity.Info);
                }
                else
                {
                    channel.IsSubscribed = true;
                    var errorMessage = result.Errors.Any() ? string.Join(", ", result.Errors) : "Onbekende fout";
                    ShowError("Fout", $"Kon niet uitschrijven: {errorMessage}");
                }
            }
        }
        catch (Exception ex)
        {
            channel.IsSubscribed = !subscribe;
            ShowError("Fout", $"Kon notificatie-instelling niet opslaan: {ex.Message}");
        }
        finally
        {
            StateHasChanged();
        }
    }

    private void UpdateDropdownOptions()
    {
        _currentLanguageOption = AvailableLanguages.FirstOrDefault(l =>
            l.Code == PreferenceState.Language) ?? AvailableLanguages[0];

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
            UpdateThemingServiceFromPreferences();
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
        UpdateThemingServiceFromPreferences();
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
        ThemingService.Subscribe -= OnThemingServiceChanged;
        PreferenceState.OnStateChanged -= StateHasChanged;
    }
}