using MudBlazor;

namespace TrainTimetable.App.Components.Core.Layout;

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
            //TextPrimary = "1d7874",
            //TextSecondary = "679289"

            //1d7874
            //ee2e31
            //071e22
            //f4c095
            //679289
        };

        Typography = new Typography()
        {
            Default = new DefaultTypography
            {
               FontFamily = new[] { "Poppins", "sans-serif" }
            }
        };
    }
}
