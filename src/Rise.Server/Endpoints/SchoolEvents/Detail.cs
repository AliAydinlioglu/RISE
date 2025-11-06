using Rise.Shared.SchoolEvents;

namespace Rise.Server.Endpoints.SchoolEvents
{
    public class Detail(ISchoolEventService studentActivityService) : Endpoint<SchoolEventRequest.Detail, Result<SchoolEventResponse.Detail>>
    {
        public override void Configure()
        {
            Get("/api/school-events/{Id}");
            AllowAnonymous();
        }

        public override Task<Result<SchoolEventResponse.Detail>> ExecuteAsync(SchoolEventRequest.Detail req, CancellationToken ctx)
        {

            return studentActivityService.GetDetailByIdAsync(req.Id, ctx);
        }

    }
}