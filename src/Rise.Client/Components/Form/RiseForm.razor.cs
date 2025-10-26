using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Rise.Client.Components.Form;

public partial class RiseForm : ComponentBase
{
    private MudForm? _mudForm;
    [Parameter] public RenderFragment? ChildContent { get; set; }
    
    [Parameter, EditorRequired] public required object Model { get; set; }
    
    [Parameter] public object? Validation { get; set; }
    [Parameter, EditorRequired] public required EventCallback OnValidSubmit { get; set; }
    [Parameter] public EventCallback OnInvalidSubmit { get; set; }
    [Parameter] public EventCallback OnCancel { get; set; }
    
    [Parameter] public string SubmitButtonText { get; set; } = "Opslaan";
    [Parameter] public bool IsSubmitting { get; set; }

    private async Task HandleSubmit()
    {
        if (_mudForm == null) return;

        await _mudForm.Validate();

        if (_mudForm.IsValid)
        {
            await OnValidSubmit.InvokeAsync();
        }
        else if (OnInvalidSubmit.HasDelegate)
        {
            await OnInvalidSubmit.InvokeAsync();
        }
    }

    private async Task HandleCancel()
    {
        if (OnCancel.HasDelegate)
        {
            await OnCancel.InvokeAsync();
        }
    }

    public void ResetValidation()
    {
        _mudForm?.ResetValidation();
    }
}