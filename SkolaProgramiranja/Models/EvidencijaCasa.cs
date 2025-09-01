using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    
namespace SkolaProgramiranja.Models
{
    public class EvidencijaCasa
    {
        public int Id { get; set; }
        public int InstruktorId { get; set; }

        public int KursId { get; set; } 
        public string KursNaziv { get; set; }
        public DateTime Datum { get; set; }
        public string TemaCasa { get; set; }
        public string Opis { get; set; }
        public string PrisutniUcenici { get; set; }
        public int TrajanjeMin { get; set; }

        public virtual Korisnik Instruktor { get; set; }
        public virtual Kurs Kurs { get; set; } 
    }
}
