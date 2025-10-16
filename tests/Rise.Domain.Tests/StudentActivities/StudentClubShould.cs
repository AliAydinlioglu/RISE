using Rise.Domain.StudentActivities;

namespace Rise.Domain.Tests.StudentActivities;

public class StudentClubShould
{
    [Fact]
    public void CreateStudentClub_WhenValidParameters()
    {
        // Arrange
        var name = "Chess Club";
        var description = "A club for chess enthusiasts.";
        var logoUrl = "http://example.com/logo.png";

        // Act
        var studentClub = new StudentClub(name, description, logoUrl);

        // Assert
        studentClub.Name.ShouldBe(name);
        studentClub.Description.ShouldBe(description);
        studentClub.LogoUrl.ShouldBe(logoUrl);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CreateStudentClub_ThrowExceptionWhenInvalidParameters(string name)
    {
        Should.Throw<ArgumentException>(() => new StudentClub(name, null, null));
    }
}