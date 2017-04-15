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

        [ForeignKey("IdRepositionStock")]
        public int IdRepositionStock { get; set; }
        public virtual RepositionStock RepositionStock { get; set; }

        [ForeignKey("IdMedicament")]
        public int IdMedicament { get; set; }
        public virtual Medicament Medicament { get; set; }

        [DisplayName("Stock Actual")]
        public int CurrentStock { get; set; }

        [DisplayName("Stock Pedido")]
        public int RequestStock {get;set;}
    }
}
