using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkolaProgramiranja.Models
{
    internal class UcenikPrisustvo
    {
        public int UcenikId {  get; set; }
        public string Ime { get; set; }
        public string Prezime {  get; set; }    
        public Boolean Prisutan {  get; set; }
    }
}
