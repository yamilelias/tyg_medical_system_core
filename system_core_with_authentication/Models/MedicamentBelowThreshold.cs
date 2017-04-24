using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using system_core_with_authentication.Models;

namespace system_core_with_authentication.Models
{
    public class MedicamentBelowThreshold
    {
        public int Id { get; set; }

        public int MedicamentId { get; set; }

        public int CurrentStock { get; set; }
    }
}
