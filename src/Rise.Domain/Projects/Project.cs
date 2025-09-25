namespace Rise.Domain.Projects;

/// <summary>
/// A project is something that a technician is working on.
/// </summary>
public class Project : Entity
{
    private string _name = string.Empty;

    public string Name
    {
        get => _name;
        set => _name = Guard.Against.NullOrEmpty(value);
    }
    public Technician Technician { get; init; } = default!;

    /// <summary>
    /// The address is immutable and owned by the <see cref="Project"/>. If you want to change the address, create a new Address and link it to the project.
    /// <see cref="ProjectConfiguration"/>
    /// </summary>
    public Address Location { get; init; } = default!;

    /// <summary>
    /// Entity Framework Core Constructor
    /// </summary>
    private Project()
    {
        
    }

    public Project(string name, Technician technician, Address location)
    {
        Name = name;
        Technician = Guard.Against.Null(technician);
        Location = Guard.Against.Null(location);
    }

    public bool CanBeEditedBy(Technician technician)
    {
        return Technician == technician; // due to Entity (comparision via ID)
    }

    public void Edit(string name)
    {
        Name = Guard.Against.NullOrEmpty(name);       
    }
}