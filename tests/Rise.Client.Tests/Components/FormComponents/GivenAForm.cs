using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Rise.Client.Components.Form;
using Shouldly;

namespace Rise.Client.Components.FormComponents;

public class GivenAForm : MudBlazorTestSetup
{
    [Fact]
    public void WhenFormIsRendered_ThenBothSubmitAndCancelButtonsShouldBeVisible()
    {
        var model = new TestFormModel();
        var cut = RenderForm(model);

        cut.Markup.ShouldContain("Opslaan");
        cut.Markup.ShouldContain("Annuleren");
        var riseButtons = cut.FindComponents<RiseButton>();
        riseButtons.Count.ShouldBe(2);
    }

    [Fact]
    public void WhenFormIsRendered_ThenSubmitButtonShouldBeOfPrimaryType()
    {
        var model = new TestFormModel();
        var cut = RenderForm(model);

        var riseButtons = cut.FindComponents<RiseButton>();
        var submitButton = riseButtons.First(b => b.Instance.ButtonType == ButtonType.Submit);
        
        submitButton.Instance.Type.ShouldBe(RiseButton.RiseButtonType.Primary);
    }

    [Fact]
    public void WhenFormIsRendered_ThenCancelButtonShouldBeOfSecondaryType()
    {
        var model = new TestFormModel();
        var cut = RenderForm(model);

        var riseButtons = cut.FindComponents<RiseButton>();
        var cancelButton = riseButtons.First(b => b.Instance.ButtonType == ButtonType.Button);
        
        cancelButton.Instance.Type.ShouldBe(RiseButton.RiseButtonType.Secondary);
    }
    
    [Fact]
    public void WhenFormIsRenderedWithDefaultSettings_ThenSubmitButtonShouldHaveTextOpslaan()
    {
        var model = new TestFormModel();
        var cut = RenderForm(model);

        var submitButton = cut.FindComponent<RiseButton>();
        submitButton.ShouldNotBeNull();
        submitButton.Markup.ShouldContain("Opslaan");
    }

    [Fact]
    public void WhenCustomSubmitButtonTextIsProvided_ThenSubmitButtonShouldHaveCustomText()
    {
        var submitButtonText = "Verzenden";
        var model = new TestFormModel();
        
        var cut = RenderForm(model, submitButtonText: submitButtonText);
        
        cut.Markup.ShouldContain(submitButtonText);
    }
    
    [Fact]
    public void WhenFormIsRenderedWithChildContent_ThenChildContentShouldBeRendered()
    {
        var model = new TestFormModel();
        var cut = RenderForm(
            model,
            childContent: builder =>
            {
                builder.OpenComponent<RiseTextField>(0);
                builder.AddAttribute(1, "Label", "Test child content");
                builder.CloseComponent();
            }, onValidSubmit: () => Task.CompletedTask); 

        cut.Markup.ShouldContain("Test child content");
    }
    
    [Fact]
    public void WhenFormIsRenderedWithModel_ThenModelShouldBeSet()
    {
        var model = new TestFormModel { FirstName = "John" };

        var cut = RenderForm(model);

        var mudForm = cut.FindComponent<MudForm>();
        mudForm.Instance.Model.ShouldBe(model);
    }

    [Fact]
    public void WhenFormIsSubmitting_ThenSubmitButtonShouldBeDisabled()
    {
        var model = new TestFormModel();
        var cut = RenderForm(model, isSubmitting: true);
        
        var riseButtons = cut.FindComponents<RiseButton>();
        var submitButton = riseButtons.First(b => b.Instance.ButtonType == ButtonType.Submit);
        
        submitButton.Instance.Disabled.ShouldBeTrue();

    }

    [Fact]
    public void WhenIsSubmitting_ThenLoaderShouldBeShownInButton()
    {
        var model = new TestFormModel();
        var cut = RenderForm(model, isSubmitting: true);
        
        var loader = cut.FindComponent<RiseLoader>();
        loader.ShouldNotBeNull();
    }
    
