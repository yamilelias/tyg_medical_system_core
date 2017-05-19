using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace system_core_with_authentication.Models.ViewModels
{
    public class MedicamentsWithUserLocationsViewModel
    {
        public List<Medicament> MedicamentsForCreateReposition { get; set; }
        public List<string> UserLocations { get; set; }
    }
}
