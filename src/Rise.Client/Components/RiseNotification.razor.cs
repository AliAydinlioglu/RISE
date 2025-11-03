using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Rise.Client.Components;

public partial class RiseNotification : ComponentBase
{
    [Parameter, EditorRequired] public string Title { get; set; } = string.Empty;
    [Parameter] public string? Subtitle { get; set; }
    [Parameter, EditorRequired] public NotificationSeverity Severity { get; set; } = NotificationSeverity.Info;
    [Parameter] public RenderFragment? ChildContent { get; set; }

    private ( Severity Severity, string IconType) _notificationProps;

    protected override void OnParametersSet()
    {
        _notificationProps = Severity switch
        {
            NotificationSeverity.Info => (MudBlazor.Severity.Info, Icons.Material.Rounded.Info),
            NotificationSeverity.Success => (MudBlazor.Severity.Success, Icons.Material.Rounded.CheckCircleOutline),
            NotificationSeverity.Warning => (MudBlazor.Severity.Warning, Icons.Material.Rounded.ReportGmailerrorred),
            NotificationSeverity.Error => (MudBlazor.Severity.Error, Icons.Material.Rounded.WarningAmber),
        };
    }

    public enum NotificationSeverity
    {
        Info,
        Success,
        Warning,
        Error
    }
}