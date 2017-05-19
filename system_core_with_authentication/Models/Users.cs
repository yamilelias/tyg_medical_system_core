using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace system_core_with_authentication.Models
{
    public class Users
    {

        public string Id { get; set; }
        
        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public string RoleId { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string SecondLastName { get; set; }

        public string Telephone { get; set; }

        public string UserImage { get; set; }


        public virtual int AccessFailedCount { get; set; }
      
        public virtual bool LockoutEnabled { get; set; }
        
        public virtual DateTimeOffset? LockoutEnd { get; set; }
       
        public virtual bool TwoFactorEnabled { get; set; }
        
        public virtual bool PhoneNumberConfirmed { get; set; }
       
        public virtual string ConcurrencyStamp { get; set; }
       
        public virtual string SecurityStamp { get; set; }
      
        public virtual string PasswordHash { get; set; }
       
        public virtual bool EmailConfirmed { get; set; }
       
        public virtual string NormalizedEmail { get; set; }
       
        public virtual string NormalizedUserName { get; set; }
      
       
    }
}
