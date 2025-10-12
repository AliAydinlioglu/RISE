namespace Rise.Domain.StudentActivities;

public class StudentClub : Entity
{
    public string Name { get; private set; }
    public string? Description { get; private set;  }
    public string? LogoUrl { get; private set; }
    
    private StudentClub(){}
    public StudentClub(string name, string description, string logoUrl)
    {
        Name = Guard.Against.NullOrWhiteSpace(name);;
        Description = description;
        LogoUrl = logoUrl;
    }
}