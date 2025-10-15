using Ardalis.Result;
using Rise.Domain.StudentActivities;
using Rise.Services.StudentActivities;
using Rise.Services.Tests.StudentActivities;
using Rise.Shared.Common;
using Rise.Shared.Locations;
using Rise.Shared.StudentActivities;

namespace Xunit.StudentActivities;

public class FakeStudentActivitiesService : IStudentActivityService
{
    public Task<Result<StudentActivityResponse.Index>> GetIndexAsync(QueryRequest.SkipTake request,
        CancellationToken ctx)
    {
        var studentActivities = StudentActivityTestDataFactory.CreateTestActivities(
            5,
            StudentActivityTestDataFactory.CreateDefaultLocation(),
            StudentActivityTestDataFactory.CreateDefaultStudentClub()
        );


        var wrapper = new StudentActivityResponse.Index
        {
            StudentActivities = studentActivities.Select(StudentActivityService.ToIndexDto).ToList(),
            TotalCount = 5,
        };

        return Task.FromResult(Result.Success(wrapper));
    }

    public Task<Result<StudentActivityResponse.Detail>> GetDetailByIdAsync(int id, CancellationToken ctx)
    {
     
        var studentActivity = StudentActivityTestDataFactory.CreateTestActivities(
            1,
            StudentActivityTestDataFactory.CreateDefaultLocation(),
            StudentActivityTestDataFactory.CreateDefaultStudentClub()
        )[0];
        
        var detailDto = StudentActivityService.ToDetailDto(studentActivity);
        return Task.FromResult(Result.Success(new StudentActivityResponse.Detail { StudentActivity = detailDto }));
    }
}