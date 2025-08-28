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

    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            // Enter = Login
            this.Tag = new RoutedCommand();
            this.CommandBindings.Add(
                new CommandBinding((RoutedCommand)this.Tag, (s, e) => Login_Click(s, null)));

            ThemeHelper.ApplyTheme(this);
            //CommandBindings.Add(new CommandBinding(ApplicationCommands.Properties, (s, e) => Login_Click(s, null)));

            //DatabaseSeeder.Seed();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string lozinka = _passwordVisible ? tbLozinka.Text.Trim() : txtLozinka.Password.Trim();

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(lozinka))
            {
                MessageBox.Show(
                    (string)Application.Current.Resources["EnterEmailAndPassword"],
                    (string)Application.Current.Resources["Warning"],
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var context = new AppDbContext())
                {
                    // za svaki slučaj — ako nema podataka generiši seed
                    DatabaseSeeder.Seed();

                    var korisnik = context.Korisnici
                        .FirstOrDefault(k => k.Email == email && k.Lozinka == lozinka);

                    if (korisnik != null)
                    {
                        // prijava uspješna
                        switch (korisnik.Uloga)
                        {
                            case "Admin":
                                new AdminView().Show();
                                break;

                            case "Instruktor":
                                if (korisnik.MoraPromijenitiLozinku)
                                {
                                    var pl = new PromijeniLozinkuView(korisnik);
                                    if (pl.ShowDialog() != true)
                                        return;
                                }
                                new InstruktorView(korisnik).Show();
                                break;

                            default:
                                MessageBox.Show(
                                    (string)Application.Current.Resources["UnknownUserRole"],
                                    (string)Application.Current.Resources["Error"],
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                                return;
                        }

                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(
                            (string)Application.Current.Resources["InvalidCredentials"],
                            (string)Application.Current.Resources["Error"],
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format((string)Application.Current.Resources["DatabaseError"], ex.Message),
                    (string)Application.Current.Resources["Error"],
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }



        private bool _passwordVisible = false;

        private void btnShowPassword_Click(object sender, RoutedEventArgs e)
        {
            _passwordVisible = !_passwordVisible;

            if (_passwordVisible)
            {
                // Prikaži lozinku (kopiraj iz PasswordBox u TextBox)
                tbLozinka.Text = txtLozinka.Password;
                txtLozinka.Visibility = Visibility.Collapsed;
                tbLozinka.Visibility = Visibility.Visible;
                iconEye.Kind = MaterialDesignThemes.Wpf.PackIconKind.EyeOffOutline;
            }
            else
            {
                // Vrati se u PasswordBox
                txtLozinka.Password = tbLozinka.Text;
                tbLozinka.Visibility = Visibility.Collapsed;
                txtLozinka.Visibility = Visibility.Visible;
                iconEye.Kind = MaterialDesignThemes.Wpf.PackIconKind.EyeOutline;
            }
        }


    }
}
