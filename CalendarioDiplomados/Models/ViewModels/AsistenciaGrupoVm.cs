using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalendarioDiplomados.Models.ViewModels
{
    public class AsistenciaGrupoVm
    {

        public int participanteId { get; set; }
        public int eventoId { get; set; }
        public string cedula { get; set; }
        public string nombre { get; set; }
        public bool asistio { get; set; }
    }

}