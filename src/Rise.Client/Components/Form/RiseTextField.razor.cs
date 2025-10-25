using Microsoft.AspNetCore.Components;

namespace Rise.Client.Components.Form;

public partial class RiseTextField : IRiseInputComponentProps
{
    [Parameter, EditorRequired] public required string Label { get; set; }
    [Parameter] public string? Value { get; set; }
    [Parameter] public EventCallback? ValueChanged { get; set; }
    [Parameter] public string? HelperText { get; set; }
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public bool Error { get; set; }
    [Parameter] public string? ErrorText { get; set; }
}