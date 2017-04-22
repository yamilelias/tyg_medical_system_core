using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace system_core_with_authentication.Models
{
    public class Location
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Nombre")]
        public string Name { get; set; }
        [DisplayName("Direccion")]
        public string Address { get; set; }
        [DisplayName("Numero Telefonico")]
        public string PhoneNumber { get; set; }
    }
}
