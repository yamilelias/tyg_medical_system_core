﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace system_core_with_authentication.Models.ViewModels
{
    public class r_Vacations_ViewModel
    {
        [Required]
        [DisplayName("Periodo de Vacaciones completo")]
        public string Vacation_Period_Completed { get; set; }

        [Required]
        [DisplayName("Dias Disponibles")]
        public int Available_Days { get; set; }

        [Required]
        [DisplayName("Dias solicitados")]
        public int Requested_Days { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:MM/dd/yyyy}")]
        [DisplayName("Fecha Inicio")]
        public DateTime Start_Date { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:MM/dd/yyyy}")]
        [DisplayName("Fecha Final")]
        public DateTime End_Date { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:MM/dd/yyyy}")]
        [DisplayName("Fecha Regreso")]
        public DateTime Comeback_Date { get; set; }

        [Required]
        [DisplayName("Dias Pendientes")]
        public int Pending_Days { get; set; }

        [DisplayName("Notas")]
        public string Notes { get; set; }
    }
}
