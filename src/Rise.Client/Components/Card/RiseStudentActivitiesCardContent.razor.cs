using Microsoft.AspNetCore.Components;
using Rise.Shared.Locations;

namespace Rise.Client.Components.Card;

public partial class RiseStudentActivitiesCardContent : RiseCard
{
    
    [Parameter, EditorRequired] public TimeOnly StartTime { get; set; }
    [Parameter, EditorRequired] public TimeOnly EndTime { get; set; }
    [Parameter, EditorRequired] public LocationDto.Index Location{ get; set; }
    [Parameter, EditorRequired] public string StudentClubName { get; set; }
    [Parameter] public string ImageUrl { get; set; } = string.Empty;
    
    private string LocationString { get; set; } = string.Empty;
    private string TimeRangeString {get; set;} = string.Empty;
    
    protected override void OnParametersSet()
    {
        LocationString = Location.Name != string.Empty 
            ? $"{Location.Name}" 
            : $"{Location.Street} {Location.HouseNumber} {Location.BusNumber}, {Location.Postcode} {Location.City}";
        TimeRangeString = $"{StartTime} - {EndTime}";
    }
}