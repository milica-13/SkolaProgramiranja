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
    public partial class UrediInstruktorProfilView : Window
    {
        private Korisnik instruktor;
        public UrediInstruktorProfilView(Korisnik instruktorZaEdit)
        {
            InitializeComponent();
            LanguageManager.ApplyCurrentLanguage();
            ThemeHelper.ApplyTheme(this);

            instruktor = instruktorZaEdit;

            txtIme.Text = instruktor.Ime;
            txtPrezime.Text = instruktor.Prezime;
            txtEmail.Text = instruktor.Email;
            txtTelefon.Text = instruktor.Telefon;

            txtObrazovanje.Text = instruktor.Obrazovanje;
            txtBiografija.Text = instruktor.Biografija;
        }

        private void Sacuvaj_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new AppDbContext())
            {
                var korisnik = context.Korisnici.Find(instruktor.Id);
                if (korisnik != null)
                {
                    korisnik.Ime = txtIme.Text.Trim();
                    korisnik.Prezime = txtPrezime.Text.Trim();
                    //korisnik.Email = txtEmail.Text.Trim();
                    korisnik.Telefon = txtTelefon.Text.Trim();
                    korisnik.Obrazovanje = txtObrazovanje.Text.Trim();
                    korisnik.Biografija = txtBiografija.Text.Trim();

                    if (!string.IsNullOrWhiteSpace(instruktor.SlikaProfilaPath))
                    {
                        korisnik.SlikaProfilaPath = instruktor.SlikaProfilaPath;
                    }

                    context.SaveChanges();
                    MessageBox.Show("Profil ažuriran!");
                    DialogResult = true;
                }
            }
        }


        private void Otkazi_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void OdaberiSliku_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Odaberi profilnu sliku",
                Filter = "Slike (*.png; *.jpg; *.jpeg)|*.png;*.jpg;*.jpeg|Svi fajlovi (*.*)|*.*"
            };

            if (dlg.ShowDialog() == true)
            {
                string folderPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "ProfilneSlike");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string fileName = System.IO.Path.GetFileName(dlg.FileName);
                string destinationPath = System.IO.Path.Combine(folderPath, fileName);

                if (!File.Exists(destinationPath))
                    File.Copy(dlg.FileName, destinationPath);

                // Prikaži sliku odmah
                var bitmap = new BitmapImage(new Uri(destinationPath, UriKind.Absolute));
                imgProfilna.Source = bitmap;

                // Sačuvaj relativnu putanju u instruktor objektu
                instruktor.SlikaProfilaPath = $@"Resources\ProfilneSlike\{fileName}";
            }
        }


        /*
        private void OdaberiSliku_Click(object sender, RoutedEventArgs e)
        {
             var dlg = new Microsoft.Win32.OpenFileDialog
             {
                   Title = "Odaberi profilnu sliku",   
                   Filter = "Slike (*.png; *.jpg) | *.png; *.jpg| Svi fajlovi (*.*)| *.*"
             };

              if(dlg.ShowDialog() == true)
              {
                   var img = new BitmapImage(new Uri(dlg.FileName));
                   imgProfilna.Source = img;

                   instruktor.SlikaProfilaPath = dlg.FileName; 
              }



        } 
        */
    }
}
