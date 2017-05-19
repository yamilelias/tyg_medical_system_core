using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace system_core_with_authentication.Models.ViewModels
{
    public class EditUserViewModel
    {

        public ApplicationUser appUser { get; set; }

        public string role { get; set; }

        public string[] Roles = { "Admin", "Supervisor", "Supervisor de RH", "Supervisor de Inventario", "Medico", "Enfermero" };

    }
}
