using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace system_core_with_authentication.Models.ViewModels
{
    public class RepositionStockViewModel
    {
        public string Note { get; set; }
        public virtual Location location { get; set; }
        public List<RepositionStockDetailed> rsd { get; set; }

    }
}
