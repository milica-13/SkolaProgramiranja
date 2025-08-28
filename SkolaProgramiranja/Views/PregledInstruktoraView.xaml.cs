using SkolaProgramiranja.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using SkolaProgramiranja.Models;

namespace SkolaProgramiranja.Views
{
    public partial class PregledInstruktoraView : Window
    {
       
        public PregledInstruktoraView()
        {
            InitializeComponent();
            ThemeHelper.ApplyTheme(this);

            UcitajInstruktore();

        }
                

        private void UcitajInstruktore()
        {
            using(var context = new AppDbContext())
            {
                var instruktori = context.Korisnici
                    .Where(k => k.Uloga == "Instruktor")
                    .ToList();
                instruktoriList.ItemsSource = instruktori;
            }
        }

        private void Nazad_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            //var forma = new AdminView();
            //forma.ShowDialog();
            
        }

        private void Detalji_Click(object sender, RoutedEventArgs e)
        {
            var dugme = sender as Button;
            var selektovani = dugme?.DataContext as Korisnik;

            if (selektovani != null)
            {
                var forma = new PrikazInstruktoraProfilView(selektovani);
                forma.ShowDialog();
            }
            /*
            var button = sender as Button;
            var instruktor = button?.DataContext as Korisnik;

            if (instruktor != null)
            {
                var view = new ProfilInstruktoraView(instruktor);
                view.ShowDialog();
            }
            */
        }

    }
}
