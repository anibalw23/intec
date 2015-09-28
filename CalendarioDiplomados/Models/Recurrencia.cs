using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalendarioDiplomados.Models
{
    public class Recurrencia
    {
        public int ID { get; set; }
        public int repeticiones { get; set; }

        public virtual Evento Evento { get; set; }
        public int EventoID { get; set; }

        public virtual DiaSemana DiaSemana { get; set; }
        public int DiaSemanaID { get; set; }
    }
}