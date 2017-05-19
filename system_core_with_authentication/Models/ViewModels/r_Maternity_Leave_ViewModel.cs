using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace system_core_with_authentication.Models.ViewModels
{
    public class r_Maternity_Leave_ViewModel
    {
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:MM/dd/yyyy}")]
        [DisplayName("Fecha Inicio")]
        public DateTime Start_Date { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:MM/dd/yyyy}")]
        [DisplayName("Fecha Final")]
        public DateTime End_Date { get; set; }

        [Required]
        [DisplayName("Dias Cubiertos")]
        public int Covered_Days { get; set; }

        [Required]
        [DisplayName("Folio")]
        public string Folio { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:MM/dd/yyyy}")]
        [DisplayName("Fecha de Inicio de trabajo")]
        public DateTime Labor_Start_Date { get; set; }

        [Required]
        [DisplayName("Unidad Medica")]
        public string Medic_Unit { get; set; }

        [DisplayName("Notas")]
        public string Notes { get; set; }
    }
}
