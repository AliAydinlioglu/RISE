namespace Rise.Domain.Contact;

public class CommunicationChannel(string name, string link)
{
    public string Name { get; private set; } = name;
    public string Link { get; private set; } = link;

    public CommunicationTypes TypeOfCommunication { get; private set; } = CommunicationTypes.None;
}