using SkolaProgramiranja.Models;
using System.Configuration;
using System.Data;
using System.Windows;
using MaterialDesignThemes.Wpf;
using SkolaProgramiranja.Views;
using Microsoft.EntityFrameworkCore;


namespace SkolaProgramiranja
{

    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level =
                System.Diagnostics.SourceLevels.Critical;

            using (var context = new AppDbContext())
            {
                context.Database.Migrate();
                DatabaseSeeder.Seed();
            }

            LanguageManager.ApplyCurrentLanguage();
            var paletteHelper = new PaletteHelper();
            paletteHelper.SetTheme(paletteHelper.GetTheme());

            var main = new MainWindow();
            main.Show();
        }
    }



}
