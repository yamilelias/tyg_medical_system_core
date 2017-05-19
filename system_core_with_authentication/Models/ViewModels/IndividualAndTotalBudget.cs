using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace system_core_with_authentication.Models.ViewModels
{
    public class IndividualAndTotalBudget
    {
        public List<MedicamentWithTotalStock> medicamentWBudget { get; set; }
        public double totalBudget { get; set; }
    }
}
