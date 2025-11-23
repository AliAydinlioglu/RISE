namespace Rise.Shared.Menu;

public static partial class WeekMenuRequest
{
    public class DayMenu
    {
        public int? RestoId { get; set; }
        public DateTimeOffset Date { get; set; }      
    }  
}

public static partial class WeekMenuResponse
{
    public class DayMenu
    {
        public string RestoName { get; set; } = null!;
        public DateTimeOffset Date { get; set; }
        public MenuItemCategoryDto[] MenuItemCategories { get; set; } = [];      
    }
}