
namespace Rise.Domain.Contact;

public class CommunicationChannel(string name, string link, CommunicationTypes typeOfCommunication = CommunicationTypes.None): ValueObject
{
    public string Name { get; private set; } = name;
    public string Link { get; private set; } = link;
    public CommunicationTypes TypeOfCommunication { get; private set; } = typeOfCommunication;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return Link;
        yield return TypeOfCommunication;
    }
}