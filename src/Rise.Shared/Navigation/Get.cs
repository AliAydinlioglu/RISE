namespace Rise.Shared.Navigation;

/// <summary>
/// Represents the request structure for navigation-related operations.
/// </summary>
public static partial class NavigationRequest
{
    public class Get
    {
        public string ContentLocation { get; set; } = null!;   
    }
}

/// <summary>
/// Represents the response structure for navigation-related operations.
/// </summary>
public static partial class NavigationResponse
{
    public class Get
    {
        public IEnumerable<NavigationDto.Get> NavigationItems { get; set; } = [];
    }
}