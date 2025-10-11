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
        public required string Label { get; set; }
        public required string Icon { get; set; }
        public required string Url { get; set; }
    }
}
