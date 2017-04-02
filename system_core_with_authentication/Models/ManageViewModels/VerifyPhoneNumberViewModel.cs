using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace system_core_with_authentication.Models.ManageViewModels
{
    public class VerifyPhoneNumberViewModel
    {
        [Required]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
    }
}
