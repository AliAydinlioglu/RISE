using Rise.Domain.Contact;
using Rise.Domain.Locations;

namespace Rise.Domain.Menu;

public class Resto : Entity
{
    public string Name { get; init; } = null!;
    
    public Location Location { get; init; } = null!;
    public PriceList? PriceList { get; init; }
    
    private readonly List<ContactPeriod> _openingHours = [];
    public IReadOnlyList<ContactPeriod> OpeningHours => _openingHours.AsReadOnly();
    
    private readonly List<Menu> _menus = [];
    public IReadOnlyList<Menu> Menus => _menus.AsReadOnly();
    
    private Resto() { }

    public Resto(string name, Location location, PriceList? priceList)
    {
        Name = Guard.Against.NullOrWhiteSpace(name);
        Location = Guard.Against.Null(location);
        PriceList = priceList;
    }

    public void AddContactPeriods(IEnumerable<ContactPeriod> contactPeriods)
    {
        foreach (var contactPeriod in contactPeriods)
        {
            _openingHours.Add(Guard.Against.Null(contactPeriod));
        }
    }
    
    public void AddMenus(IEnumerable<Menu> menus)
    {
        foreach (var menu in menus)
        {
            _menus.Add(Guard.Against.Null(menu));
        }
    }
}