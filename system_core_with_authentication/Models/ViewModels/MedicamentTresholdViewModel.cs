using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace system_core_with_authentication.Models.ViewModels
{
    public class MedicamentTresholdViewModel
    {
        public int Id { get; set; }

        [DisplayName("Medicine")]
        public String Name { get; set; }

        [DisplayName("Current Stock")]
        public int CurrentStock { get; set; }
    }
}
