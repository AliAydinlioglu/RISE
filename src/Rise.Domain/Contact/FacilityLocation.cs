
namespace Rise.Domain.Contact;

public class FacilityLocation: ValueObject
{
    private FacilityLocation() { }

    public FacilityLocation(StructuredAddress structuredAddress, string locationName)
    {
        LocationName = locationName;
        FaclitityAddress = structuredAddress;
    }

    public StructuredAddress FaclitityAddress { get; private set; }
    public string LocationName { get; private set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FaclitityAddress;
        yield return LocationName;
    }
}
