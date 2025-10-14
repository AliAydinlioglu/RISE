using Microsoft.AspNetCore.Components;
using Rise.Shared.Locations;

namespace Rise.Client.Components;

public partial class StudentActivitiesCardContent : Card
{
    
    [Parameter, EditorRequired] public TimeOnly StartTime { get; set; }
    [Parameter, EditorRequired] public TimeOnly EndTime { get; set; }
    // [Parameter] public string LocationName { get; set; } = string.Empty;
    [Parameter, EditorRequired] public LocationDto.Index Location{ get; set; }
    [Parameter, EditorRequired] public string StudentClubName { get; set; }
    [Parameter] public string ImageUrl { get; set; } = string.Empty;
    
    private string LocationString { get; set; } = string.Empty;
    private string TimeRangeString {get; set;} = string.Empty;
    
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        LocationString = Location.Name != string.Empty 
            ? $"{Location.Name}" 
            : $"{Location.Street} {Location.HouseNumber} {Location.BusNumber}, {Location.Postcode} {Location.City}";
        TimeRangeString = $"{StartTime} - {EndTime}";
    }
}