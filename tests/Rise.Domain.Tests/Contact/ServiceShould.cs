using Rise.Domain.Common;
using Rise.Domain.Contact;

namespace Rise.Domain.Tests.Contact;

public class ServiceShould
{
    private const string ServiceName = "Studentensecretariaat";
    private const string Description = "Service voor administratieve vragen";
    private readonly FacilityCategory _category = new("Administratief");
    
    [Fact]
    public void BeCreated_WithNameOnly()
    {
        var service = new Facility(ServiceName);

        service.ShouldNotBeNull();
        service.Name.ShouldBe(ServiceName);
        service.ServiceCategory.ShouldNotBeNull();
        service.ServiceCategory.Name.ShouldBe("Onbekend");
        service.Description.ShouldBeNull();
        service.Location.ShouldBeNull();
        service.OpeningHours.ShouldBeEmpty();
        service.Remarks.ShouldBeEmpty();
        service.CommunicationChannels.ShouldBeEmpty();
    }

    [Fact]
    public void BeCreated_WithNameAndCategory()
    {
        var service = new Facility(ServiceName, _category);

        service.ShouldNotBeNull();
        service.Name.ShouldBe(ServiceName);
        service.ServiceCategory.ShouldBe(_category);
        service.Description.ShouldBeNull();
        service.Location.ShouldBeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ThrowException_WhenNameIsNullOrWhitespace(string name)
    {
        Should.Throw<ArgumentException>(() => new Facility(name));
    }

    [Fact]
    public void AddDescription_Successfully()
    {
        var service = new Facility(ServiceName);

        service.DescribeService(Description);

        service.Description.ShouldBe(Description);
    }

    [Fact]
    public void AddCommunicationChannel_Successfully()
    {
        var service = new Facility(ServiceName);
        var channel = new CommunicationChannel("Email", "test@hogent.be", CommunicationTypes.Email);

        service.AddCommunicationChannel(channel);

        service.CommunicationChannels.ShouldContain(channel);
        service.CommunicationChannels.Count.ShouldBe(1);
    }

    [Fact]
    public void AddMultipleCommunicationChannels_Successfully()
    {
        var service = new Facility(ServiceName);
        var emailChannel = new CommunicationChannel("Email", "test@hogent.be", CommunicationTypes.Email);
        var phoneChannel = new CommunicationChannel("Telefoon", "09 123 45 67", CommunicationTypes.Phone);

        service.AddCommunicationChannel(emailChannel);
        service.AddCommunicationChannel(phoneChannel);

        service.CommunicationChannels.Count.ShouldBe(2);
        service.CommunicationChannels.ShouldContain(emailChannel);
        service.CommunicationChannels.ShouldContain(phoneChannel);
    }

    [Fact]
    public void ChangeLocation_Successfully()
    {
        var service = new Facility(ServiceName);
        var address = new StructuredAddress("Valentin Vaerwyckweg", 1, 9000, "Gent", "");
        var location = new FacilityLocation(address, "Schoonmeersen");

        service.ChangeLocation(location);

        service.Location.ShouldBe(location);
    }

    [Fact]
    public void AddRemark_Successfully()
    {
        var service = new Facility(ServiceName);
        var remark = "Gesloten op feestdagen";

        service.AddRemark(remark);

        service.Remarks.ShouldContain(remark);
        service.Remarks.Count.ShouldBe(1);
    }

    [Fact]
    public void AddMultipleRemarks_Successfully()
    {
        var service = new Facility(ServiceName);
        var remark1 = "Gesloten op feestdagen";
        var remark2 = "Gesloten tijdens examenperiode";

        service.AddRemark(remark1);
        service.AddRemark(remark2);

        service.Remarks.Count.ShouldBe(2);
        service.Remarks.ShouldContain(remark1);
        service.Remarks.ShouldContain(remark2);
    }

    [Fact]
    public void ChangeOpeningHours_Successfully()
    {
        var service = new Facility(ServiceName);
        var openingHours = new List<ContactPeriod>
        {
            new ContactPeriod(new DateOnly(2025, 11, 6), new List<TimeRange>
            {
                new TimeRange(new TimeOnly(9, 0), new TimeOnly(12, 0)),
                new TimeRange(new TimeOnly(13, 0), new TimeOnly(17, 0))
            })
        };

        service.ChangeOpeningsHours(openingHours);

        service.OpeningHours.ShouldBe(openingHours);
        service.OpeningHours.Count.ShouldBe(1);
    }

    [Fact]
    public void HasOpeningHours_ReturnTrue_WhenOpeningHoursExist()
    {
        var service = new Facility(ServiceName);
        var openingHours = new List<ContactPeriod>
        {
            new ContactPeriod(new DateOnly(2025, 11, 6), new List<TimeRange>
            {
                new TimeRange(new TimeOnly(9, 0), new TimeOnly(17, 0))
            })
        };
        service.ChangeOpeningsHours(openingHours);

        var hasHours = service.HasOpeningHours();

        hasHours.ShouldBeTrue();
    }

    [Fact]
    public void HasOpeningHours_ReturnFalse_WhenNoOpeningHours()
    {
        var service = new Facility(ServiceName);

        var hasHours = service.HasOpeningHours();

        hasHours.ShouldBeFalse();
    }

    [Fact]
    public void IsOpenOn_ReturnTrue_WhenWithinOpeningHours()
    {
        var service = new Facility(ServiceName);
        var date = new DateOnly(2025, 11, 6);
        var openingHours = new List<ContactPeriod>
        {
            new ContactPeriod(date, new List<TimeRange>
            {
                new TimeRange(new TimeOnly(9, 0), new TimeOnly(17, 0))
            })
        };
        service.ChangeOpeningsHours(openingHours);

        var isOpen = service.IsOpenOn(date, new TimeOnly(14, 0));

        isOpen.ShouldBeTrue();
    }

    [Fact]
    public void IsOpenOn_ReturnFalse_WhenOutsideOpeningHours()
    {
        var service = new Facility(ServiceName);
        var date = new DateOnly(2025, 11, 6);
        var openingHours = new List<ContactPeriod>
        {
            new ContactPeriod(date, new List<TimeRange>
            {
                new TimeRange(new TimeOnly(9, 0), new TimeOnly(17, 0))
            })
        };
        service.ChangeOpeningsHours(openingHours);

        var isOpen = service.IsOpenOn(date, new TimeOnly(18, 0));

        isOpen.ShouldBeFalse();
    }

    [Fact]
    public void IsOpenOn_ReturnFalse_WhenDifferentDate()
    {
        var service = new Facility(ServiceName);
        var date = new DateOnly(2025, 11, 6);
        var openingHours = new List<ContactPeriod>
        {
            new ContactPeriod(date, new List<TimeRange>
            {
                new TimeRange(new TimeOnly(9, 0), new TimeOnly(17, 0))
            })
        };
        service.ChangeOpeningsHours(openingHours);

        var isOpen = service.IsOpenOn(new DateOnly(2025, 11, 7), new TimeOnly(14, 0));

        isOpen.ShouldBeFalse();
    }

    [Fact]
    public void IsOpenOn_ReturnTrue_WhenTimeIsAtStartOfRange()
    {
        var service = new Facility(ServiceName);
        var date = new DateOnly(2025, 11, 6);
        var openingHours = new List<ContactPeriod>
        {
            new ContactPeriod(date, new List<TimeRange>
            {
                new TimeRange(new TimeOnly(9, 0), new TimeOnly(17, 0))
            })
        };
        service.ChangeOpeningsHours(openingHours);

        var isOpen = service.IsOpenOn(date, new TimeOnly(9, 0));

        isOpen.ShouldBeTrue();
    }

    [Fact]
    public void IsOpenOn_HandleMultipleTimeRanges()
    {
        var service = new Facility(ServiceName);
        var date = new DateOnly(2025, 11, 6);
        var openingHours = new List<ContactPeriod>
        {
            new ContactPeriod(date, new List<TimeRange>
            {
                new TimeRange(new TimeOnly(9, 0), new TimeOnly(12, 0)),
                new TimeRange(new TimeOnly(13, 0), new TimeOnly(17, 0))
            })
        };
        service.ChangeOpeningsHours(openingHours);

        service.IsOpenOn(date, new TimeOnly(10, 0)).ShouldBeTrue();
        service.IsOpenOn(date, new TimeOnly(12, 30)).ShouldBeFalse();
        service.IsOpenOn(date, new TimeOnly(14, 0)).ShouldBeTrue();
    }
}

