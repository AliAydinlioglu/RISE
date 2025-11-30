using Rise.Client.Components;
using Rise.Client.Faker;
using Rise.Client.Layout;
using Shouldly;

namespace Rise.Client.Offline;

public class GivenAnOfflineBanner : MudBlazorTestSetup, IDisposable
{
    private readonly FakeConnectionService _fakeConnectionService;

    public GivenAnOfflineBanner()
    {
        _fakeConnectionService = new FakeConnectionService();
        Services.AddSingleton<IConnectionService>(_fakeConnectionService);
    }

    [Fact(DisplayName = "When online, then banner should not be rendered")]
    public void Render_WhenOnline_ShouldNotRenderBanner()
    {
        _fakeConnectionService.SetOnline(true);

        var cut = RenderComponent<OfflineBanner>();

        cut.Markup.ShouldBeEmpty();
    }

    [Fact(DisplayName = "When offline, then banner should be rendered")]
    public void Render_WhenOffline_ShouldRenderBanner()
    {
        _fakeConnectionService.SetOnline(false);

        var cut = RenderComponent<OfflineBanner>();

        cut.Markup.ShouldNotBeEmpty();
    }

    [Fact(DisplayName = "When connection goes offline, then banner should appear")]
    public void ConnectionStateChanged_WhenGoingOffline_ShouldShowBanner()
    {
        _fakeConnectionService.SetOnline(true);
        var cut = RenderComponent<OfflineBanner>();
        cut.Markup.ShouldBeEmpty();

        _fakeConnectionService.SimulateConnectionChange(false);

        cut.Markup.ShouldNotBeEmpty();
    }

    [Fact(DisplayName = "When connection goes online, then banner should disappear")]
    public void ConnectionStateChanged_WhenGoingOnline_ShouldHideBanner()
    {
        _fakeConnectionService.SetOnline(false);
        var cut = RenderComponent<OfflineBanner>();
        cut.Markup.ShouldNotBeEmpty();

        _fakeConnectionService.SimulateConnectionChange(true);

        cut.Markup.ShouldBeEmpty();
    }

    [Fact(DisplayName = "When initialized, then InitializeAsync should be called")]
    public void OnInitialized_ShouldCallInitializeAsync()
    {
        var cut = RenderComponent<OfflineBanner>();

        _fakeConnectionService.InitializeAsyncCallCount.ShouldBe(1);
        _fakeConnectionService.SubscriberCount.ShouldBe(1);
    }
    
    [Fact(DisplayName = "When offline, then banner should display correct title, subtitle and severity")]
    public void Render_WhenOffline_ShouldDisplayCorrectTitle()
    {
        _fakeConnectionService.SetOnline(false);

        var cut = RenderComponent<OfflineBanner>();

        cut.Markup.ShouldContain("Geen internetverbinding");
        cut.Markup.ShouldContain("De gegevens die u ziet zijn mogelijks verouderd.");
        cut.Markup.ShouldContain("Warning");
    }

    public new void Dispose()
    {
        _fakeConnectionService.Reset();
    }
}