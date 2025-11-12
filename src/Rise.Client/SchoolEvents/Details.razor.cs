using Microsoft.AspNetCore.Components;
using Rise.Shared.SchoolEvents;

namespace Rise.Client.SchoolEvents;

public partial class Details : ComponentBase
{
    [Parameter] public string? Id { get; set; }

    private bool _showError;
    
    private SchoolEventDto.Detail? _schoolEvent;
    [Inject] public required ISchoolEventService SchoolEventService { get; set; }
    [Inject] public required NavigationManager NavigationManager { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (string.IsNullOrWhiteSpace(Id))
        {
            Log.Error("Id is NULL or Whitespace");
            
            _showError = true;
            await InvokeAsync(StateHasChanged);
            return;
        }

        if (!int.TryParse(Id, out var idAsInt))
        {
            Log.Error("Parsing Id failed");
            
            _showError = true;
            await InvokeAsync(StateHasChanged);
            return;
        }
        
        var result = await SchoolEventService.GetDetailByIdAsync(idAsInt, CancellationToken.None);

        if (result is { IsSuccess: true, Value: not null, Value.SchoolEvent: not null})
        {
            _showError = false;
            _schoolEvent = result.Value.SchoolEvent;
        }
        else
        {
            _showError = true;
            Log.Error(string.Join(";",result.Errors));
        }
        
        await InvokeAsync(StateHasChanged);
    }
}