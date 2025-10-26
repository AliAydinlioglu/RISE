using Microsoft.AspNetCore.Components;

namespace Rise.Client.Components.Form;

public partial class RiseTextField : IRiseInputComponentProps
{
    [Parameter, EditorRequired] public required string Label { get; set; }
    [Parameter] public string? Value { get; set; }
    [Parameter] public EventCallback<string>? ValueChanged { get; set; }
    [Parameter] public string? HelperText { get; set; }
    [Parameter] public bool Disabled { get; set; }
    
    [Parameter] public Func<string?, string?>? Validation { get; set; }
    [Parameter] public bool Error { get; set; }
    [Parameter] public string? ErrorText { get; set; }
    [Parameter] public bool Required { get; set; }
    [Parameter] public string? RequiredError { get; set; }
}