using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace system_core_with_authentication.Models.ViewModels
{
    public class r_Viatical_ViewModel
    {
        [Required]
        [DisplayName("Destino del viaje")]
        public string Travel_Destination { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:MM/dd/yyyy}")]
        [DisplayName("Fecha Inicio")]
        public DateTime Start_Date { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:MM/dd/yyyy}")]
        [DisplayName("Fecha Final")]
        public DateTime End_Date { get; set; }

        [DisplayName("Notas")]
        public string Notes { get; set; }
    }
}
