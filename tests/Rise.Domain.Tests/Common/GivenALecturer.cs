using Rise.Domain.Calendar;

namespace Rise.Domain.Tests.Common;

public class GivenALecturer
{
    [Fact(DisplayName = "When FirstName is null, then exception should be thrown")]
    public void NullFirstName()
    {
        Should.Throw<ArgumentException>(() =>
            new Lecturer(null, "Doe"));
    }
    
    [Fact(DisplayName = "When FirstName is empty, then exception should be thrown")]
    public void EmptyFirstName()
    {
        Should.Throw<ArgumentException>(() =>
            new Lecturer("", "Doe"));
    }
    
    [Fact(DisplayName = "When LastName is null, then exception should be thrown")]
    public void NullLastName()
    {
        Should.Throw<ArgumentException>(() =>
            new Lecturer("John", null));
    }
    
    [Fact(DisplayName = "When LastName is empty, then exception should be thrown")]
    public void EmptyLastName()
    {
        Should.Throw<ArgumentException>(() =>
            new Lecturer("John", ""));
    }
}