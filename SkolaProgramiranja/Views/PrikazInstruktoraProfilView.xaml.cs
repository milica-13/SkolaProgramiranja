using SkolaProgramiranja.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SkolaProgramiranja.Views
{
    public partial class PrikazInstruktoraProfilView : Window
    {
        private Korisnik instruktor;
        public PrikazInstruktoraProfilView(Korisnik instruktor)
        {
            InitializeComponent();
            LanguageManager.ApplyCurrentLanguage();
            ThemeHelper.ApplyTheme(this);

            this.instruktor = instruktor;
            DataContext = instruktor;

            PrikaziProfilnuSliku();
        }

        private void Zatvori_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }

        private void PrikaziProfilnuSliku()
        {
            string fullPath = instruktor.SlikaProfilaFullPath;

            if (string.IsNullOrWhiteSpace(fullPath) || !File.Exists(fullPath))
            {
                fullPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "avatar_placeholder.png");
            }

            if (File.Exists(fullPath))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(fullPath, UriKind.Absolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                imgProfilna.Source = bitmap;
            }
        }
    }


    
}
