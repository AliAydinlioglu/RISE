namespace Rise.Domain.WeekMenu;

public class WeekMenuCategory : Entity
{
    public string Name { get; set; } = null!;
    
    private WeekMenuCategory() { }
    
    public WeekMenuCategory(string name)
    {
        Name = Guard.Against.NullOrWhiteSpace(name);
    }
}