using System.Text.Json;
using Microsoft.JSInterop;
using MudBlazor;
using MudBlazor.Utilities;
using Rise.Shared.StudentClubs;
using Rise.Shared.UserPreferences;

namespace Rise.Client.Theme;

public interface IThemingService
{
    MudTheme Theme { get; set;}
    bool IsDarkMode { get; set; }
    bool ImagesOff { get; set; }
    bool IsNeutral { get; set; }
    
    EventHandler<IThemingService> Subscribe { get; set; }

    void Initialize ();
    void ColorTheme(string mainColor);
}

public class ThemingService(IJSRuntime jsRuntime, IUserPreferenceService userPreferenceService) : IThemingService
{
    public MudTheme Theme
    {
        get => _theme;
        set
        {
            _theme = value;
            AsyncToLocalStorage( UserPreferenceKeys.Theme, value);
        }
    }
    
    private MudTheme _theme = DefaultTheme();
    public bool ImagesOff
    {
        get => _imagesOff;
        set
        {
            _imagesOff = value;
            AsyncToLocalStorage( UserPreferenceKeys.ImagesOff, value);
        }
    }

    private bool _imagesOff = true; // before updating the theme, the images are off to prevent network. 

    public bool IsNeutral
    {
        get => _isNeutral;
        set
        {
            _isNeutral = value;
            if (value)
            {
                Theme = DefaultTheme();
            }
            else
            {
                ColorTheme();
            }
            AsyncToLocalStorage(UserPreferenceKeys.IsNeutral, value);
        }
    }
    private bool _isNeutral = false;

    public bool IsDarkMode
    {
        get => _isDarkMode;
        set
        {
            _isDarkMode = value;
            AsyncToLocalStorage("darkmode", value);
        }
    }
    private bool _isDarkMode = false;
    
    public EventHandler<IThemingService> Subscribe { get; set; }
   
    
    public void Initialize()
    {
        LoadThemeFromStorage(UserPreferenceKeys.Theme, theme => Theme = JsonSerializer.Deserialize<MudTheme>(theme) ?? DefaultTheme());
        LoadThemeFromStorage("darkmode", darkmode => IsDarkMode = JsonSerializer.Deserialize<bool>(darkmode?.ToLower() ?? "false")); // does not exist on backend yet
        LoadThemeFromStorage(UserPreferenceKeys.ImagesOff, imagesOff => ImagesOff = JsonSerializer.Deserialize<bool>(imagesOff?.ToLower() ?? "false")); 
        LoadThemeFromStorage(UserPreferenceKeys.IsNeutral, neutral => IsNeutral = JsonSerializer.Deserialize<bool>(neutral?.ToLower() ?? "false"));
        // load theme from user preferences
    }

    /// <summary>
    /// Loads theme settings from local storage. If no value exists in local storage,
    /// fetches from user account preferences as fallback.
    /// </summary>
    /// <param name="key">The storage key to retrieve the theme setting.</param>
    /// <param name="setterCallback">Callback action to set the retrieved value.</param>
    private void LoadThemeFromStorage(string key, Action<string?> setterCallback)
    {
        jsRuntime.InvokeAsync<string?>("localStorage.getItem", key).AsTask()
            .ContinueWith(task =>
            {
                if (string.IsNullOrEmpty(task.Result))
                {
                    userPreferenceService.GetPreferencesAsync(CancellationToken.None)
                        .ContinueWith(backend => setterCallback(backend.Result.Value.UserPreferences.Settings[key].ToString()))
                        .CatchAndLog();
                } 
                else setterCallback(task.Result);
            });
    }
    
    public void ColorTheme(string mainColor = HoGentColors.Pantone7461U)
    {
        MudColor baseColor = new MudColor(mainColor);
        
        MudTheme lightTheme = DefaultTheme();
        lightTheme.PaletteLight.Primary = Colors.Blue.Default;
        lightTheme.PaletteLight.Secondary = Colors.Blue.Blue50;
        lightTheme.PaletteLight.Tertiary = Colors.Blue.Blue50;
        
        lightTheme.PaletteLight.AppbarBackground = Colors.Blue.Default; 
        lightTheme.PaletteLight.AppbarText = Colors.Blue.Default;

        lightTheme.PaletteDark.Primary = baseColor.ColorDarken(0.2);
        lightTheme.PaletteDark.Secondary = baseColor.ColorDarken(0.9);
        lightTheme.PaletteDark.Tertiary = baseColor.ColorLighten(0.2);

        lightTheme.PaletteDark.AppbarBackground = baseColor;
        lightTheme.PaletteDark.AppbarText = HoGentColors.Black;
        Theme = lightTheme;
    }
    
    // neutral theme
    // colorful theme

    private async void AsyncToLocalStorage(string key, object value)
    {
        Subscribe?.Invoke(this, this);
        try
        {
            string jsonValue = JsonSerializer.Serialize(value);
            await jsRuntime.InvokeVoidAsync("localStorage.setItem", key, jsonValue);
            // enable for hot-saving to backend - disabled in favor of settings page save button
            //await userPreferenceService.UpdateSinglePreferenceAsync(key, jsonValue, CancellationToken.None);
            
        }
        catch (Exception e)
        {
            // saving in the backend is not always possible. But that's ok.
            // Theme can be triggered locally before a logged in user has a full preference object.
            // Like upon reading device theme
            Console.WriteLine(e);
            throw;
        }
    }
    
