using Microsoft.AspNetCore.Components;
using Rise.Shared.SchoolEvents;

namespace Rise.Client.SchoolEvents;

public partial class Details : ComponentBase
{
    [Parameter] public int Id { get; set; }

    private bool _showError;
    
    private SchoolEventDto.Detail? _schoolEvent;
    [Inject] public required ISchoolEventService SchoolEventService { get; set; }
    [Inject] public required NavigationManager NavigationManager { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (Id <= 0)
        {
            Log.Error("Id is invalid");
            NavigationManager.NavigateTo("/notfound");
            return;
        }
        
        var result = await SchoolEventService.GetDetailByIdAsync(Id, CancellationToken.None);

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