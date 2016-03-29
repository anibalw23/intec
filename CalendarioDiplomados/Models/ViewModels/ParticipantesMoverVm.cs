using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CalendarioDiplomados.Models.ViewModels
{
    public class ParticipantesMoverVm
    
    {
        public int grupoSrcID { get; set; }
        [Required(ErrorMessage = "Seleccione el grupo a que desea mover!")]
        public int grupoDestID { get; set; }
        public string nombreGrupo { get; set; }
        public List<ParticipanteVm> participantes { get; set; }
        public SelectList grupoDestList { get; set; }
    }
}