    [Fact]
    public async Task WhenFormIsSubmittedWithoutErrors_ThenOnValidSubmitShouldBeInvoked()
    {
        var model = new TestFormModel();
        var wasInvoked = false;
        
        var cut = RenderForm(model, onValidSubmit: () =>
        {
            wasInvoked = true;
            return Task.CompletedTask;
        });
        
        var riseButtons = cut.FindComponents<RiseButton>();
        var submitButton = riseButtons.First(b => b.Instance.ButtonType == ButtonType.Submit);
        await cut.InvokeAsync(async () => await submitButton.Instance.OnClick.InvokeAsync());

        wasInvoked.ShouldBeTrue();
    }
    
    [Fact]
    public async Task WhenFormIsSubmittedWithErrors_ThenOnInvalidSubmitShouldBeInvoked()
    {
        var model = new TestFormModel();
        var wasInvoked = false;

        var cut = RenderForm(
            model,
            childContent: builder =>
            {
                builder.OpenComponent<RiseTextField>(0);
                builder.AddAttribute(1, "Label", "First Name");
                builder.AddAttribute(2, "Value", model.FirstName);
                builder.AddAttribute(3, "Required", true);
                builder.AddAttribute(4, "RequiredError", "Required");
                builder.CloseComponent();
            },
            onInvalidSubmit: () =>
            {
                wasInvoked = true;
                return Task.CompletedTask;
            }
        );

        var riseButtons = cut.FindComponents<RiseButton>();
        var submitButton = riseButtons.First(b => b.Instance.ButtonType == ButtonType.Submit);
        
        await cut.InvokeAsync(async () => await submitButton.Instance.OnClick.InvokeAsync());
        
        wasInvoked.ShouldBeTrue();
    }
    
    [Fact]
    public async Task WhenCancelButtonIsClicked_ThenOnCancelShouldBeInvoked()
    {
        var model = new TestFormModel();
        var wasInvoked = false;
        
        var cut = RenderForm(model, onCancel: () =>
        {
            wasInvoked = true;
            return Task.CompletedTask;
        });

        var cancelButton = cut.FindAll("button").First(b => b.TextContent.Contains("Annuleren"));
        await cancelButton.ClickAsync(new MouseEventArgs());

        wasInvoked.ShouldBeTrue();
    }

    private IRenderedComponent<RiseForm> RenderForm(
        TestFormModel model,
        RenderFragment? childContent = null,
        string? submitButtonText = null,
        bool isSubmitting = false,
        Func<object, string, Task<IEnumerable<string>>>? validation = null,
        Func<Task>? onValidSubmit = null,
        Func<Task>? onInvalidSubmit = null,
        Func<Task>? onCancel = null)
    {
        onValidSubmit ??= () => Task.CompletedTask;
        onInvalidSubmit ??= () => Task.CompletedTask;
        onCancel ??= () => Task.CompletedTask;

        return RenderComponent<RiseForm>(parameters =>
        {
            parameters.Add(param => param.Model, model);
            parameters.Add(param => param.OnValidSubmit, EventCallback.Factory.Create(this, onValidSubmit));
            parameters.Add(param => param.OnInvalidSubmit, EventCallback.Factory.Create(this, onInvalidSubmit));
            parameters.Add(param => param.OnCancel, EventCallback.Factory.Create(this, onCancel));
            
            if (childContent != null)
                parameters.Add(param => param.ChildContent, childContent);
            
            if (submitButtonText != null)
                parameters.Add(param => param.SubmitButtonText, submitButtonText);
            
            if (isSubmitting)
                parameters.Add(param => param.IsSubmitting, isSubmitting);
            
            if (validation != null)
                parameters.Add(param => param.Validation, validation);
        });
    }
}

// Test model voor Form testen
// Gebaseerd op vragenformulier
public class TestFormModel
{
    public string FirstName { get; set; } = string.Empty;
}