namespace Rise.Domain.Calendar;

/// <summary>
/// Represents a person that is a lecturer at HOGENT
/// </summary>
public class Lecturer : Entity
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    
    private Lecturer(){}
    
    public Lecturer(string firstName, string lastName)
    {
        FirstName = Guard.Against.NullOrWhiteSpace(firstName);
        LastName = Guard.Against.NullOrWhiteSpace(lastName);
    }
}