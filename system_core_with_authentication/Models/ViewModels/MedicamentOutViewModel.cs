using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace system_core_with_authentication.Models.ViewModels
{
    public class MedicamentOutViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        int Quantity { get; set; }
        public int Total { get; set; }
        public DateTime Expiration { get; set; }
    }
}
