using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalendarioDiplomados.Models.ViewModels
{
    public class AsistenciaVM
    {
        public string fecha { get; set; }
        public bool asistio { get; set; }
        public int eventoID { get; set; }
        public int participanteID { get; set; }

    }
}