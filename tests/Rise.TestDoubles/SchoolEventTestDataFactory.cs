using System.Runtime.Intrinsics.X86;
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

    public static SchoolEvent CreateDefaultSchoolEvent()
    {
        var titel = "Title";
        var timeRange = new TimeRange(new TimeOnly(14, 0, 0), new TimeOnly(16, 0, 0));

        return new SchoolEvent(
            titel,
            "Beschrijving",
            DateTimeOffset.Now, 
            timeRange,
            price: (decimal)15.00,
            registerLink: "https://hogent.be/registreer",
            capacity: 50,
            registrable: true,
            publicity: "Public",
            category: "Sport",
            imageUrl: $"/images/{titel.ToLower().Replace(" ", "")}.png",
            CreateDefaultLocation()
        );
    }
    
    public static SchoolEvent CreateSchoolEvent(
        string title,
        string description,
        DateTimeOffset date,
        Location location,
        decimal price = 0m)
    {
        var timeRange = new TimeRange(new TimeOnly(14, 0, 0), new TimeOnly(16, 0, 0));

        return new SchoolEvent(
            title,
            description,
            date,
            timeRange,
            price: price,
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
    
    public static List<SchoolEvent> CreateTestSchoolEventsSameDate(int count, Location location)
    {
        var events = new List<SchoolEvent>();

        var date = new DateTimeOffset(2025, 9, 5, 0, 0, 0, TimeSpan.Zero);
        for (var i = 1; i <= count; i++)
        {
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