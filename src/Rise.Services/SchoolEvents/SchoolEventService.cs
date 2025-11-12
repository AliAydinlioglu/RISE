using Rise.Persistence;
using Rise.Services.Identity;
using Rise.Shared.Common;
using Rise.Shared.SchoolEvents;
using Microsoft.EntityFrameworkCore;

namespace Rise.Services.SchoolEvents
{
    public class SchoolEventService(ApplicationDbContext dbContext, ISessionContextProvider sessionContextProvider) : ISchoolEventService
    {
        public async Task<Result<SchoolEventResponse.Index>> GetIndexAsync(QueryRequest.SkipTake request, CancellationToken ctx)
        {
            if (!request.Filters.TryGetValue("Date", out var dateFilter) || 
                !DateTime.TryParse(dateFilter!.ToString()![..10], out var dateFilterDate))
            {
                Log.Error("{0}: Invalid filter date", dateFilter);
                return Result.Error("Date filter is missing or in wrong format.");
            }
            
            var query = dbContext.SchoolEvents
                .Include(sa => sa.Location)
                .Where(sa => sa.Date.Date == dateFilterDate.Date)
                .AsQueryable();

            var totalCount = await query.CountAsync(ctx);

            var studentEvents = await query
                .AsNoTracking()
                .OrderBy(sa => sa.TimeRange.StartTime)
                .Select(sa => sa.ToIndexDto())
                .Skip(request.Skip)
                .Take(request.Take)
                .ToListAsync(ctx);

            return Result.Success(new SchoolEventResponse.Index
            {
                SchoolEvents = studentEvents,
                TotalCount = totalCount,
            });
        }

        public async Task<Result<SchoolEventResponse.Detail>> GetDetailByIdAsync(int id, CancellationToken ctx)
        {
            var schoolEvent = await dbContext.SchoolEvents
                .Include(sa => sa.Location)
                .AsNoTracking()
                .FirstOrDefaultAsync(sa => sa.Id == id, ctx);

            if (schoolEvent == null)
                return Result.NotFound($"Student event with ID {id} not found.");

            var detail = schoolEvent.ToDetailDto();

            return Result.Success(new SchoolEventResponse.Detail { SchoolEvent = detail });
        }
    }
}