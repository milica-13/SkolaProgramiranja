using System.Windows;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;
using MaterialDesignColors;


namespace SkolaProgramiranja.Models
{
    public static class ThemeHelper
    {
        public static void ApplyTheme(Window window)
        {
            var paletteHelper = new PaletteHelper();
            Theme theme;

            switch (Properties.Settings.Default.Theme)
            {
                case "Dark":
                    theme = Theme.Create(BaseTheme.Dark,
                        SwatchHelper.Lookup[MaterialDesignColor.DeepPurple],
                        SwatchHelper.Lookup[MaterialDesignColor.Lime]);
                    break;

                case "Purple":
                    var primary = (Color)ColorConverter.ConvertFromString("#7E57C2"); // ljubičasta
                    var secondary = (Color)ColorConverter.ConvertFromString("#B39DDB"); // svijetlo ljubičasta
                    theme = Theme.Create(BaseTheme.Light, primary, secondary);
                    break;


                default:
                    theme = Theme.Create(BaseTheme.Light,
                        SwatchHelper.Lookup[MaterialDesignColor.Indigo],
                        SwatchHelper.Lookup[MaterialDesignColor.Orange]);
                    break;
            }

            paletteHelper.SetTheme(theme);
        }

        public static void SaveTheme(string theme)
        {
            Properties.Settings.Default.Theme = theme;
            Properties.Settings.Default.Save();
        }
    }
}
