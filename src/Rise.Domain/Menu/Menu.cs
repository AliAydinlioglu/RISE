namespace Rise.Domain.Menu;

public class Menu : Entity
{
    public DateTimeOffset Date { get; set; } 
    public Resto Resto { get; init; } = null!;
    
    private readonly List<MenuItem> _menuItems = [];
    public IReadOnlyList<MenuItem> MenuItems => _menuItems.AsReadOnly();
    
    private Menu() { }
    
    public Menu(DateTimeOffset date, Resto resto)
    {
        Date = Guard.Against.Null(date);
        Resto = Guard.Against.Null(resto);
    }

    public void AddWeekMenuItems(IEnumerable<MenuItem> menuItems)
    {
        foreach (var menuItem in menuItems)
        {
            _menuItems.Add(Guard.Against.Null(menuItem));
        }
    }
}