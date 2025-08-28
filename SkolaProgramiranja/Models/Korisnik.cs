using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkolaProgramiranja.Models
{
    public class Korisnik
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Email { get; set; }
        public string Lozinka { get; set; } 
        public string Uloga { get; set; }

        public string? Obrazovanje { get; set; }
        public string? Biografija { get; set; }
        public string? Telefon { get; set; }
        public string? SlikaProfilaPath { get; set; } = "profilna.png";
        public bool MoraPromijenitiLozinku { get; set; } = true;

        public ICollection<Kurs> Kursevi { get; set; }
        public string SlikaProfilaFullPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(SlikaProfilaPath))
                    return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "avatar_placeholder.png");

                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SlikaProfilaPath);
            }
        }



    }

}
