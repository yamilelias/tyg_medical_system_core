using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace system_core_with_authentication.Models.ViewModels
{
    public class MedicamentWithTotalStock
    {
        public Medicament medicament { get; set; }
        public int sumTotal { get; set; }
    }
}
