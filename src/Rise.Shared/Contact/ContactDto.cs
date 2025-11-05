namespace Rise.Shared.Contact;

public static class ContactDto
{
    public class Index
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string ServiceCategoryName { get; set; }
        public string? Description { get; set; }
        public ServiceLocationDto? Location { get; set; }
        public IEnumerable<ContactPeriodDto> OpeningHours { get; set; } = [];
        public IEnumerable<string> Remarks { get; set; } = [];
        public IEnumerable<CommunicationChannelDto> CommunicationChannels { get; set; } = [];
    }

    public class ServiceLocationDto
    {
        public required StructuredAddressDto ServiceAddress { get; set; }
        public required string LocationName { get; set; }
    }

    public class StructuredAddressDto
    {
        public required string Street { get; set; }
        public required int HouseNumber { get; set; }
        public string? BusNumber { get; set; }
        public required int Postcode { get; set; }
        public required string City { get; set; }
    }

    public class ContactPeriodDto
    {
        public required DateOnly ContactDate { get; set; }
        public IEnumerable<TimeRangeDto> ContactHours { get; set; } = [];
    }

    public class TimeRangeDto
    {
        public required TimeOnly StartTime { get; set; }
        public required TimeOnly EndTime { get; set; }
    }

    public class CommunicationChannelDto
    {
        public required string Name { get; set; }
        public required string Link { get; set; }
        public required string TypeOfCommunication { get; set; }
    }
}

