namespace Rise.Domain.WeekMenu;

public class WeekMenuItem : Entity
{
    public DayOfWeek DayOfWeek { get; set; }
    public WeekMenuCategory WeekMenuCategory { get; init; } = null!;
    public MenuItem MenuItem { get; init; } = null!;
    public WeekMenu WeekMenu { get; init; } = null!;
    
    private WeekMenuItem() { }

    public WeekMenuItem(WeekMenuCategory weekMenuCategory,  MenuItem menuItem,  WeekMenu weekMenu)
    {
        WeekMenuCategory = Guard.Against.Null(weekMenuCategory);
        MenuItem = Guard.Against.Null(menuItem);
        WeekMenu = Guard.Against.Null(weekMenu);
    }
}