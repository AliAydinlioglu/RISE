namespace Rise.Client.Components.Form;

public interface IRiseInputComponentProps
{
    string Label { get; }
    string? HelperText { get; }
    bool Disabled { get; }
    bool Error { get; }
    string? ErrorText { get; }
    bool Required { get; }
    string? RequiredError { get; }
}