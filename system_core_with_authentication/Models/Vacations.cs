using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace system_core_with_authentication.Models
{
    public class Vacations
    {
        public int Id { get; set; }
        [ForeignKey("RequestId")]
        public virtual Request Request { get; set; }
        public string Vacation_Period_Completed {get;set;}
        public int Available_Days { get; set; }
        public int Requested_Days { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set; }
        public DateTime Comeback_Date { get; set; }
        public int Pending_Days { get; set; }
        public bool Solved { get; set; }
    }
}
