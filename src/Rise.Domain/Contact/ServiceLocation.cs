
namespace Rise.Domain.Contact;

public class ServiceLocation: ValueObject
{
    private ServiceLocation() { }

    public ServiceLocation(StructuredAddress structuredAddress, string locationName)
    {
        LocationName = locationName;
        ServiceAddress = structuredAddress;
    }

    public StructuredAddress ServiceAddress { get; private set; }
    public string LocationName { get; private set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ServiceAddress;
        yield return LocationName;
    }
}
