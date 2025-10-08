namespace Rise.Domain.StudentActivities;

public class StudentClub : Entity
{
    public string Name { get; private set; }
    public string Description { get; private set;  }
    public string LogoUrl { get; private set; }
    
    public StudentClub(){}
    public StudentClub(string name, string description, string logoUrl)
    {
        Guard.Against.NullOrWhiteSpace(name);

        Name = name;
        Description = description;
        LogoUrl = logoUrl;
    }
}