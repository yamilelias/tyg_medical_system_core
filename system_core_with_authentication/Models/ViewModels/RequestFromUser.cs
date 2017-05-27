using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace system_core_with_authentication.Models.ViewModels
{
    public class RequestFromUser
    {
        public List<RepositionStock> RepositionStockList { get; set; }
        public List<ShiftChange> ShiftChange { get; set; }
        public List<BreastFeeding> BreastFeeding { get; set; }
        public List<Permit> Permit { get; set; }
        public List<AllowanceWithoutPayment> AllowanceWithoutPayment { get; set; }
        public List<Vacations> Vacations { get; set; }
        public List<Maternity_Leave> Maternity_Leave { get; set; }
        public List<Viatical> Viatical { get; set; }
    }
}
