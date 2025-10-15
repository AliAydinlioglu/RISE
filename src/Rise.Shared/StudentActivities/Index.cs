namespace Rise.Shared.StudentActivities;

public static partial class StudentActivityResponse
{
    public class Index
    {
        public IEnumerable<StudentActivityDto.Index> StudentActivities { get; set; } = [];
        public int TotalCount { get; set; }
    }
  
}