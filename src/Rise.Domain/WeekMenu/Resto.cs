using Rise.Domain.Contact;
using Rise.Domain.Locations;

namespace Rise.Domain.WeekMenu;

public class Resto : Entity
{
    public string Name { get; set; } = null!;
    public Location Location { get; init; } = null!;
    
    private readonly List<ContactPeriod> _contactPeriods = [];
    public IReadOnlyList<ContactPeriod> ContactPeriods => _contactPeriods.AsReadOnly();
    
    private Resto() { }

    public Resto(string name, Location location)
    {
        Name = Guard.Against.NullOrWhiteSpace(name);
        Location = location;
    }

    public void AddContactPeriods(IEnumerable<ContactPeriod> contactPeriods)
    {
        _contactPeriods.AddRange(contactPeriods);
    }
}