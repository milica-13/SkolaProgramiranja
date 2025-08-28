using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkolaProgramiranja.Models
{
    public class PlanCasa
    {
        public int Id { get; set; }
        public int KursId { get; set; }
        public DateTime Datum { get; set; }
        public string Tema { get; set; } = "";
    }
}
