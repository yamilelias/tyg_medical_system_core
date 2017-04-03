using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TYG_Medical_System_Core.Models
{
    public class MedicineItem
    {
        public int Id { get; set; }
        public String Description { get; set; }
        public String Serial { get; set; }
        public String Note { get; set; }

        [DataType(DataType.Date)]
        public String MaintenanceDate { get; set; }

        public byte[] Image { get; set; }
    }
}
