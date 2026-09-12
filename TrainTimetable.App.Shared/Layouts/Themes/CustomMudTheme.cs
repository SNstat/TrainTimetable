using MudBlazor;

namespace TrainTimetable.App.Shared.Layouts.Themes;

public class CustomMudTheme : MudTheme
{
    public CustomMudTheme()
    {
        PaletteLight = new PaletteLight()
        {
            Primary = "#1d7874",
            Secondary = "#ee2e31",
            AppbarBackground = "#071e22",
            Background = "#eeeeee",
            Surface = "#ffffff",
            Tertiary = "#071e22"
        };

        PaletteDark = new PaletteDark()
        {
            Primary = "#1d7874",
            Secondary = "#AF2364",
            AppbarBackground = "#1E384E",
            Background = "#122333",
            Surface = "#182B3C",
            Tertiary = "#1E384E",
            DrawerBackground = "#182B3C"
        };

        Typography = new Typography()
        {
            Default = new DefaultTypography
            {
                FontFamily = ["Poppins", "sans-serif"]
            }
        };
    }
}
