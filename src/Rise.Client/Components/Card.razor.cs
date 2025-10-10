using Microsoft.AspNetCore.Components;
using static System.String;

namespace Rise.Client.Components;

public partial class Card
{
    [Parameter, EditorRequired] public DateTime Date { get; set; }
    [Parameter, EditorRequired] public string Title { get; set; } = Empty;
    [Parameter] public string Description { get; set; } = Empty;
    [Parameter] public string CardHeader { get; set; } = Empty;
    
    private string DayOfMonth { get; set; } = Empty;
    private string MonthAbbreviation { get; set; } = Empty;
    private string BackgroundTitle { get; set; } = Empty;

    protected override void OnParametersSet()
    {
        DayOfMonth = Date.ToString("dd");
        MonthAbbreviation = Date.ToString("MMM");
        BackgroundTitle = Concat(Title.Where(char.IsUpper));        
    }
}