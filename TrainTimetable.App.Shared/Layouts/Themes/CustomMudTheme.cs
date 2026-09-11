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
