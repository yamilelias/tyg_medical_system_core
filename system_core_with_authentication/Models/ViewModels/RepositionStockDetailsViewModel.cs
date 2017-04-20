using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace system_core_with_authentication.Models.ViewModels
{
    public class RepositionStockDetailsViewModel
    {
        public virtual Medicament medicament { get; set; }
        public int CurrentStock { get; set; }
        public int RequestStock { get; set; }
    }
}
