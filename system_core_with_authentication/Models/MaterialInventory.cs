using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace system_core_with_authentication.Models
{
    public class MaterialInventory
    {
        public int Id { get; set; }

        [Required]
        public String Description { get; set; }

        [Required]
        public String Serial { get; set; }

        [Required]
        public String Note { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public String MaintananceDate { get; set; }

        public String ImageName { get; set; }

        public String ImagePath { get; set; }

    }
}
