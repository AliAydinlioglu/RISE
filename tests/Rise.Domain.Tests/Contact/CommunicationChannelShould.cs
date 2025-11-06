using Rise.Domain.Contact;

namespace Rise.Domain.Tests.Contact;

public class CommunicationChannelShould
{
    private const string ChannelName = "Email";
    private const string Link = "test@hogent.be";
    
    [Fact]
    public void BeCreated_WithValidParameters()
    {
        var channel = new CommunicationChannel(ChannelName, Link, CommunicationTypes.Email);

        channel.ShouldNotBeNull();
        channel.Name.ShouldBe(ChannelName);
        channel.Link.ShouldBe(Link);
        channel.TypeOfCommunication.ShouldBe(CommunicationTypes.Email);
    }

    [Fact]
    public void BeCreated_WithDefaultCommunicationType()
    {
        var channel = new CommunicationChannel(ChannelName, Link);

        channel.ShouldNotBeNull();
        channel.TypeOfCommunication.ShouldBe(CommunicationTypes.None);
    }

    [Fact]
    public void BeEqual_WhenAllPropertiesAreTheSame()
    {
        var channel1 = new CommunicationChannel(ChannelName, Link, CommunicationTypes.Email);
        var channel2 = new CommunicationChannel(ChannelName, Link, CommunicationTypes.Email);

        channel1.ShouldBe(channel2);
    }

    [Fact]
    public void NotBeEqual_WhenNamesAreDifferent()
    {
        var channel1 = new CommunicationChannel("Email", Link, CommunicationTypes.Email);
        var channel2 = new CommunicationChannel("Telefoon", Link, CommunicationTypes.Email);

        channel1.ShouldNotBe(channel2);
    }

    [Fact]
    public void NotBeEqual_WhenLinksAreDifferent()
    {
        var channel1 = new CommunicationChannel(ChannelName, "test1@hogent.be", CommunicationTypes.Email);
        var channel2 = new CommunicationChannel(ChannelName, "test2@hogent.be", CommunicationTypes.Email);

        channel1.ShouldNotBe(channel2);
    }

    [Fact]
    public void NotBeEqual_WhenTypesAreDifferent()
    {
        var channel1 = new CommunicationChannel(ChannelName, Link, CommunicationTypes.Email);
        var channel2 = new CommunicationChannel(ChannelName, Link, CommunicationTypes.Phone);

        channel1.ShouldNotBe(channel2);
    }

    [Theory]
    [InlineData("Email", "student@hogent.be", CommunicationTypes.Email)]
    [InlineData("Telefoon", "09 123 45 67", CommunicationTypes.Phone)]
    [InlineData("Formulier", "https://www.hogent.be/contact", CommunicationTypes.Form)]
    [InlineData("Facebook", "https://facebook.com/hogent", CommunicationTypes.SocialMedia)]
    public void BeCreated_WithDifferentCommunicationTypes(string name, string link, CommunicationTypes type)
    {
        var channel = new CommunicationChannel(name, link, type);

        channel.ShouldNotBeNull();
        channel.Name.ShouldBe(name);
        channel.Link.ShouldBe(link);
        channel.TypeOfCommunication.ShouldBe(type);
    }

    [Fact]
    public void SupportAllCommunicationTypes()
    {
        var none = new CommunicationChannel("Test", "link", CommunicationTypes.None);
        none.TypeOfCommunication.ShouldBe(CommunicationTypes.None);

        var form = new CommunicationChannel("Test", "link", CommunicationTypes.Form);
        form.TypeOfCommunication.ShouldBe(CommunicationTypes.Form);

        var phone = new CommunicationChannel("Test", "link", CommunicationTypes.Phone);
        phone.TypeOfCommunication.ShouldBe(CommunicationTypes.Phone);

        var email = new CommunicationChannel("Test", "link", CommunicationTypes.Email);
        email.TypeOfCommunication.ShouldBe(CommunicationTypes.Email);

        var socialMedia = new CommunicationChannel("Test", "link", CommunicationTypes.SocialMedia);
        socialMedia.TypeOfCommunication.ShouldBe(CommunicationTypes.SocialMedia);
    }
}

