
namespace Rise.Domain.Contact;

public class ServiceLocation(StructuredAddress structuredAddress): ValueObject
{
    public StructuredAddress ServiceAddress { get; private set; } = structuredAddress;
    public string LocationName { get; private set; } = string.Empty;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ServiceAddress;
        yield return LocationName;
    }
}
