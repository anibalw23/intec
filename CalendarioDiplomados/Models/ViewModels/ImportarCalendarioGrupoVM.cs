using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalendarioDiplomados.Models.ViewModels
{
    public class ImportarCalendarioGrupoVM
    {
        public int grupoSrc { get; set; }
        public int grupoDest { get; set; }
        public int calendarioId { get; set; }
    }
}