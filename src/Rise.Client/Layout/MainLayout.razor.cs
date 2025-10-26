using MudBlazor;

namespace Rise.Client.Layout;

public partial class MainLayout
{
    
    MudTheme MyCustomTheme = new MudTheme()
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
            BackgroundGray = Colors.Gray.Default,
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
            Primary = Colors.Blue.Blue30
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
                FontFamily = new[] { "Montserrat", "Arial", "sans-serif" }
            },
            
            H1 = new H1Typography()
            {
                FontWeight = "800"
            },
            
            Button = new ButtonTypography()
            {
                FontWeight = "800"
            }
        }
    };
}