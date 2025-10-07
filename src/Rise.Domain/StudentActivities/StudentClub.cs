namespace Rise.Domain.StudentActivities;

public class StudentClub : Entity
{
    public string Name { get; }
    public string Description { get; }
    public string LogoUrl { get; }
    
    public StudentClub(string name, string description, string logoUrl)
    {
        Guard.Against.NullOrWhiteSpace(name);

        Name = name;
        Description = description;
        LogoUrl = logoUrl;
    }
}