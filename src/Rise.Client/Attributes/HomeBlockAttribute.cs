namespace Rise.Client.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class HomeBlockAttribute : Attribute
{
        public string Label { get; }
        public string Icon { get; }
        public string Route { get; }
        public string[]? Roles { get; }

        public HomeBlockAttribute(string label, string icon, string route, string[]? roles = null)
        {
            Label = label;
            Icon = icon;
            Route = route;
            Roles = roles;
        }
}