using MudBlazor;
using Rise.Client.Components.Form;
using Shouldly;

namespace Rise.Client.Components.FormComponents;

public class GivenAnInputField : MudBlazorTestBase
{
    private const string DefaultLabel = "Voornaam";

    [Fact]
    public void WhenInputIsRenderedWithRequiredLabel_ThenLabelShouldBeVisible()
    {
        var cut = RenderTextFieldWithLabel();

        cut.Markup.ShouldContain(DefaultLabel);
        cut.Find("label").TextContent.ShouldBe(DefaultLabel);
    }

    [Fact]
    public void WhenInputIsRenderedWithoutOptionalParams_ThenDefaultsShouldBeSet()
    {
        var cut = RenderTextFieldWithLabel();
        var mudTextField = GetMudTextField(cut);

        mudTextField.Disabled.ShouldBeFalse();
        mudTextField.Error.ShouldBeFalse();
        mudTextField.ErrorText.ShouldBeNull();
        mudTextField.HelperText.ShouldBeNull();
        mudTextField.Variant.ShouldBe(Variant.Outlined);
    }

    [Fact]
    public void WhenHelperTextIsProvided_ThenItShouldBeRendered()
    {
        const string helperText = "Vul je volledige naam in";

        var cut = RenderTextFieldWithLabel(parameters => parameters
            .Add(param => param.HelperText, helperText)
        );
        var mudTextField = GetMudTextField(cut);

        mudTextField.HelperText.ShouldBe(helperText);
        cut.Markup.ShouldContain(helperText);
    }

    [Fact]
    public void WhenErrorStateIsTrue_ThenErrorTextShouldBeRendered()
    {
        const string errorText = "Voornaam is verplicht";

        var cut = RenderTextFieldWithLabel(parameters => parameters
            .Add(param => param.Error, true)
            .Add(param => param.ErrorText, errorText)
        );
        var mudTextField = GetMudTextField(cut);

        mudTextField.Error.ShouldBeTrue();
        mudTextField.ErrorText.ShouldBe(errorText);
        cut.Markup.ShouldContain(errorText);
    }

    [Fact]
    public void WhenDisabledIsTrue_ThenInputShouldBeDisabled()
    {
        var cut = RenderTextFieldWithLabel(parameters => parameters
            .Add(param => param.Disabled, true)
        );
        var mudTextField = GetMudTextField(cut);

        mudTextField.Disabled.ShouldBeTrue();
        cut.Markup.ShouldContain("disabled");
    }

    
    private IRenderedComponent<RiseTextField> RenderTextFieldWithLabel(
        Action<ComponentParameterCollectionBuilder<RiseTextField>>? additionalParameters = null)
    {
        return RenderComponent<RiseTextField>(parameters =>
        {
            parameters.Add(param => param.Label, DefaultLabel);
            additionalParameters?.Invoke(parameters);
        });
    }

    private static MudTextField<string> GetMudTextField(IRenderedComponent<RiseTextField> cut)
    {
        return cut.FindComponent<MudTextField<string>>().Instance;
    }
}