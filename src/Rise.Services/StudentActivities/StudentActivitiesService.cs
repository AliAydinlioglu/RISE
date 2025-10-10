using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Rise.Persistence;
using Rise.Services.Identity;
using Rise.Shared.Common;
using Rise.Shared.Locations;
using Rise.Shared.StudentActivities;
using Rise.Shared.StudentClubs;

namespace Rise.Services.StudentActivities;

public class StudentActivitiesService(ApplicationDbContext dbContext, ISessionContextProvider sessionContextProvider) : IStudentActivitiesService
{
    public async Task<Result<StudentActivityResponse.Index>> GetIndexAsync(QueryRequest.SkipTake request,
        CancellationToken ctx)
    {
        var query = dbContext.StudentActivities
            .Include(sa => sa.Location)
            .Include(sa => sa.StudentClub)
            .AsQueryable();

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
                StartTime = sa.StartTime,
                EndTime = sa.EndTime,
                ImageUrl = sa.ImageUrl,
                Location = new LocationDto.Index
                {
                    Id = sa.Location.Id,
                    Name = sa.Location.Name,
                    Street = sa.Location.Street,
                    HouseNumber = sa.Location.HouseNumber,
                    Postcode = sa.Location.Postcode,
                    City = sa.Location.City
                },
                StudentClub = new StudentClubDto.Summary
                {
                    Id = sa.StudentClub.Id,
                    Name = sa.StudentClub.Name
                }
            })
            .ToListAsync(ctx);
        
        return Result.Success(new StudentActivityResponse.Index
        {
            StudentActivities = studentActivities,
            TotalCount = totalCount,
        });
    }

    public async Task<Result<StudentActivityResponse.Detail>> GetDetailByIdAsync(int id, CancellationToken ctx)
    {
        var studentActivity = await dbContext.StudentActivities
            .Include(sa => sa.Location)
            .Include(sa => sa.StudentClub)
            .AsNoTracking()
            .FirstOrDefaultAsync(sa => sa.Id == id, ctx);

        if (studentActivity == null)
            return Result.NotFound($"Student activity with ID {id} not found.");

        var detail = new StudentActivityDto.Detail
        {
            Id = studentActivity.Id,
            Title = studentActivity.Title,
            Description = studentActivity.Description,
            Date = studentActivity.Date,
            StartTime = studentActivity.StartTime,
            EndTime = studentActivity.EndTime,
            ImageUrl = studentActivity.ImageUrl,
            Location = new LocationDto.Index
            {
                Id = studentActivity.Location.Id,
                Name = studentActivity.Location.Name,
                Street = studentActivity.Location.Street,
                HouseNumber = studentActivity.Location.HouseNumber,
                Postcode = studentActivity.Location.Postcode,
                City = studentActivity.Location.City
            },
            StudentClub = new StudentClubDto.Index
            {
                Id = studentActivity.StudentClub.Id,
                Name = studentActivity.StudentClub.Name,
                Description = studentActivity.StudentClub.Description,
                LogoUrl = studentActivity.StudentClub.LogoUrl
            }
        };

        return Result.Success(new StudentActivityResponse.Detail { StudentActivity = detail });
    }
}