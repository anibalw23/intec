using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalendarioDiplomados.Models
{
    public class Taller
    {
        public int ID { get; set; }
        public string nombre { get; set; }

        public virtual Modulo Modulo { get; set; }
        public int ModuloID { get; set; }

        public int orden { get; set; }
        public int cantidadDias { get; set; }

        public int? FacilitadorID { get; set; }
        public virtual Facilitador Facilitador { get; set; }

        public virtual ICollection<Evento> eventos { get; set; }
    }
}