using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace system_core_with_authentication.Models
{
    public class RepositionStockDetailed
    {
        [Key]
        public int Id { get; set; }

        public int MedicamentId { get; set; }

        [ForeignKey("MedicamentId")]
        public virtual Medicament Medicament { get; set; }


        [DisplayName("Cantidad actual")]
        public int CurrentStock { get; set; }

        [DisplayName("Cantidad a pedir")]
        public int RequestStock {get;set;}
    }
}
