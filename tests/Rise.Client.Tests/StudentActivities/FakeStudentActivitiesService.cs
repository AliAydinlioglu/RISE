using Ardalis.Result;
using Rise.Shared.Common;
using Rise.Shared.StudentActivities;
using Rise.TestDoubles;

namespace Rise.Client.StudentActivities;

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
            StudentActivities = studentActivities.Select(Services.StudentActivities.StudentActivityService.ToIndexDto).ToList(),
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
        
        var detailDto = Services.StudentActivities.StudentActivityService.ToDetailDto(studentActivity);
        return Task.FromResult(Result.Success(new StudentActivityResponse.Detail { StudentActivity = detailDto }));
    }
}