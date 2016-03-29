using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalendarioDiplomados.Models
{

    public enum TandaAusencia { 
        manana_tarde = 0,
        manana = 1,
        tarde = 2,
        noche = 3
    }

    public class Ausencia
    {
        public int ID { get; set; }

        public virtual Evento Evento {get; set; }
        public int eventoID { get; set; }

        public virtual Participante Participante { get; set; }
        public int participanteID { get; set; }

        public TandaAusencia TandaAusencia { get; set; }


    }
}