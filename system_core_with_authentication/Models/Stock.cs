using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace system_core_with_authentication.Models
{
    public class Stock
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Total { get; set; }

        [Required]
        [DisplayName("Caducidad")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Expiration { get; set; }

        [Required]
        [DisplayName("Medicamento")]
        public int MedicamentId { get; set; }

        [ForeignKey("MedicamentId")]
        public Medicament Medicament { get; set; }
    }
}
