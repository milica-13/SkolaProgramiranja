using MaterialDesignThemes.Wpf;
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

    public partial class PromijeniLozinkuView : Window
    {
        private readonly Korisnik korisnik;
        private readonly SnackbarMessageQueue _messageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(3));

        public PromijeniLozinkuView(Korisnik korisnikZaPromjenu)
        {
            InitializeComponent();
            // Enter = Login
            this.Tag = new RoutedCommand();
            this.CommandBindings.Add(
                new CommandBinding((RoutedCommand)this.Tag, (s, e) => Sacuvaj_Click(s, null)));

            ThemeHelper.ApplyTheme(this);

            korisnik = korisnikZaPromjenu;

            MainSnackbar.MessageQueue = _messageQueue;
        }

        private void Sacuvaj_Click(object sender, RoutedEventArgs e)
        {
            string novaLozinka = txtNovaLozinka.Password.Trim();
            string potvrda = txtPotvrda.Password.Trim();

            if (string.IsNullOrEmpty(novaLozinka) || string.IsNullOrEmpty(potvrda))
            {
                _messageQueue.Enqueue(FindRes("Msg_EmptyPasswordFields"));
                return;
            }

            if (novaLozinka.Length < 6)
            {
                _messageQueue.Enqueue(FindRes("Msg_PasswordTooShort"));
                return;
            }

            if (novaLozinka != potvrda)
            {
                _messageQueue.Enqueue(FindRes("Msg_PasswordsMismatch"));
                return;
            }

            using (var context = new AppDbContext())
            {
                var korisnikIzBaze = context.Korisnici.Find(korisnik.Id);
                if (korisnikIzBaze != null)
                {
                    korisnikIzBaze.Lozinka = novaLozinka;
                    korisnikIzBaze.MoraPromijenitiLozinku = false;
                    context.SaveChanges();
                }
            }

            _messageQueue.Enqueue(FindRes("Msg_PasswordChanged"));
            DialogResult = true;
            Close();
        }

        private string FindRes(string key)
        {
            return Application.Current.TryFindResource(key)?.ToString() ?? key;
        }


        private void Otkazi_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void btnShow_Checked(object sender, RoutedEventArgs e)
        {
            txtNovaLozinka.Visibility = btnShow.IsChecked == true ? Visibility.Hidden : Visibility.Visible;

            if (btnShow.IsChecked == true)
            {
                var tb = new TextBox
                {
                    Text = txtNovaLozinka.Password,
                    Margin = txtNovaLozinka.Margin,
                    HorizontalAlignment = txtNovaLozinka.HorizontalAlignment
                };
                Grid parent = txtNovaLozinka.Parent as Grid;
                parent.Children.Add(tb);
            }
            else
            {
                var parent = txtNovaLozinka.Parent as Grid;
                var txt = parent.Children.OfType<TextBox>().FirstOrDefault();
                if (txt != null) parent.Children.Remove(txt);
            }
        }
    }
}
