using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace system_core_with_authentication.Models.ViewModels
{
    public class MaterialInventoryViewModel
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

        [FileExtensions(Extensions = "jpg,jpeg")]
        public IFormFile File { get; set; }

        public String ImageName { get; set; }
    }
}
