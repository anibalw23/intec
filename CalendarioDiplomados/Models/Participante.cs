using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalendarioDiplomados.Models
{
    public class Participante
    {
        public int ID { get; set; }
        public string cedula { get; set; }
        public string nombre { get; set; }
        public string telefono { get; set; }
        public string direccion { get; set; }

        public virtual ICollection<Grupo> grupos { get; set; }
    }
}