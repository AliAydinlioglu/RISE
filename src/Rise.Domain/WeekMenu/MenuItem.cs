namespace Rise.Domain.WeekMenu;

public class MenuItem : Entity
{
    public string Name { get; set; } = null!;
    public double? StudentPrice { get; set; }
    public double? ExternalPrice { get; set; }
    public bool IsOnPriceList { get; set; }

    public PriceListCategory PriceListCategory { get; init; } = null!;
    
    private readonly List<DietaryRestriction> _dietaryRestrictions = [];
    public IReadOnlyList<DietaryRestriction> DietaryRestrictions => _dietaryRestrictions.AsReadOnly();
    
    private readonly List<Allergen> _allergens = [];
    public IReadOnlyList<Allergen> Projects => _allergens.AsReadOnly();
    
    private readonly List<WeekMenuItem> _weekMenuItems = [];
    public IReadOnlyList<WeekMenuItem> WeekMenuItems => _weekMenuItems.AsReadOnly();

    private MenuItem() { }
    
    public MenuItem(
        string name, 
        bool isOnPriceList, 
        PriceListCategory priceListCategory, 
        double? studentPrice = null,
        double? externalPrice = null)
    {
        Name = Guard.Against.NullOrWhiteSpace(name);
        IsOnPriceList = isOnPriceList;
        PriceListCategory = Guard.Against.Null(priceListCategory);
        StudentPrice = studentPrice;
        ExternalPrice = externalPrice;
    }

    public void AddDietaryRestriction(DietaryRestriction dietaryRestriction)
    {
        _dietaryRestrictions.Add(Guard.Against.Null(dietaryRestriction));
    }

    public void AddAllergen(Allergen allergen)
    {
        _allergens.Add(Guard.Against.Null(allergen));
    }
    
    public void AddWeekMenuItem(WeekMenuItem weekMenuItem)
    {
        _weekMenuItems.Add(Guard.Against.Null(weekMenuItem));
    }

    public void AddWeekMenuItems(IEnumerable<WeekMenuItem> weekMenuItems)
    {
        foreach (var weekMenuItem in weekMenuItems)
            AddWeekMenuItem(weekMenuItem);
    }
}