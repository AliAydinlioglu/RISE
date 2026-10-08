namespace Rise.Client.Models
{
    public record NavItemModel
    {
        public string Label { get; }
        public string Icon { get; }
        public string Link { get; }

        public NavItemModel(string label, string icon, string link)
        {
            Label = label;
            Icon = icon;
            Link = link;
        }
    }
}
