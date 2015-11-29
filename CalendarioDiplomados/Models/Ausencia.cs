using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalendarioDiplomados.Models
{
    public class Ausencia
    {
        public int ID { get; set; }

        public virtual Evento Evento {get; set; }
        public int eventoID { get; set; }

        public virtual Participante Participante { get; set; }
        public int participanteID { get; set; }

    }
}