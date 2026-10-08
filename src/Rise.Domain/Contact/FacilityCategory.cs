
namespace Rise.Domain.Contact;

public class FacilityCategory(string name): ValueObject
{
    public string Name { get; private set; } = name;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
    }
}