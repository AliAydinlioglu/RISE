using MudBlazor;
using Rise.Client.Attributes;

namespace Rise.Client.Contact;


[HomeBlock(icon: @Icons.Material.Outlined.EventNote, label: "Contact", route: "/contact")]
public partial class ContactIndex
{
    private List<ContactService> contactServices = new()
    {
        new ContactService
        {
            Name = "Studentensecretariaat",
            OpeningHours = new Dictionary<DayOfWeek, List<(TimeSpan, TimeSpan)>>
            {
                { DayOfWeek.Monday,    new() { (TimeSpan.Parse("08:30"), TimeSpan.Parse("12:00")), (TimeSpan.Parse("13:00"), TimeSpan.Parse("16:00")) }},
                { DayOfWeek.Tuesday,   new() { (TimeSpan.Parse("08:30"), TimeSpan.Parse("18:30")) }},
                { DayOfWeek.Wednesday, new() { (TimeSpan.Parse("08:30"), TimeSpan.Parse("12:00")), (TimeSpan.Parse("13:00"), TimeSpan.Parse("16:00")) }},
                { DayOfWeek.Friday,    new() { (TimeSpan.Parse("08:30"), TimeSpan.Parse("12:00")), (TimeSpan.Parse("13:00"), TimeSpan.Parse("16:00")) }},
            }
        },
        new ContactService
        {
            Name = "Bib",
            OpeningHours = new Dictionary<DayOfWeek, List<(TimeSpan, TimeSpan)>>
            {
                { DayOfWeek.Monday,    new() { (TimeSpan.Parse("08:30"), TimeSpan.Parse("12:00")), (TimeSpan.Parse("13:00"), TimeSpan.Parse("16:00")) }},
                { DayOfWeek.Tuesday,   new() { (TimeSpan.Parse("08:30"), TimeSpan.Parse("18:30")) }},
                { DayOfWeek.Wednesday, new() { (TimeSpan.Parse("08:30"), TimeSpan.Parse("12:00")), (TimeSpan.Parse("13:00"), TimeSpan.Parse("16:00")) }},
                { DayOfWeek.Thursday,  new() { (TimeSpan.Parse("08:30"), TimeSpan.Parse("12:00")), (TimeSpan.Parse("13:00"), TimeSpan.Parse("16:00")) }},
                { DayOfWeek.Friday,    new() { (TimeSpan.Parse("08:30"), TimeSpan.Parse("12:00")), (TimeSpan.Parse("13:00"), TimeSpan.Parse("16:00")) }},
            }
        },
        new ContactService
        {
            Name = "Standaard Student Shop",
            OpeningHours = new Dictionary<DayOfWeek, List<(TimeSpan, TimeSpan)>>()
            {
                { DayOfWeek.Thursday,  new() { (TimeSpan.Parse("08:30"), TimeSpan.Parse("12:00")), (TimeSpan.Parse("13:00"), TimeSpan.Parse("16:00")) }},
            }
        }
    };

    public class ContactService
    {
        public string Name { get; set; } = string.Empty;
        public Dictionary<DayOfWeek, List<(TimeSpan Open, TimeSpan Close)>> OpeningHours { get; set; } = [];

        public bool IsOpen => IsOpenNow();

        private bool IsOpenNow()
        {
            var now = DateTime.Now;
            var today = now.DayOfWeek;

            if (!OpeningHours.TryGetValue(today, out var timeRanges))
                return false;

            var currentTime = now.TimeOfDay;

            return timeRanges.Any(range => currentTime >= range.Open && currentTime <= range.Close);
        }
    }

}


