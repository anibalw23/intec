using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CalendarioDiplomados.Models
{
    public class Calendario
    {
        public int ID { get; set; }
        public string nombre { get; set; }

        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime fechaInicio { get; set; }

        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime fechaFin { get; set; }

        public virtual Grupo Grupo { get; set; }
        public int GrupoID { get; set; }

        public virtual ICollection<Evento> eventos { get; set; }
    }
}