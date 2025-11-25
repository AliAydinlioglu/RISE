using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Rise.Client.Components;

public partial class RiseButton
{
    [Parameter, EditorRequired] public EventCallback OnClick { get; set; }
    [Parameter, EditorRequired] public RenderFragment? ChildContent { get; set; }
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public ButtonType ButtonType { get; set; } = ButtonType.Button;
    [Parameter] public RiseButtonType Type { get; set; } = RiseButtonType.Primary;
    [Parameter] public RiseButtonSize Size { get; set; } = RiseButtonSize.Medium;
    [Parameter] public string Class { get; set; } = string.Empty;
    private Variant GetVariant() => Type switch
    {
        RiseButtonType.Primary => Variant.Filled,
        RiseButtonType.Secondary => Variant.Outlined,
    };

    private string GetSizeClass() => Size switch
    {
        RiseButtonSize.Small => "px-4 py-2",
        RiseButtonSize.Medium => "px-6 py-3",
        RiseButtonSize.Large => "px-10 py-4",
        RiseButtonSize.XLarge => "px-16 py-6",
    };

    public enum RiseButtonType
    {
        Primary,
        Secondary
    }

    public enum RiseButtonSize
    {
        Small,
        Medium,
        Large,
        XLarge
    }
}