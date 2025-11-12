using Ardalis.Result;
using Rise.Domain.Locations;
using Rise.Domain.SchoolEvents;
using Rise.Shared.Common;
using Rise.Shared.Locations;
using Rise.Shared.SchoolEvents;
using Rise.TestDoubles;

namespace Rise.Client.SchoolEvents;

public class FakeSchoolEventService : ISchoolEventService
{
    private IEnumerable<SchoolEvent>? _schoolEventsForIndex;
    private SchoolEvent? _schoolEventsForDetails;

    public void SetSchoolEventsForIndex(int count)
    {
        var location = SchoolEventTestDataFactory.CreateDefaultLocation();
        _schoolEventsForIndex = SchoolEventTestDataFactory.CreateTestSchoolEvents(count, location);
    }
    
    public IEnumerable<SchoolEvent> GetSchoolEventsForIndex()
    {
        return _schoolEventsForIndex ?? [];
    }
    
    public void SetSchoolEventForDetails(SchoolEvent schoolEvent)
    {
        _schoolEventsForDetails = schoolEvent;
    }
    
    public SchoolEvent? GetSchoolEventForDetails()
    {
        return _schoolEventsForDetails;
    }
    
    public Task<Result<SchoolEventResponse.Index>> GetIndexAsync(QueryRequest.SkipTake request, CancellationToken ctx)
    {
        var events = _schoolEventsForIndex?
            .Skip(request.Skip)?
            .Take(request.Take)?
            .Select(ToIndexDto)?
            .ToList();
        
        var result = new SchoolEventResponse.Index
        {
            SchoolEvents = events,
            TotalCount = _schoolEventsForIndex?.Count() ?? 0
        };
        
        return Task.FromResult(Result.Success(result));
    }

    public Task<Result<SchoolEventResponse.Detail>> GetDetailByIdAsync(int id, CancellationToken ctx)
    {   
        var result = new SchoolEventResponse.Detail()
        {
            SchoolEvent = _schoolEventsForDetails != null ? ToDetailDto(_schoolEventsForDetails) : null
        };
        
        return Task.FromResult(Result.Success(result));
    }
    
    private static SchoolEventDto.Detail ToDetailDto(SchoolEvent se)
    {
        return new SchoolEventDto.Detail
        {
            Id = se.Id,
            Title = se.Title,
            Description = se.Description,
            Date = se.Date,
            StartTime = se.TimeRange.StartTime,
            EndTime = se.TimeRange.EndTime,
            Price = se.Price,
            RegisterLink = se.RegisterLink,
            Capacity = se.Capacity,
            Registrable = se.Registrable,
            Publicity = se.Publicity,
            Category = se.Category,
            ImageUrl = se.ImageUrl,
            Location = ToLocationDto(se.Location),
        };
    }
    
    private static SchoolEventDto.Index ToIndexDto(SchoolEvent se)
    {
        return new SchoolEventDto.Index
        {
            Id = se.Id,
            Title = se.Title,
            Description = se.Description,
            Date = se.Date,
            StartTime = se.TimeRange.StartTime,
            EndTime = se.TimeRange.EndTime,
            Price = se.Price,
            RegisterLink = se.RegisterLink,
            Capacity = se.Capacity,
            Registrable = se.Registrable,
            Publicity = se.Publicity,
            Category = se.Category,
            ImageUrl = se.ImageUrl,
            Location = ToLocationDto(se.Location),
        };
    }
    
    private static LocationDto.Index ToLocationDto(Location loc)
    {
        return new LocationDto.Index
        {
            Id = loc.Id,
            Name = loc.Name,
            Street = loc.Street,
            HouseNumber = loc.HouseNumber,
            City = loc.City,
            Postcode = loc.Postcode,
            BusNumber = loc.BusNumber
        };
    }
}