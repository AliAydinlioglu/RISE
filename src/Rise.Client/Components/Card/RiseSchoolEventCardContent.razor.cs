using Microsoft.AspNetCore.Components;
using Rise.Shared.Locations;

namespace Rise.Client.Components.Card;

public partial class RiseSchoolEventCardContent : RiseCard
{
    [Parameter, EditorRequired] public TimeOnly StartTime { get; set; }
    [Parameter, EditorRequired] public TimeOnly EndTime { get; set; }
    [Parameter, EditorRequired] public string Category { get; set; }
    [Parameter] public LocationDto.Index? Location { get; set; }

    private string TimeRangeString => $"{StartTime} - {EndTime}";
    private new string DateString => $"{Date:dddd d MMMM yyyy}"; 
}