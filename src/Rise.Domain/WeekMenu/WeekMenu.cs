namespace Rise.Domain.WeekMenu;

public class WeekMenu : Entity
{
    public DateTimeOffset StartDate { get; set; } 
    public Resto Resto { get; init; } = null!;
    
    private readonly List<WeekMenuItem> _weekMenuItems = [];
    public IReadOnlyList<WeekMenuItem> WeekMenuItems => _weekMenuItems.AsReadOnly();
    
    private WeekMenu() { }
    
    public WeekMenu(DateTimeOffset startDate, Resto resto)
    {
        StartDate = Guard.Against.Null(startDate);
        Resto = Guard.Against.Null(resto);
    }

    public void AddWeekMenuItem(WeekMenuItem weekMenuItem)
    {
        _weekMenuItems.Add(Guard.Against.Null(weekMenuItem));
    }
}