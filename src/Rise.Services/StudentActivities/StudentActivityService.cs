using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Rise.Domain.StudentActivities;
using Rise.Persistence;
using Rise.Services.Identity;
using Rise.Shared.Common;
using Rise.Shared.Locations;
using Rise.Shared.StudentActivities;
using Rise.Shared.StudentClubs;

namespace Rise.Services.StudentActivities;

public class StudentActivityService(ApplicationDbContext dbContext, ISessionContextProvider sessionContextProvider) : IStudentActivityService
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
            .Select(sa => ToIndexDto(sa))
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

        var detail = ToDetailDto(studentActivity);

        return Result.Success(new StudentActivityResponse.Detail { StudentActivity = detail });
    }

    private static StudentActivityDto.Index ToIndexDto(StudentActivity sa)
    {
        return new StudentActivityDto.Index
        {
            Id = sa.Id,
            Title = sa.Title,
            Description = sa.Description,
            Date = sa.Date,
            StartTime = sa.TimeRange.StartTime,
            EndTime = sa.TimeRange.EndTime,
            ImageUrl = sa.ImageUrl,
            Location = ToLocationDto(sa.Location),
            StudentClub = toStudentClubSummaryDto(sa.StudentClub)
        };
    }

    private static StudentActivityDto.Detail ToDetailDto(StudentActivity sa)
    {
        return new StudentActivityDto.Detail
        {
            Id = sa.Id,
            Title = sa.Title,
            Description = sa.Description,
            Date = sa.Date,
            StartTime = sa.TimeRange.StartTime,
            EndTime = sa.TimeRange.EndTime,
            ImageUrl = sa.ImageUrl,
            Location = ToLocationDto(sa.Location),
            StudentClub = ToStudentClubIndexDto(sa.StudentClub)
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

    private static StudentClubDto.Index ToStudentClubIndexDto(StudentClub club)
    {
        return new StudentClubDto.Index
        {
            Id = club.Id,
            Name = club.Name,
            Description = club.Description,
            LogoUrl = club.LogoUrl
        };
    }

    private static StudentClubDto.Summary toStudentClubSummaryDto(StudentClub club)
    {
        return new StudentClubDto.Summary
        {
            Id = club.Id,
            Name = club.Name
        };
    }
}