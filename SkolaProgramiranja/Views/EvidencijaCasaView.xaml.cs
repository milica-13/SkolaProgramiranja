using SkolaProgramiranja.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SkolaProgramiranja.Views
{
    public partial class EvidencijaCasaView : Window
    {
        private readonly Korisnik instruktor;
        private readonly EvidencijaCasa evidencijaZaUređivanje;
        private List<Kurs> kursevi;

        private class UcenikPrisustvo {
            public int UcenikId { get; set; }
            public string Ime { get; set; } 
            public string Prezime { get; set; }
            public bool Prisutan { get; set; }
        }

        public EvidencijaCasaView(Korisnik instruktor)
        {
            InitializeComponent();
            ThemeHelper.ApplyTheme(this);

            this.instruktor = instruktor;
            this.evidencijaZaUređivanje = null;

            Init();
        }

        public EvidencijaCasaView(Korisnik instruktor, EvidencijaCasa evidencija)
        {
            InitializeComponent();
            this.instruktor = instruktor;
            this.evidencijaZaUređivanje = evidencija;

            Init();

            tbDatum.SelectedDate = evidencija.Datum;
            tbTema.Text = evidencija.TemaCasa;
            tbOpis.Text = evidencija.Opis;
            //tbPrisutni.Text = evidencija.PrisutniUcenici;
            tbTrajanje.Text = evidencija.TrajanjeMin.ToString();

            var kurs = kursevi.FirstOrDefault(k => k.Id == evidencija.KursId);
            if (kurs != null)
                cbKursevi.SelectedItem = kurs;

            PrecekirajPrisutneIzSacuvanogStringa(evidencija.PrisutniUcenici);
        }

        private void PrecekirajPrisutneIzSacuvanogStringa(string sacuvani)
        {
            if (string.IsNullOrWhiteSpace(sacuvani) || lstUcenici.ItemsSource == null)
            {
                return;
            }
            var setImena = sacuvani.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var item in (IEnumerable<UcenikPrisustvo>)lstUcenici.ItemsSource)
            {
                if (setImena.Contains(item.Ime))
                    item.Prisutan = true;
            }

            lstUcenici.Items.Refresh();
        }

        private void CbKursevi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using var context = new AppDbContext();

            if (cbKursevi.SelectedItem is not Kurs odabraniKurs)
            {
                lstUcenici.ItemsSource = null;
                return;
            }

            var countOnCourse = context.Korisnici
                .Count(u => u.Uloga == "Ucenik" && u.Kursevi.Any(k => k.Id == odabraniKurs.Id));

            if (countOnCourse == 0)
            {
                
                DbSeeder.EnsureStudentsAndEnrollments(context, minPerCourse: 15);
                
                context.ChangeTracker.Clear();
            }

            var ucenici = new AppDbContext().Korisnici
                .Where(u => u.Uloga == "Ucenik" && u.Kursevi.Any(k => k.Id == odabraniKurs.Id))
                .Select(u => new UcenikPrisustvo
                {
                    UcenikId = u.Id,
                    Ime = u.Ime + " " + u.Prezime,
                    Prezime = u.Prezime,
                    Prisutan = false
                })
                .OrderBy(x => x.Prezime).ThenBy(x => x.Ime)
                .ToList();

            lstUcenici.ItemsSource = ucenici;

            if (evidencijaZaUređivanje != null)
            {
                using var ctx = new AppDbContext();
                var pris = ctx.Prisustva
                    .Where(p => p.EvidencijaCasaId == evidencijaZaUređivanje.Id)
                    .ToList();

                if (pris.Count > 0)
                {
                    var map = pris.ToDictionary(p => p.UcenikId, p => p.Prisutan);
                    foreach (var x in ucenici)
                        if (map.TryGetValue(x.UcenikId, out var pr)) x.Prisutan = pr;
                    lstUcenici.Items.Refresh();
                }
                else
                {
                    PrecekirajPrisutneIzSacuvanogStringa(evidencijaZaUređivanje.PrisutniUcenici);
                }
            }
        }

        private void Init()
        {
            tbDatum.SelectedDate = DateTime.Now;

            using (var context = new AppDbContext())
            {
                EnsureStudentsIfEmpty(context);

                kursevi = context.Kursevi
                                 .Where(k => k.InstruktorId == instruktor.Id)
                                 .ToList();
                cbKursevi.ItemsSource = kursevi;
            }

            if (kursevi.Count > 0)
                cbKursevi.SelectedIndex = 0; 
        }


        private void Sacuvaj_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbTema.Text) || tbDatum.SelectedDate == null)
            { MessageBox.Show((string)Application.Current.Resources["Msg_RequiredFields"]); return; }

            if (cbKursevi.SelectedItem is not Kurs odabraniKurs)
            { MessageBox.Show((string)Application.Current.Resources["Msg_SelectCourse"]); return; }

            try
            {
                using var context = new AppDbContext();
                EvidencijaCasa evidencija;

                if (evidencijaZaUređivanje == null)
                {
                    evidencija = new EvidencijaCasa { InstruktorId = instruktor.Id };
                    context.EvidencijeCasa.Add(evidencija);
                }
                else
                {
                    evidencija = context.EvidencijeCasa.Find(evidencijaZaUređivanje.Id)
                                 ?? throw new Exception("Evidencija nije pronađena.");
                }

                evidencija.KursId = odabraniKurs.Id;
                evidencija.KursNaziv = odabraniKurs.Naziv;
                evidencija.Datum = tbDatum.SelectedDate.Value;
                evidencija.TemaCasa = tbTema.Text;
                evidencija.Opis = tbOpis.Text;
                evidencija.TrajanjeMin = int.TryParse(tbTrajanje.Text, out int t) ? t : 0;

                
                var prisutniImena = (lstUcenici.ItemsSource as IEnumerable<UcenikPrisustvo>)?
                    .Where(x => x.Prisutan).Select(x => x.Ime).ToList() ?? new List<string>();
                evidencija.PrisutniUcenici = string.Join(", ", prisutniImena);

                context.SaveChanges(); 

                var list = (lstUcenici.ItemsSource as IEnumerable<UcenikPrisustvo>)?.ToList() ?? new List<UcenikPrisustvo>();
                var ids = list.Select(x => x.UcenikId).ToList();

                var postojece = context.Prisustva
                    .Where(p => p.EvidencijaCasaId == evidencija.Id && ids.Contains(p.UcenikId))
                    .ToList();

                foreach (var x in list)
                {
                    var post = postojece.FirstOrDefault(p => p.UcenikId == x.UcenikId);
                    if (post == null)
                    {
                        context.Prisustva.Add(new Prisustvo
                        {
                            EvidencijaCasaId = evidencija.Id,
                            UcenikId = x.UcenikId,
                            Prisutan = x.Prisutan
                        });
                    }
                    else
                    {
                        post.Prisutan = x.Prisutan; 
                    }
                }

                context.SaveChanges();

                MessageBox.Show((string)Application.Current.Resources["Msg_SaveSuccess"]);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{Application.Current.Resources["Msg_SaveError"]}\n{ex.Message}",
                    (string)Application.Current.Resources["Msg_SaveErrorTitle"],
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void CbSelectAll_Checked(object sender, RoutedEventArgs e)
        {
            bool isChecked = (sender as CheckBox)?.IsChecked == true;
            if (lstUcenici.ItemsSource is IEnumerable<UcenikPrisustvo> lista)
            {
                foreach (var u in lista)
                    u.Prisutan = isChecked;

                lstUcenici.Items.Refresh();
            }
        }



        private void Exportuj_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbTema.Text) || tbDatum.SelectedDate == null || cbKursevi.SelectedItem is not Kurs odabraniKurs)
            {
                MessageBox.Show((string)Application.Current.Resources["Msg_ExportRequired"]);
                return;
            }

            var saveDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Text file (*.txt)|*.txt",
                FileName = $"Evidencija_{tbDatum.SelectedDate:yyyyMMdd}_{odabraniKurs.Naziv}.txt"
            };

            if (saveDialog.ShowDialog() == true)
            {
                string tekst = $"Datum: {tbDatum.SelectedDate:dd.MM.yyyy}\n" +
                               $"Kurs: {odabraniKurs.Naziv}\n" +
                               $"Tema časa: {tbTema.Text}\n" +
                               $"Trajanje: {tbTrajanje.Text} minuta\n" +
                               $"Opis: {tbOpis.Text}\n";

                System.IO.File.WriteAllText(saveDialog.FileName, tekst);
                MessageBox.Show((string)Application.Current.Resources["Msg_ExportSuccess"]);
            }
        }


        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private static void EnsureStudentsIfEmpty(AppDbContext context)
        {
            if (context.Korisnici.Any(k => k.Uloga == "Ucenik"))
                return;

            var imena = new[]
            {
        "Luka","Sara","Milan","Ana","Nikola","Petar","Jovana","Filip","Teodora","Marko",
        "Marija","Stefan","Milica","Aleksa","Ivana","Mia","Vuk","Tara","Ognjen","Elena"
    };

            var prezimena = new[]
            {
        "Petrović","Jovanović","Marković","Ilić","Nikolić","Marić","Kovačević","Matić","Stanković","Savić",
        "Radić","Lukić","Vasiljević","Popović","Vujić","Đurić","Babić","Milić","Gajić","Vuković"
    };

            var rnd = new Random();
            var novi = new List<Korisnik>();
            for (int i = 0; i < 20; i++)
            {
                var ime = imena[rnd.Next(imena.Length)];
                var prez = prezimena[rnd.Next(prezimena.Length)];
                var email = $"{Trans(ime).ToLower()}.{Trans(prez).ToLower()}{rnd.Next(100, 999)}@mail.com";

                novi.Add(new Korisnik
                {
                    Ime = ime,
                    Prezime = prez,
                    Email = email,
                    Lozinka = "test123",
                    Uloga = "Ucenik",
                    MoraPromijenitiLozinku = false,
                    Kursevi = new List<Kurs>() 
                });
            }

            var kursevi = context.Kursevi.ToList();
            if (kursevi.Count > 0)
            {
                int idx = 0;
                foreach (var u in novi)
                {
                    var kurs = kursevi[idx % kursevi.Count];
                    context.Attach(kurs);
                    u.Kursevi.Add(kurs);
                    idx++;
                }
            }

            context.Korisnici.AddRange(novi);
            context.SaveChanges();

            static string Trans(string s) =>
                s.Replace("š", "s").Replace("đ", "dj").Replace("č", "c").Replace("ć", "c").Replace("ž", "z")
                 .Replace("Š", "S").Replace("Đ", "Dj").Replace("Č", "C").Replace("Ć", "C").Replace("Ž", "Z");
        }

    }
}
