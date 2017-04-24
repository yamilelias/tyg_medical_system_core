using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace system_core_with_authentication.Models
{
    public class RepositionStock
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("RequestId")]
        public virtual Request Request {get;set;}

        public virtual Location Location { get; set; }
        [DisplayName("Resuelto")]
        public bool Solved { get; set; }

        public virtual ICollection<RepositionStockDetailed> RepositionStockDetailed { get; set; }
    }
}
