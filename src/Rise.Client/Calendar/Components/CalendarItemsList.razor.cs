using Microsoft.AspNetCore.Components;

namespace Rise.Client.Calendar.Components;

public partial class CalendarItemsList
{
    [Parameter, EditorRequired] public IEnumerable<CalendarViewItem>? Items { get; set; }
    [Parameter, EditorRequired] public string EmptyMessage { get; set; } = string.Empty;
}