namespace Rise.Shared.Navigation;

/// <summary>
/// Contains data transfer objects (DTOs) used for navigation-related operations.
/// </summary>
public class NavigationDto
{
    /// <summary>
    /// Represents a navigation get containing minimalistic navigation details.
    /// </summary>
    public class Get
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
    }
}
