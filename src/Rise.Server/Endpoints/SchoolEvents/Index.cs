using Rise.Shared.SchoolEvents;
using Rise.Shared.Common;

namespace Rise.Server.Endpoints.SchoolEvents
{
    public class Index(ISchoolEventService studentActivityService) : Endpoint<QueryRequest.SkipTake, Result<SchoolEventResponse.Index>>
    {
        public override void Configure()
        {
            Get("/api/school-events");
            AllowAnonymous();
        }

        public override Task<Result<SchoolEventResponse.Index>> ExecuteAsync(QueryRequest.SkipTake req, CancellationToken ct)
        {
            return studentActivityService.GetIndexAsync(req, ct);
        }
    }
}