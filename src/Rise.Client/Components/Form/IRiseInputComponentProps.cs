using Microsoft.AspNetCore.Components;

namespace Rise.Client.Components.Form;

public interface IRiseInputComponentProps
{
    string Label { get; set; }
    string? Value { get; set; }
    EventCallback? ValueChanged { get; set; }
    string? HelperText { get; set; }
    bool Disabled { get; set; }
    bool Error { get; set; }
    string? ErrorText { get; set; }

}