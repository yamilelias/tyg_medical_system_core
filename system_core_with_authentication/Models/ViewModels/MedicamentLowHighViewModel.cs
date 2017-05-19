using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace system_core_with_authentication.Models.ViewModels
{
    public class MedicamentLowHighViewModel
    {
        public List<MedicamentWithTotalStock> MedicamentLow { get; set; }
        public List<Medicament> MedicamentHigh { get; set; }
        public List<ApplicationUser> Users { get; set; }
        public List<Request> Requests { get; set; }
        public double sumBudget { get; set; }
        public RequestFromUser RequestFromUser { get; set; }
    }
}
