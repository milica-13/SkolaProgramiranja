using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkolaProgramiranja.Models
{
    public class Prisustvo
    {
        public int Id { get; set; }
        public int EvidencijaCasaId { get; set; }
        public int UcenikId { get; set; }

        public Boolean Prisutan { get; set; }

        public EvidencijaCasa EvidencijaCasa { get; set; } = null!;
        public Korisnik Ucenik { get; set; } = null!;

            
    }   
}
