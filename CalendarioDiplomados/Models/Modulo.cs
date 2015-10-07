using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalendarioDiplomados.Models
{
    public class Modulo
    {
        public int ID { get; set; }
        public string nombre { get; set; }

        public virtual Diplomado Diplomado { get; set; }
        public int DiplomadoID { get; set; }

        public virtual ICollection<Taller> talleres { get; set; }
    }
}