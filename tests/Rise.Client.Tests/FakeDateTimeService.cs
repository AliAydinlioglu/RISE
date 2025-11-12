using Rise.Shared;

namespace Rise.Client;

public class FakeDateTimeService(DateTime fixedDateTime) : IDateTimeService
{
    private DateTime _fixedDateTime = fixedDateTime;
    
    public DateTime Now => _fixedDateTime;
    public DateTime Today => _fixedDateTime.Date;
    public void SetDateTime(DateTime dateTime) => _fixedDateTime = dateTime;
}