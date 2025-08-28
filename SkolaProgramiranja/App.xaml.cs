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
            //using (var context = new AppDbContext())
            //{
            // context.Database.Migrate();
            //}
            /*
            base.OnStartup(e);
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
            var paletteHelper = new PaletteHelper();

            LanguageManager.ApplyCurrentLanguage(); 

            Theme theme = paletteHelper.GetTheme();

            //if (Properties.Settings.Default.Theme == "Dark")
            //   theme.SetBaseTheme(Theme.Dark);
            // else
            //    theme.SetBaseTheme(Theme.Light);
            //
            paletteHelper.SetTheme(theme);
            using var ctx = new AppDbContext();
            //DbSeeder.EnsureStudentsAndEnrollments(ctx, minPerCourse: 15);
            var mainWindow = new MainWindow();
            mainWindow.Show();
            */

            //NOVA VERZIJA
            base.OnStartup(e);

            // smanji binding noise u Output prozoru
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level =
                System.Diagnostics.SourceLevels.Critical;

            // 1) migracije + seed (jednom na startu)
            using (var context = new AppDbContext())
            {
                context.Database.Migrate();
                DatabaseSeeder.Seed();
            }

            // 2) primijeni jezik i temu
            LanguageManager.ApplyCurrentLanguage();
            var paletteHelper = new PaletteHelper();
            paletteHelper.SetTheme(paletteHelper.GetTheme());

            // 3) pokreni glavni prozor
            var main = new MainWindow();
            main.Show();
        }
    }



}
