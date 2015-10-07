using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalendarioDiplomados.Models
{
    public class DiaSemana
    {
        public int ID { get; set; }
        public string nombre { get; set; }
        public int numero { get; set; }

        public virtual ICollection<Recurrencia> recurrecias { get; set; }
    }
}