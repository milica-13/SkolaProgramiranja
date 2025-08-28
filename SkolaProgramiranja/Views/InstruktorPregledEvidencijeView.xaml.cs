using SkolaProgramiranja.Models;
using SkolaProgramiranja.Views;
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
    public partial class InstruktorPregledEvidencijeView : Window
    {
        private readonly Korisnik instruktor;
        private List<Kurs> kursevi;
        private List<EvidencijaCasa> evidencije;

        public InstruktorPregledEvidencijeView(Korisnik instruktor)
        {
            InitializeComponent();
            ThemeHelper.ApplyTheme(this);
            this.instruktor = instruktor;

            using (var context = new AppDbContext())
            {
                kursevi = context.Kursevi.Where(k => k.InstruktorId == instruktor.Id).ToList();
                cbKursevi.ItemsSource = kursevi;
                cbKursevi.DisplayMemberPath = "Naziv";
                cbKursevi.SelectedIndex = 0;

                evidencije = context.EvidencijeCasa
                                    .Where(e => e.InstruktorId == instruktor.Id)
                                    .ToList();

                foreach (var e in evidencije)
                {
                    var kurs = kursevi.FirstOrDefault(k => k.Id == e.KursId);
                    e.KursNaziv = kurs?.Naziv ?? "Nepoznato";

                    var prisLista = context.Prisustva
                                           .Where(p => p.EvidencijaCasaId == e.Id && p.Prisutan)
                                           .Select(p => p.Ucenik.Ime + " " + p.Ucenik.Prezime)
                                           .ToList();
                    e.PrisutniUcenici = string.Join(", ", prisLista);
                }

                // VAŽNO: očisti Items prije ItemsSource
                dgEvidencije.ItemsSource = null;
                dgEvidencije.Items.Clear();

                dgEvidencije.ItemsSource = evidencije;

                var empty = evidencije.Count == 0;
                lblStatus.Text = empty ? "Nema dostupnih evidencija." : "";
                lblStatus.Visibility = empty ? Visibility.Visible : Visibility.Collapsed;
                dgEvidencije.Visibility = empty ? Visibility.Collapsed : Visibility.Visible;
            }
        }


        private void Filtriraj_Click(object sender, RoutedEventArgs e)
        {
            var filtrirane = evidencije.AsEnumerable();

            if (cbKursevi.SelectedItem is Kurs odabrani)
                filtrirane = filtrirane.Where(ev => ev.KursId == odabrani.Id);

            if (dpDatum.SelectedDate.HasValue)
                filtrirane = filtrirane.Where(ev => ev.Datum.Date == dpDatum.SelectedDate.Value.Date);

            var lista = filtrirane.ToList();

            // očisti prije nove dodjele
            dgEvidencije.ItemsSource = null;
            dgEvidencije.Items.Clear();

            dgEvidencije.ItemsSource = lista;

            var empty = lista.Count == 0;
            lblStatus.Text = empty ? "Nema dostupnih evidencija." : "";
            lblStatus.Visibility = empty ? Visibility.Visible : Visibility.Collapsed;
            dgEvidencije.Visibility = empty ? Visibility.Collapsed : Visibility.Visible;
        }


        private void Uredi_Click(object sender, RoutedEventArgs e)
        {
            var evidencija = (sender as FrameworkElement)?.DataContext as EvidencijaCasa;
            if (evidencija != null)
            {
                var forma = new EvidencijaCasaView(instruktor, evidencija);
                forma.ShowDialog();
                this.Close(); 
            }
        }

        private void Exportuj_Click(object sender, RoutedEventArgs e)
        {
            if (dgEvidencije.SelectedItem is not EvidencijaCasa evidencija)
            {
                MessageBox.Show("Odaberite evidenciju koju želite eksportovati.");
                return;
            }

            var saveDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Text file (*.txt)|*.txt",
                FileName = $"Evidencija_{evidencija.Datum:yyyyMMdd}.txt"
            };

            if (saveDialog.ShowDialog() == true)
            {
                var tekst = $"Datum: {evidencija.Datum:dd.MM.yyyy}\n" +
                            $"Kurs: {evidencija.KursNaziv}\n" +
                            $"Tema: {evidencija.TemaCasa}\n" +
                            $"Opis: {evidencija.Opis}\n" +
                            $"Trajanje: {evidencija.TrajanjeMin} minuta\n" +
                            $"Prisutni: {evidencija.PrisutniUcenici}";

                File.WriteAllText(saveDialog.FileName, tekst);

                MessageBox.Show("Evidencija sačuvana.");
            }
        }

        private void Obrisi_Click(object sender, RoutedEventArgs e)
        {
            var evidencija = (sender as FrameworkElement)?.DataContext as EvidencijaCasa;
            if (evidencija != null)
            {
                if (MessageBox.Show("Jeste li sigurni da želite obrisati?", "Potvrda", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    using (var context = new AppDbContext())
                    {
                        var zaBrisanje = context.EvidencijeCasa.Find(evidencija.Id);
                        context.EvidencijeCasa.Remove(zaBrisanje);
                        context.SaveChanges();
                        MessageBox.Show("Evidencija obrisana.");
                        this.Close();
                        //new PrikazEvidencijeView(instruktor).ShowDialog(); // reload
                    }
                }
            }
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
        private void DarkTheme_Checked(object sender, RoutedEventArgs e)
        {
            ThemeHelper.ApplyTheme(this);
        }

        private void DarkTheme_Unchecked(object sender, RoutedEventArgs e)
        {
            ThemeHelper.ApplyTheme(this);
        }
    }
}
