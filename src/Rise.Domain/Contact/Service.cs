namespace Rise.Domain.Contact;

public class Service(string name, ServiceCategory? serviceCategory) : Entity
{
    public string Name { get; private set; } = name;

    public ServiceCategory ServiceCategory { get; private set; } 
        = serviceCategory ?? new ServiceCategory("Onbekend");

    public string? Description { get; private set; }

    public ServiceLocation? Location { get; private set; }

    public ContactPeriod? OpeningHours { get; private set; }

    public HashSet<string> Remarks { get; private set; } = [];

    public List<CommunicationChannel> CommunicationChannels { get; private set; } = [];

    public void DescribeService(string description)
    {
        Description = description;
    }

    public void AddCommunicationChannel(CommunicationChannel communicationChannel)
    {
        CommunicationChannels.Add(communicationChannel);
    }

    public void ChangeLocation(ServiceLocation location)
    {
        Location = location;
    }

    public void AddRemark(string remark)
    {
        Remarks.Add(remark);
    }

    public void ChangeOpeningsHours(ContactPeriod contactPeriod)
    {
        OpeningHours = contactPeriod;
    }
}
