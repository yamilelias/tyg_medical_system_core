using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace system_core_with_authentication.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Nombre")]
        public string Name { get; set; }
        [DisplayName("Primer Apellido")]
        public string LastName { get; set; }
        [DisplayName("Segundo Apellido")]
        public string SecondLastName { get; set; }
        [DisplayName("Direccion")]
        public string Address { get; set; }
        [DisplayName("Correo")]
        [EmailAddress]
        public string Email { get; set; }
        [DisplayName("Contraseña")]
        public string Password { get; set; }
        [DisplayName("Numero de Telefono")]
        public string PhoneNumber { get; set; }
        [DisplayName("Tipo de Usuario")]
        public string Type { get; set; }
        [DisplayName("Activo")]
        public bool Active { get; set; }
        [DisplayName("Fecha")]
        public DateTime EntryDate { get; set; }
        [DisplayName("Imagen")]
        public byte[] Image { get; set; }

    }
}
