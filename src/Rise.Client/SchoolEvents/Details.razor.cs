using Microsoft.AspNetCore.Components;

namespace Rise.Client.SchoolEvents;

public partial class Details : ComponentBase
{
    [Parameter] public string? Id { get; set; }

    private string Title { get; set; }
    private string? Description { get; set; } = String.Empty;
    private string Address { get; set; }
    private string LocationName { get; set; }
    private string TimeString { get; set; }
    private string LocalDateString { get; set; }
    private string Category { get; set; }
}