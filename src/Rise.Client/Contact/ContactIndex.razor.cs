using MudBlazor;
using Rise.Client.Attributes;
using Rise.Shared.Contact;

namespace Rise.Client.Contact;

[HomeBlock(icon: @Icons.Material.Outlined.EventNote, label: "Contact", route: "/contact")]
public partial class ContactIndex
{
    private List<ContactDto.Index> contactServices = [];

    protected override async Task OnInitializedAsync()
    {
        contactServices = await ContactDataLoader.LoadContactsAsync();
    }

    public bool IsOpen(IEnumerable<ContactDto.ContactPeriodDto> openingHours)
    {
        var now = DateTime.Now;
        var today = DateOnly.FromDateTime(now);
        var currentTime = TimeOnly.FromDateTime(now);

        // Zoek alle periodes van vandaag (datum vergelijken op dag van week)
        var todayPeriods = openingHours
            .Where(p => p.ContactDate.DayOfWeek == today.DayOfWeek)
            .ToList();

        if (!todayPeriods.Any())
            return false;

        // Controleer of huidige tijd binnen een van de tijdsvensters valt
        return todayPeriods.Any(p =>
            p.ContactHours.Any(h =>
                currentTime >= h.StartTime && currentTime <= h.EndTime));
    }

}


