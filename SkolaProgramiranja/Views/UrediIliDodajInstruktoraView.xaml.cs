using SkolaProgramiranja.Models;
using System;
using System.Collections.Generic;
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
    public partial class UrediIliDodajInstruktoraView : Window
    {
        public UrediIliDodajInstruktoraView()
        {
            InitializeComponent();
            ThemeHelper.ApplyTheme(this);

        }

        private void Sacuvaj_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIme.Text))
            {
                MessageBox.Show("Unesite ime.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPrezime.Text))
            {
                MessageBox.Show("Unesite prezime.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !txtEmail.Text.Contains("@"))
            {
                MessageBox.Show("Unesite ispravan email.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtLozinka.Password) || txtLozinka.Password.Length < 6)
            {
                MessageBox.Show("Lozinka mora imati barem 6 karaktera.");
                return;
            }

            using (var context = new AppDbContext())
            {
                var novi = new Korisnik
                {
                    Ime = txtIme.Text,
                    Prezime = txtPrezime.Text,
                    Email = txtEmail.Text,
                    Lozinka = txtLozinka.Password,
                    Uloga = "Instruktor",
                    Obrazovanje = txtObrazovanje.Text,
                    Biografija = txtBiografija.Text
                };

                context.Korisnici.Add(novi);
                context.SaveChanges();
            }

            DialogResult = true;
            Close();
        }
        private bool _passwordVisible = false;
        private void btnShowPassword_Click(object sender, RoutedEventArgs e)
        {
            _passwordVisible = !_passwordVisible;

            if (_passwordVisible)
            {
                tbLozinka.Text = txtLozinka.Password;
                txtLozinka.Visibility = Visibility.Collapsed;
                tbLozinka.Visibility = Visibility.Visible;
                iconEye.Kind = MaterialDesignThemes.Wpf.PackIconKind.EyeOffOutline;
            }
            else
            {
                txtLozinka.Password = tbLozinka.Text;
                tbLozinka.Visibility = Visibility.Collapsed;
                txtLozinka.Visibility = Visibility.Visible;
                iconEye.Kind = MaterialDesignThemes.Wpf.PackIconKind.EyeOutline;
            }
        }
        private void Otkazi_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }

}
