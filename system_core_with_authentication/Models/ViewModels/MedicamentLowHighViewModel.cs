﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace system_core_with_authentication.Models.ViewModels
{
    public class MedicamentLowHighViewModel
    {
        public List<Medicament> MedicamentLow { get; set; }
        public List<Medicament> MedicamentHigh { get; set; }
    }
}
