namespace Rise.Client.Components.Form;

public interface IRiseInputComponentProps
{
    string Label { get; set; }
    string? HelperText { get; set; }
    bool Disabled { get; set; }
    bool Error { get; set; }
    string? ErrorText { get; set; }
}