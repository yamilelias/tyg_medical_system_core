﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace system_core_with_authentication.Models.ViewModels
{
    public class CreateLocationScheduleViewModel
    {
        public ApplicationUser user { get; set; }

        public LocationSchedule ls { get; set; }

    }
}