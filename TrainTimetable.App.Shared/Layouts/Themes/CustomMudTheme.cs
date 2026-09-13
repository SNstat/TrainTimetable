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
            Primary = "#236a66",
            Secondary = "#78618E",
            AppbarBackground = "#1f2937",
            Background = "#111827",
            Surface = "#1f2937",
            Tertiary = "#1f2937",
            DrawerBackground = "#1f2937"
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
