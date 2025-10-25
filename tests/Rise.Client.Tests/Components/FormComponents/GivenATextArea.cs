using Rise.Client.Components.Form;
using Shouldly;

namespace Rise.Client.Components.FormComponents;

public class GivenATextArea : GivenAnInputSetupTest<RiseTextArea>
{
    [Fact]
    public void WhenTextAreaIsRenderedWithoutLinesSpecified_ThenLinesShouldDefaultTo5()
    {
        var cut = RenderInputFieldWithLabel();
        
        var mudTextField = GetMudTextField(cut);
        
        mudTextField.Lines.ShouldBe(5);
    }

    [Fact]
    public void WhenTextAreaIsRenderedWithLinesSpecified_ThenLinesShouldBeSet()
    {
        var cut = RenderInputFieldWithLabel(
            parameters => parameters.Add(param => param.Lines, 3)
            );
        
        var mudTextField = GetMudTextField(cut);
        
        mudTextField.Lines.ShouldBe(3);
    }
}