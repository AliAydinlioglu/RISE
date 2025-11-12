using System.Globalization;
using Microsoft.AspNetCore.Components;
using static System.String;


namespace Rise.Client.Components.Card;

public partial class RiseCard
{
    [Inject] public required NavigationManager NavigationManager { get; set; }
    
    [Parameter, EditorRequired] public DateTime Date { get; set; }
    [Parameter, EditorRequired] public string Title { get; set; } = Empty;
    [Parameter] public string Description { get; set; } = Empty;
    [Parameter] public string CardHeader { get; set; } = Empty;
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? OnClickNavigateTo { get; set; }
    
    protected string DayOfMonth { get; set; } = Empty;
    protected string MonthAbbreviation { get; set; } = Empty;
    protected string BackgroundTitle { get; set; } = Empty;
    protected string DateString { get; set; } = Empty;
    
    protected override void OnParametersSet()
    {
        DayOfMonth = Date.ToString("dd");
        MonthAbbreviation = Date.ToString("MMM", new CultureInfo("nl-NL")).TrimEnd('.');
        BackgroundTitle = Concat(Title.Where(char.IsUpper)); 
        DateString = Date.ToString("dd.MM.yyyy");
    }

    private void HandleClick()
    {
        if (!IsNullOrWhiteSpace(OnClickNavigateTo))
        {
            NavigationManager.NavigateTo(OnClickNavigateTo);
        }
    }
}