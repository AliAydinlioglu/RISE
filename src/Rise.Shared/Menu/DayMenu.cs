namespace Rise.Shared.Menu;

public static partial class MenuRequest
{
    public class DayMenu
    {
        public int? RestoId { get; set; }
        public DateTimeOffset Date { get; set; }      
    }  
}

public static partial class MenuResponse
{
    public class DayMenu
    {
        public MenuItemCategoryDto[] MenuItemCategories { get; set; } = [];      
    }
}