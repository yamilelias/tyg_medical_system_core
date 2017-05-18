using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace system_core_with_authentication.Models.ViewModels
{
    public class r_Permit_ViewModel
    {
        [Required]
        [DisplayName("Tipo")]
        public int Type { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:MM/dd/yyyy}")]
        [DisplayName("Fecha")]
        public DateTime Date { get; set; }
        [Required]
        [DisplayName("Hora Inicio")]
        public string Start_Hour { get; set; }
        [Required]
        [DisplayName("Hora Final")]
        public string End_Hour { get; set; }

        [DisplayName("Notas")]
        public string Notes { get; set; }
    }
}
