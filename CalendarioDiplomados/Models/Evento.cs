using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CalendarioDiplomados.Models
{


    public class Evento
    {
        public int ID { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayName("Fecha de Inicio")]
        
        public DateTime fechaIncicio { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayName("Fecha de Fin")]
        public DateTime fechaFin { get; set; }
        public int duracion { get; set; }
        public int orden { get; set; }

        public virtual Calendario Calendario { get; set; }
        public int CalendarioID { get; set; }

        
        [DisplayName("Taller")]
        public int? TallerID { get; set; }
        public virtual Taller Taller { get; set; }
       
        
        [DisplayName("Facilitador")]
        public int? FacilitadorID { get; set; }
        public virtual Facilitador Facilitador { get; set; }

        [DisplayName("Facilitador")]
        public int? ChoferID { get; set; }
        public virtual Chofer Chofer { get; set; }

        //Dias que se repite
        [DisplayName("Lunes")]
        public bool isLunes { get; set; }
        [DisplayName("Martes")]
        public bool isMartes { get; set; }
        [DisplayName("Miércoles")]
        public bool isMiercoles { get; set; }
        [DisplayName("Jueves")]
        public bool isJueves { get; set; }
        [DisplayName("Viernes")]
        public bool isViernes { get; set; }
        [DisplayName("Sábado")]
        public bool isSabado { get; set; }
        [DisplayName("Domingo")]
        public bool isDomingo { get; set; }

        public virtual ICollection<Recurrencia> recurrecias { get; set; }


    }
}