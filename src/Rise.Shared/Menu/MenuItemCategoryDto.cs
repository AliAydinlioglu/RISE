namespace Rise.Shared.Menu;


public class MenuItemCategoryDto
{
    public string CategoryName { get; set; } = null!;
    public MenuItemDto[] MenuItems { get; set; } = [];
}
