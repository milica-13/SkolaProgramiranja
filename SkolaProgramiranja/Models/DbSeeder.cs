using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkolaProgramiranja.Models
{
    internal class DbSeeder
    {
        public static void SeedStudentsIfNeeded()
        {
            using var context = new AppDbContext();

            // Ako već ima učenika, ne radi ništa
            if (context.Korisnici.Any(k => k.Uloga == "Ucenik"))
                return;

            var imena = new[]
            {
                "Luka","Sara","Milan","Ana","Nikola","Petar","Jovana","Filip","Teodora","Marko",
                "Marija","Stefan","Milica","Aleksa","Ivana","Mia","Vuk","Tara","Ognjen","Dorotea",
                "Andrej","Elena","Matea","Relja","Una","Nina","Ilija","Helena","Pavle","Lena"
            };

            var prezimena = new[]
            {
                "Petrović","Jovanović","Marković","Ilić","Nikolić","Marić","Kovačević","Milinković","Šarić","Matić",
                "Lukić","Stanković","Radić","Savić","Maksimović","Popović","Vasiljević","Vujić","Đurić","Babić",
                "Milić","Gajić","Mrdak","Tomić","Cvijetić","Knežević","Božić","Krunić","Vuković","Gligorić"
            };

            var rnd = new Random();
            var novi = new List<Korisnik>();
            int howMany = 20;

            for (int i = 0; i < howMany; i++)
            {
                var ime = imena[rnd.Next(imena.Length)];
                var prezime = prezimena[rnd.Next(prezimena.Length)];
                var emailBase = $"{Transliterate(ime).ToLower()}.{Transliterate(prezime).ToLower()}";
                var email = $"{emailBase}{rnd.Next(100, 999)}@mail.com"; // smanji šansu kolizije

                novi.Add(new Korisnik
                {
                    Ime = ime,
                    Prezime = prezime,
                    Email = email,
                    Lozinka = "test123",     // stavi hash ako koristiš hashiranje
                    Uloga = "Ucenik",
                    MoraPromijenitiLozinku = false
                });
            }

            context.Korisnici.AddRange(novi);
            context.SaveChanges();

            // (Opcionalno) raspodijeli učenike po postojećim kursevima – ravnomjerno
            var sviKursevi = context.Kursevi.ToList();
            if (sviKursevi.Count > 0)
            {
                var ucenici = context.Korisnici.Where(k => k.Uloga == "Ucenik").ToList();

                int idx = 0;
                foreach (var u in ucenici)
                {
                    var kurs = sviKursevi[idx % sviKursevi.Count];
                    // osiguraj da kolekcije postoje
                    u.Kursevi ??= new List<Kurs>();
                    kurs.Polaznici ??= new List<Korisnik>();

                    u.Kursevi.Add(kurs);
                    // kurs.Polaznici.Add(u); // nije nužno dodavati na obje strane, EF će povezati
                    idx++;
                }

                context.SaveChanges();
            }
        }

        // Pojednostavljena transliteracija za email (bez dijakritika)
        private static string Transliterate(string s)
        {
            return s
                .Replace("š", "s").Replace("đ", "dj").Replace("č", "c")
                .Replace("ć", "c").Replace("ž", "z")
                .Replace("Š", "S").Replace("Đ", "Dj").Replace("Č", "C")
                .Replace("Ć", "C").Replace("Ž", "Z");
        }

        public static void EnsureStudentsAndEnrollments(AppDbContext context, int minPerCourse = 15)
        {
            // 1) ako nema dovoljno učenika uopšte – generiši
            EnsureStudentsIfEmpty(context, totalTarget: Math.Max(50, context.Kursevi.Count() * minPerCourse));

            // 2) upiši na kurseve tako da svaki ima bar minPerCourse (ne dodaje duplikate)
            var kursevi = context.Kursevi.ToList();
            var ucenici = context.Korisnici.Where(k => k.Uloga == "Ucenik").ToList();

            foreach (var kurs in kursevi)
            {
                // trenutni broj
                var count = context.Entry(kurs)
                    .Collection(k => k.Polaznici)
                    .Query()
                    .Count();


                if (count >= minPerCourse) continue;

                // izaberi učenike koji još NISU na ovom kursu
                var need = minPerCourse - count;
                var candidates = ucenici
                    .Where(u => !context.Entry(u).Collection(x => x.Kursevi).Query().Any(k => k.Id == kurs.Id))
                    .Take(need)
                    .ToList();

                foreach (var u in candidates)
                {
                    context.Attach(kurs);
                    u.Kursevi ??= new List<Kurs>();
                    u.Kursevi.Add(kurs);
                }
                context.SaveChanges();
            }
        }

        public static void EnsureStudentsIfEmpty(AppDbContext context, int totalTarget)
        {
            if (context.Korisnici.Any(k => k.Uloga == "Ucenik")) return;

            var imena = new[] { "Luka","Sara","Milan","Ana","Nikola","Petar","Jovana","Filip","Teodora","Marko",
            "Marija","Stefan","Milica","Aleksa","Ivana","Mia","Vuk","Tara","Ognjen","Elena","Ilija","Nina","Pavle","Lena" };
            var prez = new[] { "Popović","Stanković","Gajić","Vuković","Jovanović","Marković","Ilić","Nikolić","Marić","Kovačević",
            "Matić","Stanković","Radić","Savić","Lukić","Vasiljević","Popović","Vujić","Đurić","Babić" };

            var rnd = new Random();
            var list = new List<Korisnik>();
            for (int i = 0; i < totalTarget; i++)
            {
                var ime = imena[rnd.Next(imena.Length)];
                var prezime = prez[rnd.Next(prez.Length)];
                var email = $"{T(ime).ToLower()}.{T(prezime).ToLower()}{rnd.Next(100, 999)}@mail.com";
                list.Add(new Korisnik { Ime = ime, Prezime = prezime, Email = email, Lozinka = "test123", Uloga = "Ucenik", MoraPromijenitiLozinku = false });

            }
            context.Korisnici.AddRange(list);
            context.SaveChanges();

            static string T(string s) => s.Replace("š", "s").Replace("đ", "dj").Replace("č", "c").Replace("ć", "c").Replace("ž", "z")
                                     .Replace("Š", "S").Replace("Đ", "Dj").Replace("Č", "C").Replace("Ć", "C").Replace("Ž", "Z");
        }
    }
}
