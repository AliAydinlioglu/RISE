using MudBlazor;

namespace Rise.Client.Theme.Fakers;

public class FakeThemingService : IThemingService
{
    private MudTheme _theme = new();
    public MudTheme Theme
    {
        get => _theme;
        set
        {
            _theme = value;
            NotifyStateChanged();
        }
    }

    private bool _isDarkMode;
    public bool IsDarkMode
    {
        get => _isDarkMode;
        set
        {
            _isDarkMode = value;
            NotifyStateChanged();
        }
    }

    private bool _imagesOff;
    public bool ImagesOff
    {
        get => _imagesOff;
        set
        {
            _imagesOff = value;
            NotifyStateChanged();
        }
    }

    private bool _isNeutral;
    public bool IsNeutral
    {
        get => _isNeutral;
        set
        {
            _isNeutral = value;
            NotifyStateChanged();
        }
    }

    public EventHandler<IThemingService> Subscribe { get; set; }

    public void Initialize()
    {
        // Intentionally left empty for testing
    }

    public void ColorTheme(string mainColor)
    {
        // Intentionally left empty for testing
    }

    private void NotifyStateChanged()
    {
        Subscribe?.Invoke(this, this);
    }
}
