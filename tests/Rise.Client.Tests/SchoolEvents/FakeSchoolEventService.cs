using Ardalis.Result;
using Rise.Domain.SchoolEvents;
using Rise.Services.SchoolEvents;
using Rise.Shared.Common;
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
            .Select(se => se.ToIndexDto())
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
            SchoolEvent = _schoolEventsForDetails != null ? _schoolEventsForDetails.ToDetailDto() : null
        };
        
        return Task.FromResult(Result.Success(result));
    }
}