using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkolaProgramiranja.Models
{
    public class Kurs
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }
        public int InstruktorId { get; set; }
        public ICollection<Korisnik> Polaznici { get; set; } = new List<Korisnik>();

        public Korisnik Instruktor { get; set; } = null!;
        [NotMapped]
        public string InstruktorFullName => $"{Instruktor.Ime} {Instruktor.Prezime}";
    }
}
