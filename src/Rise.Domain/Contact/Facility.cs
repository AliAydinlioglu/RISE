﻿namespace Rise.Domain.Contact;

public class Facility : Entity
{
    private Facility() { }

    public Facility(string name)
    {
        Name = Guard.Against.NullOrWhiteSpace(name);
        FacilityCategory = new FacilityCategory("Onbekend");
    }

    public Facility(string name, FacilityCategory serviceCategory) : this(name)
    {
        FacilityCategory = serviceCategory;
    }

    public string Name { get; private set; }

    public FacilityCategory FacilityCategory { get; private set; }

    public string? Description { get; private set; }

    public FacilityLocation? Location { get; private set; }

    public List<ContactPeriod> OpeningHours { get; private set; } = [];

    public List<string> Remarks { get; private set; } = [];

    public List<CommunicationChannel> CommunicationChannels { get; private set; } = [];

    public void DescribeService(string description)
    {
        Description = description;
    }

    public void AddCommunicationChannel(CommunicationChannel communicationChannel)
    {
        CommunicationChannels.Add(communicationChannel);
    }

    public void ChangeLocation(FacilityLocation location)
    {
        Location = location;
    }

    public void AddRemark(string remark)
    {
        Remarks.Add(remark);
    }

    public void ChangeOpeningsHours(List<ContactPeriod> contactPeriod)
    {
        OpeningHours = contactPeriod;
    }

    /// <summary>
    /// Check if it is necessary to know if the service is open or not
    /// </summary>
    /// <returns>bool</returns>
    public bool HasOpeningHours()
    {
        return OpeningHours.Count > 0;
    }

    /// <summary>
    /// Check if it is open on a specific date time
    /// </summary>
    public bool IsOpenOn(DateOnly date, TimeOnly hour)
    {
        var daily = OpeningHours.FirstOrDefault(d => d.ContactDate == date);
        return daily?.ContactHours.Any(tr => hour.IsBetween(tr.StartTime, tr.EndTime)) ?? false;
    }

    /// <summary>
    /// Check if it is open now
    /// </summary>
    public bool IsOpen()
    {
        var now = DateTime.Now;
        return IsOpenOn(DateOnly.FromDateTime(now), TimeOnly.FromDateTime(now));
    }
}
