using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Rise.Client.Components;

public partial class Notification : ComponentBase
{
    public enum NotificationSeverity
    {
        Info,
        Success,
        Warning,
        Error
    }
    
    [Parameter, EditorRequired] public string Title { get; set; } = string.Empty;
    [Parameter] public string? Subtitle { get; set; } = string.Empty;
    [Parameter, EditorRequired] public NotificationSeverity Severity { get; set; } = NotificationSeverity.Info;
    [Parameter] public RenderFragment? ChildContent { get; set; }
    
    private string IconType => Severity switch
    {
        NotificationSeverity.Info => Icons.Material.Rounded.Info,
        NotificationSeverity.Success => Icons.Material.Rounded.CheckCircleOutline,
        NotificationSeverity.Warning => Icons.Material.Rounded.ReportGmailerrorred,
        NotificationSeverity.Error => Icons.Material.Rounded.WarningAmber,
        _ => Icons.Material.Filled.Info
    };
    
    private static Severity MapToMudSeverity(NotificationSeverity s) => s switch
    {
        NotificationSeverity.Info => MudBlazor.Severity.Info,
        NotificationSeverity.Success => MudBlazor.Severity.Success,
        NotificationSeverity.Warning => MudBlazor.Severity.Warning,
        NotificationSeverity.Error => MudBlazor.Severity.Error,
        _ => MudBlazor.Severity.Info
    };
}