namespace Rise.Shared.Identity;

/// <summary>
/// Provides a centralized definition of application roles as static properties.
/// This class is used to define role names that are utilized across the application
/// for authentication and authorization purposes.
/// </summary>
public static class AppRoles
{
    public const string Administrator = "Administrator";
    public const string Technician = "Technician";
    
    public const string Public = "1abb2640-a706-4624-b209-89d94f038384";
    public const string Regular = "29da4f78-e833-4da6-b029-31ff91b59ae0";
    public const string Distance = "5c1e700b-2b73-4f56-94cb-bd6952dc20a6";
}