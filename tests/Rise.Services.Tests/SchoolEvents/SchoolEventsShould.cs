using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using Rise.Persistence;
using Rise.Services.SchoolEvents;
using Rise.Shared.Common;
using Rise.Shared.SchoolEvents;
using Rise.TestDoubles;

namespace Rise.Services.Tests.SchoolEvents;

public class SchoolEventsShould
{
    [Fact]
    public async Task GetAllSchoolEvents_ReturnsAllEvents()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: nameof(GetAllSchoolEvents_ReturnsAllEvents))
            .Options;

        using var dbContext = new ApplicationDbContext(options);

        var location = SchoolEventTestDataFactory.CreateDefaultLocation();

        var date1 = new DateTimeOffset(2025, 9, 10, 0, 0, 0, TimeSpan.Zero);
        var date2 = new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero);

        var schoolEvent1 = SchoolEventTestDataFactory.CreateSchoolEvent(
            "Info Sessie Bachelorproef",
            "Informatie over je bachelorproef",
            date1,
            location);

        var schoolEvent2 = SchoolEventTestDataFactory.CreateSchoolEvent(
            "Open Lesdag",
            "Kom kennismaken met HOGENT",
            date2,
            location);

        dbContext.Locations.Add(location);
        dbContext.SchoolEvents.AddRange(schoolEvent1, schoolEvent2);
        await dbContext.SaveChangesAsync();

        ISchoolEventService service = new SchoolEventService(dbContext, null);

        // Act
        var result = await service.GetIndexAsync(new QueryRequest.SkipTake { }, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.SchoolEvents.Count().ShouldBe(2);
        result.Value.TotalCount.ShouldBe(2);
        result.Value.SchoolEvents.Any(e => e.Title == "Info Sessie Bachelorproef").ShouldBeTrue();
        result.Value.SchoolEvents.Any(e => e.Title == "Open Lesdag").ShouldBeTrue();

        var eventInfoSessie = result.Value.SchoolEvents.FirstOrDefault(e => e.Title == "Info Sessie Bachelorproef");
        eventInfoSessie.ShouldNotBeNull();
        eventInfoSessie.Description.ShouldBe("Informatie over je bachelorproef");
        eventInfoSessie.Date.ShouldBe(new DateTimeOffset(2025, 9, 10, 0, 0, 0, TimeSpan.Zero));
        eventInfoSessie.StartTime.ShouldBe(new TimeOnly(14, 0, 0)); 
        eventInfoSessie.EndTime.ShouldBe(new TimeOnly(16, 0, 0));
        eventInfoSessie.ImageUrl.ShouldBe("/images/infosessiebachelorproef.png");
        eventInfoSessie.Location.Name.ShouldBe("Test Location");
        eventInfoSessie.Price.ShouldBe(0m);
        eventInfoSessie.Capacity.ShouldBe(50);
        eventInfoSessie.Registrable.ShouldBeTrue();
        eventInfoSessie.Publicity.ShouldBe("Public");

        var eventOpenLesdag = result.Value.SchoolEvents.FirstOrDefault(e => e.Title == "Open Lesdag");
        eventOpenLesdag.ShouldNotBeNull();
        eventOpenLesdag.Description.ShouldBe("Kom kennismaken met HOGENT");
        eventOpenLesdag.Date.ShouldBe(new DateTimeOffset(2025, 10, 10, 0, 0, 0, TimeSpan.Zero));
        eventInfoSessie.StartTime.ShouldBe(new TimeOnly(14, 0, 0));
        eventInfoSessie.EndTime.ShouldBe(new TimeOnly(16, 0, 0));
        eventOpenLesdag.Location.Name.ShouldBe("Test Location");
    }

    [Fact]
    public async Task GetAllSchoolEvents_CorrectPagination()
    {
        const int AMOUNT_OF_EVENTS = 5;
        const int SKIP = 1;
        const int TAKE = 2;
        var filters = new Dictionary<string, object?>()
        {
            { "Date", new DateTime(2025,9,5) }
        };

        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: nameof(GetAllSchoolEvents_CorrectPagination))
            .Options;

        using var dbContext = new ApplicationDbContext(options);

        var location = SchoolEventTestDataFactory.CreateDefaultLocation();
        var events = SchoolEventTestDataFactory.CreateTestSchoolEventsSameDate(AMOUNT_OF_EVENTS, location);

        dbContext.Locations.Add(location);
        dbContext.SchoolEvents.AddRange(events);
        await dbContext.SaveChangesAsync();

        var service = new SchoolEventService(dbContext, null);

        // Act
        var result = await service.GetIndexAsync(
            new QueryRequest.SkipTake { Skip = SKIP, Take = TAKE, Filters = filters},
            CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.TotalCount.ShouldBe(AMOUNT_OF_EVENTS);
        result.Value.SchoolEvents.Count().ShouldBe(TAKE);
        result.Value.SchoolEvents.Any(e => e.Title == "EventTest2").ShouldBeTrue();
        result.Value.SchoolEvents.Any(e => e.Title == "EventTest3").ShouldBeTrue();
        result.Value.SchoolEvents.Any(e => e.Title == "EventTest1").ShouldBeFalse();
    }

    [Fact]
    public async Task GetSchoolEventById_Returns()
    {
        const int AMOUNT_OF_EVENTS = 5;
        const int TO_TEST_EVENT_ID = 2;

        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: nameof(GetSchoolEventById_Returns))
            .Options;

        using var dbContext = new ApplicationDbContext(options);

        var location = SchoolEventTestDataFactory.CreateDefaultLocation();
        var events = SchoolEventTestDataFactory.CreateTestSchoolEvents(AMOUNT_OF_EVENTS, location);

        dbContext.Locations.Add(location);
        dbContext.SchoolEvents.AddRange(events);
        await dbContext.SaveChangesAsync();

        var service = new SchoolEventService(dbContext, null);

        // Act
        var result = await service.GetDetailByIdAsync(TO_TEST_EVENT_ID, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.SchoolEvent.ShouldNotBeOfType(typeof(Array));
        result.Value.SchoolEvent.Id.ShouldBe(TO_TEST_EVENT_ID);
        result.Value.SchoolEvent.Title.ShouldBe($"EventTest{TO_TEST_EVENT_ID}");
        result.Value.SchoolEvent.Description.ShouldBe($"DescTest{TO_TEST_EVENT_ID}");
        result.Value.SchoolEvent.ImageUrl.ShouldBe($"/images/eventtest{TO_TEST_EVENT_ID}.png");
        result.Value.SchoolEvent.Location.Name.ShouldBe("Test Location");
        result.Value.SchoolEvent.Price.ShouldBe(0m);
        result.Value.SchoolEvent.Registrable.ShouldBeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(999)]
    [InlineData(6)]
    public async Task GetSchoolEventById_Invalid(int invalidId)
    {
        const int AMOUNT_OF_EVENTS = 5;

        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"GetSchoolEventById_Invalid_{invalidId}")
            .Options;

        using var dbContext = new ApplicationDbContext(options);

        var location = SchoolEventTestDataFactory.CreateDefaultLocation();
        var events = SchoolEventTestDataFactory.CreateTestSchoolEvents(AMOUNT_OF_EVENTS, location);

        dbContext.Locations.Add(location);
        dbContext.SchoolEvents.AddRange(events);
        await dbContext.SaveChangesAsync();

        var service = new SchoolEventService(dbContext, null);

        // Act
        var result = await service.GetDetailByIdAsync(invalidId, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.NotFound);
    }

    [Fact]
    public async Task GetAllSchoolEvents_FiltersByDate()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: nameof(GetAllSchoolEvents_FiltersByDate))
            .Options;

        using var dbContext = new ApplicationDbContext(options);

        var location = SchoolEventTestDataFactory.CreateDefaultLocation();

        var pastEvent = SchoolEventTestDataFactory.CreateSchoolEvent(
            "Past Event",
            "Event in het verleden",
            new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
            location);

        var futureEvent = SchoolEventTestDataFactory.CreateSchoolEvent(
            "Future Event",
            "Toekomstig event",
            new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero),
            location);

        dbContext.Locations.Add(location);
        dbContext.SchoolEvents.AddRange(pastEvent, futureEvent);
        await dbContext.SaveChangesAsync();

        var service = new SchoolEventService(dbContext, null);

        // Act - alleen toekomstige events
        var result = await service.GetIndexAsync(
            new QueryRequest.SkipTake
            {
                Filters = new Dictionary<string, object?>{ { "Date", new DateTime(2026,1,1) } }
            },
            CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.SchoolEvents.Any(e => e.Title == "Future Event").ShouldBeTrue();
    }
}