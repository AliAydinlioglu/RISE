using Rise.Shared.Common;

namespace Rise.Shared.SchoolEvents
{
    public interface ISchoolEventService
    {
        Task<Result<SchoolEventResponse.Index>> GetIndexAsync(QueryRequest.SkipTake request, CancellationToken ctx);

        Task<Result<SchoolEventResponse.Detail>> GetDetailByIdAsync(int id, CancellationToken ctx);
    }
}