    private static MudTheme DefaultTheme()
    {
        return new MudTheme()
        {
            PaletteLight = new PaletteLight()
            {
                Black = Colors.Shades.Black,
                White = Colors.Shades.White,
                Primary = Colors.Shades.Black,
                PrimaryContrastText = Colors.Shades.White,
                Secondary = Colors.Shades.White,
                SecondaryContrastText = Colors.Shades.White,
                Tertiary = Colors.Cyan.Default,
                TertiaryContrastText = Colors.Shades.White,
                Info = Colors.Shades.Black,
                InfoContrastText = Colors.Shades.White,
                Success = Colors.Green.Default,
                SuccessContrastText = Colors.Shades.Black,
                Warning = Colors.Orange.Default,
                WarningContrastText = Colors.Shades.Black,
                Error = Colors.Red.Default,
                ErrorContrastText = Colors.Shades.Black,
                Dark = Colors.Shades.Black,
                DarkContrastText = Colors.Shades.White,

                TextPrimary = Colors.Shades.Black,
                TextSecondary = Colors.Shades.White,
                TextDisabled = Colors.Gray.Default,

                ActionDefault = Colors.Shades.Black,
                ActionDisabled = Colors.Gray.Gray50,
                ActionDisabledBackground = Colors.Gray.Gray30,

                Background = Colors.Shades.White,
                BackgroundGray = Colors.Gray.Gray50,
                Surface = Colors.Shades.White,

                DrawerBackground = Colors.Shades.White,
                DrawerText = Colors.Shades.Black,
                DrawerIcon = Colors.Shades.Black,

                AppbarBackground = Colors.Shades.Black,
                AppbarText = Colors.Shades.White,

                LinesDefault = Colors.Gray.Gray15,
                LinesInputs = Colors.Shades.Black,
                TableLines = Colors.Gray.Gray50,
                TableStriped = Colors.Gray.Gray30,
                TableHover = Colors.Cyan.Default,

                Divider = Colors.Gray.Gray50,
                DividerLight = Colors.Gray.Gray15,

                Skeleton = Colors.Gray.Gray30,

                PrimaryDarken = Colors.Shades.Black50,
                PrimaryLighten = Colors.Gray.Gray15,
                SecondaryDarken = Colors.Shades.Black,
                SecondaryLighten = Colors.Gray.Gray15,
                TertiaryDarken = Colors.Cyan.Cyan50,
                TertiaryLighten = Colors.Cyan.Cyan30,
                InfoDarken = Colors.Shades.Black50,
                InfoLighten = Colors.Blue.Blue30,
                SuccessDarken = Colors.Green.Green50,
                SuccessLighten = Colors.Green.Green30,
                WarningDarken = Colors.Orange.Orange50,
                WarningLighten = Colors.Orange.Orange30,
                ErrorDarken = Colors.Red.Red50,
                ErrorLighten = Colors.Red.Red30,
                DarkDarken = Colors.Shades.Black50,
                DarkLighten = Colors.Shades.Black30,
                
                BorderOpacity = 1.0,
                HoverOpacity = 0.16,
                RippleOpacity = 0.1,
                RippleOpacitySecondary = 0.2,

                GrayDefault = Colors.Gray.Default,
                GrayLight = Colors.Gray.Gray50,
                GrayLighter = Colors.Gray.Gray30,
                GrayDark = Colors.Shades.Black30,
                GrayDarker = Colors.Shades.Black50,

                OverlayDark = "rgba(0,0,0,0.5)",
                OverlayLight = "rgba(255,255,255,0.5)"
            },
            PaletteDark = new PaletteDark()
            {
                Primary = Colors.Shades.Black,
                Secondary = Colors.Shades.Black,
                Tertiary = Colors.Shades.Black,
                AppbarBackground = Colors.Shades.Black
            },

            LayoutProperties = new LayoutProperties()
            {
                DrawerWidthLeft = "260px",
                DrawerWidthRight = "300px",
                DrawerMiniWidthLeft = "56px",
                DrawerMiniWidthRight = "56px"
            },
            
            Typography = new Typography()
            {
                Default = new DefaultTypography()
                {
                    FontFamily = new[] { "Montserrat", "Arial", "sans-serif" },
                    LetterSpacing = "0em"
                },
                
                H1 = new H1Typography
                {
                    FontWeight = "800"
                },
                
                H2 = new H2Typography
                {
                    FontSize = "36px",
                    FontWeight = "600",
                    LineHeight = "1.25",
                },
                
                H3 = new H3Typography
                {
                    FontSize = "28px",
                    FontWeight = "600",
                    LineHeight = "1.3",
                },
                
                H4 = new H4Typography
                {
                    FontSize = "22px",
                    FontWeight = "600",
                    LineHeight = "1.4",
                },
                
                Body1 = new Body1Typography
                {
                    FontWeight = "400",
                    FontSize = "18px",
                    LineHeight = "1.5",
                },
                
                Button = new ButtonTypography()
                {
                    FontWeight = "800"
                },
                
                Caption = new CaptionTypography
                {
                    FontSize = "12px",
                    LineHeight = "1.4",
                    LetterSpacing = "0.02em"
                }
            }
        };
    }
}