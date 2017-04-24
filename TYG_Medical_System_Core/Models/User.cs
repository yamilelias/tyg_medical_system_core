using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TYG_Medical_System_Core.Models
{
    public class User
    {

        public int ID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }
        public int Telephone { get; set; }

    }
}
