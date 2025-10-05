using Rise.Persistence;
using Rise.Services.Identity;
using Rise.Shared.Common;
using Rise.Shared.StudentActivities;

namespace Rise.Services.StudentActivities;

public class StudentActivitiesService(ApplicationDbContext dbContext, ISessionContextProvider sessionContextProvider) : IStudentActivitiesService
{
    public async Task<Result<StudentActivityResponse.Index>> GetIndexAsync(QueryRequest.SkipTake request,
        CancellationToken ctx)
    {
        var query = dbContext.StudentActivities.AsQueryable();

        var totalCount = await query.CountAsync(ctx);
        
        var studentActivities = await query.AsNoTracking()
            .Skip(request.Skip)
            .Take(request.Take)
            .Select(sa => new StudentActivityDto.Index
            {
                Id = sa.Id,
                Title = sa.Title,
                Description = sa.Description,
                Date = sa.Date,
                Location = sa.Location,
                StudentClub = sa.StudentClub
            })
            .ToListAsync(ctx);
        
        return Result.Success(new StudentActivityResponse.Index
        {
            StudentActivities = studentActivities,
            TotalCount = totalCount,
        });
    }
}