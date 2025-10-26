using Microsoft.AspNetCore.Components;
using MudBlazor;
using Rise.Client.Components.Form;
using Shouldly;

namespace Rise.Client.Components.FormComponents;

public abstract class GivenAnInputSetupTest<TRiseInputComponent> : MudBlazorTestSetup
    where TRiseInputComponent : ComponentBase, IRiseInputComponentProps
{
    private const string DefaultLabel = "Voornaam";

    [Fact]
    public void WhenInputIsRenderedWithRequiredLabel_ThenLabelShouldBeVisible()
    {
        var cut = RenderInputFieldWithLabel();

        cut.Markup.ShouldContain(DefaultLabel);
        cut.Find("label").TextContent.ShouldBe(DefaultLabel);
    }

    [Fact]
    public void WhenInputIsRenderedWithoutOptionalParams_ThenDefaultsShouldBeSet()
    {
        var cut = RenderInputFieldWithLabel();
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

        var cut = RenderInputFieldWithLabel(parameters => parameters
            .Add(param => param.HelperText, helperText)
        );
        var mudTextField = GetMudTextField(cut);

        mudTextField.HelperText.ShouldBe(helperText);
        cut.Markup.ShouldContain(helperText);
    }

    [Fact]
    public void WhenErrorStateIsTrue_ThenErrorTextShouldBeRendered()
    {
        const string errorText = "Voornaam error";

        var cut = RenderInputFieldWithLabel(parameters => parameters
            .Add(param => param.Error, true)
            .Add(param => param.ErrorText, errorText)
        );
        var riseTextField = cut.Instance;
    
        riseTextField.Error.ShouldBeTrue();
        riseTextField.ErrorText.ShouldBe(errorText);
    }

    [Fact]
    public void WhenRequiredStateIsTrue_ThenRequiredTextShouldBeRendered()
    {
        const string requiredText = "Voornaam is verplicht";

        var cut = RenderInputFieldWithLabel(parameters => parameters
            .Add(param => param.Required, true)
            .Add(param => param.RequiredError, requiredText)
        );
        var mudTextField = GetMudTextField(cut);

        mudTextField.Required.ShouldBeTrue();
        mudTextField.RequiredError.ShouldBe(requiredText);
    }

    [Fact]
    public void WhenDisabledIsTrue_ThenInputShouldBeDisabled()
    {
        var cut = RenderInputFieldWithLabel(parameters => parameters
            .Add(param => param.Disabled, true)
        );
        var mudTextField = GetMudTextField(cut);

        mudTextField.Disabled.ShouldBeTrue();
        cut.Markup.ShouldContain("disabled");
    }

    
    protected IRenderedComponent<TRiseInputComponent> RenderInputFieldWithLabel(
        Action<ComponentParameterCollectionBuilder<TRiseInputComponent>>? additionalParameters = null)
    {
        return RenderComponent<TRiseInputComponent>(parameters =>
        {
            parameters.Add(param => param.Label, DefaultLabel);
            additionalParameters?.Invoke(parameters);
        });
    }

    protected MudTextField<string> GetMudTextField(IRenderedComponent<TRiseInputComponent> cut)
    {
        return cut.FindComponent<MudTextField<string>>().Instance;
    }
}