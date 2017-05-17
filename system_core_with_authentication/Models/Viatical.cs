using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace system_core_with_authentication.Models
{
    public class Viatical
    {
        public int Id { get; set; }
        [ForeignKey("RequestId")]
        public virtual Request Request { get; set; }
        public string Travel_Destination { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set; }
        public bool Solved { get; set; }
    }
}
