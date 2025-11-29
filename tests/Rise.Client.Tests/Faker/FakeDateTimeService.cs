using Rise.Shared;

namespace Rise.Client.Faker;

public class FakeDateTimeService: IDateTimeService
{
    public DateTime Now => new DateTime(2025, 11, 28, 10, 30, 0);
    public DateTime Today => new DateTime(2025, 11, 28, 10, 30, 0);
}