namespace Rise.Domain.Contact;

public class ServiceLocation(StructuredAddress structuredAddress)
{
    public StructuredAddress ServiceAddress { get; private set; } = structuredAddress;
}
