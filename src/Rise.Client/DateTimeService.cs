using Rise.Shared;

namespace Rise.Client;

public class DateTimeService : IDateTimeService
{
    public DateTime Now => DateTime.Now;
    public DateTime Today => DateTime.Today;
}