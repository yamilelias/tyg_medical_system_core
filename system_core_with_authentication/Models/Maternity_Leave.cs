using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace system_core_with_authentication.Models
{
    public class Maternity_Leave
    {
        public int Id { get; set; }
        [ForeignKey("RequestId")]
        public virtual Request Request { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set; }
        public int Covered_Days { get; set; }
        public string Folio { get; set; }
        public DateTime Labor_Start_Date { get; set; }
        public string Medic_Unit { get; set; }
        public bool Solved { get; set; }
    }
}
