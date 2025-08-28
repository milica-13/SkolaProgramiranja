using Microsoft.EntityFrameworkCore;
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
using Microsoft.EntityFrameworkCore;
using MaterialDesignThemes.Wpf;
using System.ComponentModel;
using System.Windows.Data;


namespace SkolaProgramiranja.Views

{
    public partial class AdminView : Window
    {
        private readonly PaletteHelper _paletteHelper = new PaletteHelper();
        private ICollectionView _view;
        public AdminView()
        {
            InitializeComponent();
            ThemeHelper.ApplyTheme(this);
            UcitajKurseve();
            ApplySavedTheme();
            ApplySidebarStyle(Properties.Settings.Default.Theme == "Dark");
            _view = CollectionViewSource.GetDefaultView(dgKursevi.ItemsSource);
            _view.Filter = KursFilter;
        }

        private void UcitajKurseve()
        {
            using(var context=new AppDbContext())
            {
                //dgKursevi.ItemsSource = context.Kursevi.ToList();
                dgKursevi.ItemsSource = context.Kursevi
                    .Include(k => k.Instruktor)
                    .ToList();
            
            }
        }

        private void Dodaj_Click(object sender, RoutedEventArgs e)
        {
            var forma = new UrediIliDodajKursView();
            if (forma.ShowDialog() == true)
            {
                UcitajKurseve();
                MessageBox.Show((string)Application.Current.Resources["CourseAdded"]);

            }
        }

        private void Uredi_Click(object sender, RoutedEventArgs e)
        {
            var selektovani = dgKursevi.SelectedItem as Kurs;
            if (selektovani != null)
            {
               
                var forma = new UrediIliDodajKursView(selektovani);
                if (forma.ShowDialog() == true)
                {
                    UcitajKurseve();
                    MessageBox.Show((string)Application.Current.Resources["CourseUpdated"]);
                }
                    
            }
        }

        private void Obrisi_Click(object sender, RoutedEventArgs e)
        {
            var selektovani = dgKursevi.SelectedItem as Kurs;
            if (selektovani != null)
            {
                var poruka = (string)Application.Current.Resources["Msg_ConfirmDelete"];
                var naslov = (string)Application.Current.Resources["Msg_DeleteTitle"];

                if (MessageBox.Show(poruka, naslov, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    using (var context = new AppDbContext())
                    {
                        context.Kursevi.Remove(selektovani);
                        context.SaveChanges();
                        UcitajKurseve();

                        var uspjesnoObrisano = (string)Application.Current.Resources["CourseDeleted"];
                        MessageBox.Show(uspjesnoObrisano);
                    }
                }
            }
        }


        private void DodajInstruktora_Click(object sender, RoutedEventArgs e)
        {
            var forma = new UrediIliDodajInstruktoraView();
            forma.ShowDialog();
        }

 
       

        private void PregledajInstruktore_Click(object sender, RoutedEventArgs e)
        {
            var forma = new PregledInstruktoraView();
            forma.ShowDialog();

        }

        private void Odjava_Click(object sender, RoutedEventArgs e)
        {
            var login = new LoginView();
            login.Show();
            this.Close();

        }

        private void TemaSvijetla_Click(object sender, RoutedEventArgs e)
        {
            ThemeHelper.SaveTheme("Light");
            ThemeHelper.ApplyTheme(this);
        }

        private void TemaTamna_Click(object sender, RoutedEventArgs e)
        {
            ThemeHelper.SaveTheme("Dark");
            ThemeHelper.ApplyTheme(this);
        }

        private void TemaLjubičasta_Click(object sender, RoutedEventArgs e)
        {
            ThemeHelper.SaveTheme("Purple");
            ThemeHelper.ApplyTheme(this);
        }





        private void ApplySavedTheme()
        {
            var theme = _paletteHelper.GetTheme();
            string savedTheme = Properties.Settings.Default.Theme;

            if (savedTheme == "Dark")
            {
                theme.SetBaseTheme(BaseTheme.Dark);
            }
            else
            {
                theme.SetBaseTheme(BaseTheme.Light);
            }

            _paletteHelper.SetTheme(theme);
        }


        private void ApplySidebarStyle(bool darkMode)
        {
            var sidebar = (StackPanel)LogicalTreeHelper.FindLogicalNode(this, "SidebarPanel");
            var elements = sidebar.Children.OfType<Button>().ToList();

            if (darkMode)
            {
                sidebar.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7E57C2"));
                foreach (var btn in elements)
                {
                    btn.Foreground = Brushes.White;
                    ((PackIcon)((StackPanel)btn.Content).Children[0]).Foreground = Brushes.White;
                }
            }
            else
            {
                sidebar.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CBC3E3")); // light purple
                foreach (var btn in elements)
                {
                    btn.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3E3E3E")); // tamna
                    ((PackIcon)((StackPanel)btn.Content).Children[0]).Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#512DA8")); 
                }
            }
        }

        private void PromijeniJezikSrpski_Click(object sender, RoutedEventArgs e)
        {
            LanguageManager.ChangeLanguage("sr");
            RestartujView();
        }

        private void PromijeniJezikEngleski_Click(object sender, RoutedEventArgs e)
        {
            LanguageManager.ChangeLanguage("en");
            RestartujView();
        }
        private void RestartujView()
        {
            var novi = new AdminView();
            novi.Show();
            this.Close();
        }

        private bool KursFilter(object obj)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text)) return true;
            var q = txtSearch.Text.Trim().ToLowerInvariant();
            if (obj is Kurs k)
                return (k.Naziv?.ToLowerInvariant().Contains(q) ?? false)
                    || (k.InstruktorFullName?.ToLowerInvariant().Contains(q) ?? false);
            return true;
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            _view?.Refresh();
        }

        private void DataGridRow_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGridRow row && row.Item is Kurs selektovani)
            {
                var dlg = new UrediIliDodajKursView(selektovani)
                {
                    Owner = this
                };

                var ok = dlg.ShowDialog() == true;
                if (ok)
                {
                    OsvjeziKurseveIZadrziSelektovani(selektovani.Id);
                }

                e.Handled = true; 
            }
        }

        private void OsvjeziKurseveIZadrziSelektovani(int idZaZadrzati)
        {
            using (var context = new AppDbContext())
            {
                
                var lista = context.Kursevi.ToList();

                dgKursevi.ItemsSource = lista;

                var ponovo = lista.FirstOrDefault(k => k.Id == idZaZadrzati);
                if (ponovo != null)
                    dgKursevi.SelectedItem = ponovo;
            }
        }

    }
}
