namespace Rise.Shared.StudentActivities;


public static partial class StudentActivityRequest
{
    public class Detail
    {
       public int Id { get; set; }
    }
}

public static partial class StudentActivityResponse
{
    public class Detail
    {
        public StudentActivityDto.Detail StudentActivity { get; set; }
    }
}