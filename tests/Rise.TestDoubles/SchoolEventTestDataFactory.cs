using Rise.Domain.Common;
using Rise.Domain.Locations;
using Rise.Domain.SchoolEvents;
using Rise.Domain.StudentActivities;

namespace Rise.TestDoubles;

public static class SchoolEventTestDataFactory
{
    public static Location CreateDefaultLocation()
    {
        return new Location("Test Location", "Test Straat", 1, 9000, "Gent",null);
    }

    public static SchoolEvent CreateSchoolEvent(
        string title,
        string description,
        DateTimeOffset date,
        Location location)
    {
        var timeRange = new TimeRange(new TimeOnly(14, 0, 0), new TimeOnly(16, 0, 0));

        return new SchoolEvent(
            title,
            description,
            date,
            timeRange,
            price: 0m,
            registerLink: "https://hogent.be/registreer",
            capacity: 50,
            registrable: true,
            publicity: "Public",
            category: "Sport",
            imageUrl: $"/images/{title.ToLower().Replace(" ", "")}.png",
            location
        );
    }

    public static List<SchoolEvent> CreateTestSchoolEvents(int count, Location location)
    {
        var events = new List<SchoolEvent>();

        for (int i = 1; i <= count; i++)
        {
            var date = new DateTimeOffset(2025, 9, i, 0, 0, 0, TimeSpan.Zero);
            var timeRange = new TimeRange(new TimeOnly(14, 0, 0), new TimeOnly(16, 0, 0));

            var schoolEvent = new SchoolEvent(
                $"EventTest{i}",
                $"DescTest{i}",
                date,
                timeRange,
                price: 0m,
                registerLink: "https://hogent.be/registreer",
                capacity: 50,
                registrable: true,
                publicity: "Public",
                category: "Sport",
                imageUrl: $"/images/eventtest{i}.png",
                location
            );

            events.Add(schoolEvent);
        }

        return events;
    }
}