using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalendarioDiplomados.Models
{
    public class Grupo
    {
        public int ID { get; set; }
        public string nombre { get; set; }

        public int cantidadParticipantes { get; set; }

        public virtual Diplomado Diplomado { get; set; }
        public int DiplomadoID { get; set; }

        public virtual ICollection<Calendario> calendarios { get; set; }
    }
}