using Rise.Domain.Contact;

namespace Rise.Shared.Contact;

public static class ContactDto
{
    public class Index
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string FacilityCategoryName { get; set; }
        public string? Description { get; set; }
        public FacilityLocationDto? Location { get; set; }
        public IEnumerable<ContactPeriodDto> OpeningHours { get; set; } = [];
        public bool? IsFacilityOpen { get; set; }
        public IEnumerable<string> Remarks { get; set; } = [];
        public IEnumerable<CommunicationChannelDto> CommunicationChannels { get; set; } = [];
    }

    public class FacilityLocationDto
    {
        public required StructuredAddressDto FacilityAddress { get; set; }
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
        public required CommunicationTypes TypeOfCommunication { get; set; }
    }
}

