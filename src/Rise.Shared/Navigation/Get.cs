namespace Rise.Shared.Navigation;

/// <summary>
/// Represents the response structure for navigation-related operations.
/// </summary>
public class NavigationResponse
{
    public class Get
    {
        public IEnumerable<NavigationDto.Get> NavigationItems { get; set; } = [];
    }
}
