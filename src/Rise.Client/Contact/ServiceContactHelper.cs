using Rise.Shared.Contact;

namespace Rise.Client.Contact;

public static class ServiceContactHelper
{
    private static bool IsOpen(IEnumerable<ContactDto.ContactPeriodDto> openingHours)
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

    public static string ShowOpeningInfo(IEnumerable<ContactDto.ContactPeriodDto> openingHours)
    {
        if (!openingHours.Any())
            return "";

        return IsOpen(openingHours) ? "OPEN" : "GESLOTEN";
    }
}
