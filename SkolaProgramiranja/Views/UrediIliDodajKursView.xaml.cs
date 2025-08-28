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

    public partial class UrediIliDodajKursView : Window
    {
        private Kurs kursZaUredjivanje;

     
        public UrediIliDodajKursView()
        {
            InitializeComponent();
            LanguageManager.ApplyCurrentLanguage();
            ThemeHelper.ApplyTheme(this);


            using (var context = new AppDbContext())
            {
                cbInstruktori.ItemsSource = context.Korisnici
                    .Where(k => k.Uloga == "Instruktor")
                    .ToList();
            }
        }

        public UrediIliDodajKursView(Kurs kurs) : this()
        {
            kursZaUredjivanje = kurs;

            txtNaziv.Text = kurs.Naziv;
            txtOpis.Text = kurs.Opis;
            cbInstruktori.SelectedValue = kurs.InstruktorId;
        }

        private void Sacuvaj_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNaziv.Text))
            {
                MessageBox.Show("Unesite naziv kursa.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtNaziv.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtOpis.Text))
            {
                MessageBox.Show("Unesite opis kursa.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtOpis.Focus();
                return;
            }
            if (cbInstruktori.SelectedValue == null)
            {
                MessageBox.Show("Odaberite instruktora.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                cbInstruktori.IsDropDownOpen = true;
                return;
            }

            int instruktorId;
            try
            {
                instruktorId = Convert.ToInt32(cbInstruktori.SelectedValue);
            }
            catch
            {
                MessageBox.Show("Neispravan odabir instruktora.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var context = new AppDbContext())
            {
                if (kursZaUredjivanje == null)
                {
                    var novi = new Kurs
                    {
                        Naziv = txtNaziv.Text.Trim(),
                        Opis = txtOpis.Text.Trim(),
                        InstruktorId = instruktorId
                    };
                    context.Kursevi.Add(novi);
                }
                else
                {
                    var postojeci = context.Kursevi.Find(kursZaUredjivanje.Id);
                    if (postojeci != null)
                    {
                        postojeci.Naziv = txtNaziv.Text.Trim();
                        postojeci.Opis = txtOpis.Text.Trim();
                        postojeci.InstruktorId = instruktorId;
                    }
                }

                context.SaveChanges();
            }

            DialogResult = true;
            Close();
        }


        private void Otkazi_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }

}
