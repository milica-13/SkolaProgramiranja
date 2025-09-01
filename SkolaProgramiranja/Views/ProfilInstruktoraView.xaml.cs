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
    public partial class ProfilInstruktoraView : Window
    {
        private Korisnik instruktor;
        public ProfilInstruktoraView(Korisnik instruktorZaEdit)
        {
            InitializeComponent();
            ThemeHelper.ApplyTheme(this);

            this.instruktor = instruktorZaEdit;
            DataContext = instruktorZaEdit;
        }

        private void UrediProfil_Click(object sender, RoutedEventArgs e)
        {
            var view = new UrediInstruktorProfilView(instruktor);
            if(view.ShowDialog() == true)
            {
                DataContext = null;
                DataContext = instruktor;
            }
        }

        private void OdaberiSliku_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Odaberi profilnu sliku",
                Filter = "Slike (*.png; *.jpg)|*.png;*.jpg|Svi fajlovi (*.*)|*.*"
            };

            if (dlg.ShowDialog() == true)
            {
                string destFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
                string fileName = System.IO.Path.GetFileName(dlg.FileName);
                string destPath = System.IO.Path.Combine(destFolder, fileName);

                if (!Directory.Exists(destFolder))
                    Directory.CreateDirectory(destFolder);

                if (!File.Exists(destPath))
                    File.Copy(dlg.FileName, destPath);

              
                instruktor.SlikaProfilaPath = $"/Resources/{fileName}";

            }
        }

        private void Otkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }

        private void Sacuvaj_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new AppDbContext())
            {
                var korisnik = context.Korisnici.Find(instruktor.Id);
                if(korisnik != null)
                {
                    korisnik.Ime = txtIme.Text.Trim();
                    korisnik.Prezime = txtPrezime.Text.Trim();
                    korisnik.Email = txtEmail.Text.Trim();
                    korisnik.Telefon = txtTelefon.Text.Trim();
                    korisnik.Obrazovanje = txtObrazovanje.Text.Trim();
                    korisnik.Biografija = txtBiografija.Text.Trim();

                   

                    korisnik.SlikaProfilaPath = instruktor.SlikaProfilaPath;

                    context.SaveChanges();
                    MessageBox.Show("Profil ažuriran!");

                    // Osvježi prikaz
                    DataContext = null;
                    DataContext = instruktor;
                }
                    
            }
        }

        private void Zatvori_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
