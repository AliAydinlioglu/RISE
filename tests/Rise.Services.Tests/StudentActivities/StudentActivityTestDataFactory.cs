using Rise.Domain.StudentActivities;

namespace Rise.Services.Tests.StudentActivities;

public static class StudentActivityTestDataFactory
{
    public static StudentClub CreateDefaultStudentClub()
    {
     return  new("Club A", "A student club description", "/images/clubA.png");
    }

    public static Location CreateDefaultLocation()
    { 
        return new("Test Location", "Test street", 42, 1234, "Test City");
    }
        
    
    public static StudentActivity CreateStudentActivity(string title, string description, DateTime date,  Location location, StudentClub club) 
    {
        return new(title, description, date,
            new DateTime(date.Year, date.Month, date.Day, 18, 0, 0),
            new DateTime(date.Year, date.Month, date.Day, 22, 0, 0),
            $"/images/{title.ToLower()}.png", location, club);
    }
    
    
    public static List<StudentActivity> CreateTestActivities(int count, Location location, StudentClub club)
    {
        return Enumerable.Range(1, count)
            .Select(i => StudentActivityTestDataFactory.CreateStudentActivity(
                $"ActivityTest{i}", 
                $"DescTest{i}",
                new DateTime(2025, 9, 10 + i), 
                location, 
                club))
            .ToList();
    }
}