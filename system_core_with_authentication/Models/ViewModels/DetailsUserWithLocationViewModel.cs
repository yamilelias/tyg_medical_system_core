using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace system_core_with_authentication.Models.ViewModels
{
    public class DetailsUserWithLocationViewModel
    {
        public ApplicationUser user { get; set; }
        public List<LocationSchedule> ls { get; set; }
    }
}
