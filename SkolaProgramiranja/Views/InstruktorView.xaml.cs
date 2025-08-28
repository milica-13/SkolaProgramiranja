using SkolaProgramiranja.Models;
using System;
using System.Linq;
using System.Windows;
using MaterialDesignThemes.Wpf;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using System.Text;


namespace SkolaProgramiranja.Views
{
    public partial class InstruktorView : Window
    {
        private readonly Korisnik trenutniInstruktor;
        private readonly PaletteHelper _paletteHelper = new PaletteHelper();
        //private List<EvidencijaCasa> _mojeEvidencije;
        //private List<PlanCasa> _planovi = new List<PlanCasa>();
        private bool _isInitializing;
        private List<EvidencijaCasa> _mojeEvidencije = new(); // nikad null
        private List<PlanCasa> _planovi = new();
        public InstruktorView(Korisnik instruktor)
        {
            InitializeComponent();
            CommandBindings.Add(new CommandBinding(ApplicationCommands.New, (s, e) => DodajPlan_Click(null, null)));

            ThemeHelper.ApplyTheme(this);
            trenutniInstruktor = instruktor;
            this.DataContext = instruktor;

            string resourcesFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
            string imeSlike = string.IsNullOrWhiteSpace(trenutniInstruktor.SlikaProfilaFullPath)
                ? "profilna.png"
                : trenutniInstruktor.SlikaProfilaFullPath;

            string punaPutanja = System.IO.Path.Combine(resourcesFolder, imeSlike);

            if (System.IO.File.Exists(punaPutanja))
            {
                imgProfilna.Source = new BitmapImage(new Uri(punaPutanja));
            }
            else
            {
                string fallbackPath = System.IO.Path.Combine(resourcesFolder, "avatar_placeholder.png");
                if (System.IO.File.Exists(fallbackPath))
                {
                    imgProfilna.Source = new BitmapImage(new Uri(fallbackPath));
                }
                else
                {
                    MessageBox.Show("Nije pronađena profilna slika niti zamjenska slika.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

            ApplySavedTheme();
            UcitajMojeKurseve();
        }

        private void UcitajMojeKurseve()
        {
            if (trenutniInstruktor == null) return;

            _isInitializing = true;

            try
            {
                using (var context = new AppDbContext())
                {
                    // 1) Kursevi instruktora
                    var kursevi = context.Kursevi
                        .Where(k => k.InstruktorId == trenutniInstruktor.Id)
                        .OrderBy(k => k.Naziv)
                        .ToList();

                    // 2) Evidencije instruktora (sve)
                    _mojeEvidencije = context.EvidencijeCasa
                        .Where(e => e.InstruktorId == trenutniInstruktor.Id)
                        .OrderByDescending(e => e.Datum)
                        .ToList();

                    // 3) Postavi combo bez trigera eventa
                    cbKursevi.SelectionChanged -= cbKursevi_SelectionChanged;
                    cbKursevi.ItemsSource = kursevi;
                    cbKursevi.SelectedIndex = kursevi.Count > 0 ? 0 : -1;
                    cbKursevi.SelectionChanged += cbKursevi_SelectionChanged;

                    // 4) Osvježi UI za prvi kurs (bez oslanjanja na event)
                    calCasovi.SelectedDates.Clear();

                    if (kursevi.Count > 0)
                    {
                        LoadForCourse(kursevi[0].Id);
                    }
                    else
                    {
                        _planovi = new List<PlanCasa>();
                        icRaspored.ItemsSource = _planovi;
                        RenderEvidencije(new List<EvidencijaCasa>());
                    }
                }
            }
            finally
            {
                _isInitializing = false;
            }
        }

        // ========== UČITAJ PODATKE ZA IZABRANI KURS ==========
        private void LoadForCourse(int kursId)
        {
            using (var context = new AppDbContext())
            {
                _planovi = context.PlanoviCasa
                    .Where(p => p.KursId == kursId)
                    .OrderBy(p => p.Datum)
                    .ToList();
            }

            icRaspored.ItemsSource = _planovi;

            // kalendar
            calCasovi.SelectedDates.Clear();

            var evidencijeKursa = (_mojeEvidencije ?? Enumerable.Empty<EvidencijaCasa>())
                .Where(ev => ev.KursId == kursId)
                .OrderByDescending(ev => ev.Datum)
                .ToList();

            foreach (var ev in evidencijeKursa)
                calCasovi.SelectedDates.Add(ev.Datum);

            foreach (var p in _planovi ?? Enumerable.Empty<PlanCasa>())
                calCasovi.SelectedDates.Add(p.Datum);

            RenderEvidencije(evidencijeKursa);
        }

        // ========== HANDLER: PROMJENA KURSA ==========
        private void cbKursevi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isInitializing) return; // ignorisi event tokom inicijalizacije
            if (cbKursevi.SelectedItem is not Kurs selektovani) return;

            try
            {
                LoadForCourse(selektovani.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri promjeni kursa.\n{ex.Message}", "Greška",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ========== RENDER EVIDENCIJA (ostaje kao ranije) ==========
        private void RenderEvidencije(List<EvidencijaCasa> evidencije)
        {
            spEvidencije.Children.Clear();

            if (evidencije == null || evidencije.Count == 0)
            {
                spEvidencije.Children.Add(new TextBlock
                {
                    Text = "Nema sačuvanih evidencija.",
                    Margin = new Thickness(0, 0, 0, 6)
                });
                return;
            }

            foreach (var ev in evidencije)
            {
                spEvidencije.Children.Add(new TextBlock
                {
                    Text = $"Čas ({ev.Datum:dd.MM.yyyy})",
                    FontWeight = FontWeights.SemiBold,
                    Margin = new Thickness(0, 8, 0, 0)
                });
                spEvidencije.Children.Add(new TextBlock
                {
                    Text = $"Tema: {ev.TemaCasa}",
                    TextWrapping = TextWrapping.Wrap
                });
                spEvidencije.Children.Add(new TextBlock
                {
                    Text = $"Prisustvovali: {ev.PrisutniUcenici}",
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(0, 0, 0, 6)
                });
            }
        }

        private void Odjava_Click(object sender, RoutedEventArgs e)
        {
            var login = new LoginView();
            login.Show();
            this.Close();
        }   

        private void UrediProfil_Click(object sender, RoutedEventArgs e)
        {
            var forma = new UrediInstruktorProfilView(trenutniInstruktor);
            if(forma.ShowDialog() == true)
            {
                MessageBox.Show("Profil ažuriran.");
                MainSnackbar.MessageQueue?.Enqueue("Profil ažuriran.");
            }
            
        }

        private void PregledProfila_Click(object sender, RoutedEventArgs e)
        {

            if (trenutniInstruktor != null)
            {
                var view = new PrikazInstruktoraProfilView(trenutniInstruktor);
                view.ShowDialog();
            }

        }
        private void EvidencijaCasa_Click(object sender, RoutedEventArgs e)
        {
            var forma = new EvidencijaCasaView(trenutniInstruktor);
            forma.ShowDialog();
        }

        private void PregledEvidencije_Click(object sender, RoutedEventArgs e) 
        {
            var forma = new InstruktorPregledEvidencijeView(trenutniInstruktor);
            forma.ShowDialog();
        }

        private void PromijeniLozinku_Click(object sender, RoutedEventArgs e)
        {
            var forma = new PromijeniLozinkuView(trenutniInstruktor);
            forma.ShowDialog();
            //MainSnackbar.MessageQueue?.Enqueue("Lozinka uspješno promjenjena.");
        }

        private void ThemeToggle_Checked(object sender, RoutedEventArgs e)
        {
            ThemeHelper.SaveTheme("Dark");
            ThemeHelper.ApplyTheme(this);
        }

        private void ThemeToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            ThemeHelper.SaveTheme("Light");
            ThemeHelper.ApplyTheme(this);
        }


        private void ApplySavedTheme()
        {
            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();
            //ITheme theme = _paletteHelper.GetTheme();
            string savedTheme = Properties.Settings.Default.Theme;
            if (savedTheme == "Dark")
                theme.SetBaseTheme(BaseTheme.Dark);
            else
                theme.SetBaseTheme(BaseTheme.Light);

            _paletteHelper.SetTheme(theme);
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;

            var comboBoxItem = e.AddedItems[0] as ComboBoxItem;
            string lang = comboBoxItem?.Tag?.ToString();

            if (!string.IsNullOrEmpty(lang))
            {
                string resourcePath = $"Languages/Strings.{lang}.xaml";

                var dictionary = new ResourceDictionary
                {
                    Source = new Uri(resourcePath, UriKind.Relative)
                };

                // ukloni stare prevode
                var existing = Application.Current.Resources.MergedDictionaries
                    .FirstOrDefault(d => d.Source != null && d.Source.OriginalString.StartsWith("Resources/Strings."));

                if (existing != null)
                    Application.Current.Resources.MergedDictionaries.Remove(existing);

                Application.Current.Resources.MergedDictionaries.Add(dictionary);
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
            var novi = new InstruktorView(trenutniInstruktor);
            novi.Show();
            this.Close();
        }

        private void DodajPlan_Click(object sender, RoutedEventArgs e)
        {
            // sljedeći slobodni datum = max(datum) + 7d (ili danas ako nema nijedan)
            DateTime nextDate;
            if (_planovi.Count > 0)
                nextDate = _planovi.Max(p => p.Datum).AddDays(7);
            else
                nextDate = DateTime.Today;

            /*var novi = new PlanCasa
            {
                KursId = ((Kurs)dgMojiKursevi.Items[0]).Id,
                Datum = nextDate,
                Tema = "(tema)"
            };
            */
            // trenutni izabrani kurs
            var kurs = cbKursevi.SelectedItem as Kurs;
            if (kurs == null) return;

            var novi = new PlanCasa
            {
                KursId = kurs.Id,
                Datum = nextDate,
                Tema = "(tema)"
            };


            //_planovi.Add(novi);
            //icRaspored.Items.Refresh();
            _planovi.Add(novi);
            icRaspored.Items.Refresh();

            SpremiPlan_Click(null, null); // automatsko snimanje

        }



        private void SpremiPlan_Click(object sender, RoutedEventArgs e)
        {
            // snimi sve izmjene
            using var ctx = new AppDbContext();
            foreach (var p in _planovi)
            {
                // upsert
                var postojeci = ctx.PlanoviCasa
                                  .FirstOrDefault(x => x.Id == p.Id);

                if (postojeci == null)
                    ctx.PlanoviCasa.Add(p);
                else
                {
                    postojeci.Datum = p.Datum;
                    postojeci.Tema = p.Tema;
                }
            }
            ctx.SaveChanges();

            MessageBox.Show("Plan sačuvan.");
        }

        private void calCasovi_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (calCasovi.SelectedDate == null) return;
            var datum = calCasovi.SelectedDate.Value.Date;
            var plan = _planovi.FirstOrDefault(p => p.Datum.Date == datum);
           
        }

        private void tbDatum_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Pronađi popup koji je u istom DataTemplate-u
            var tb = sender as TextBox;
            if (tb == null) return;

            var grid = VisualTreeHelper.GetParent(tb) as Grid;
            if (grid == null) return;

            var popup = grid.Children.OfType<Popup>().FirstOrDefault();
            if (popup != null)
            {
                popup.PlacementTarget = tb;
                popup.IsOpen = true;
            }
        }
        
    }
}
