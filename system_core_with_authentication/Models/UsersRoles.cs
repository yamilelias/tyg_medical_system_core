using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace system_core_with_authentication.Models
{
    public class UsersRoles
    {
        public List<SelectListItem> userRoles;

        public UsersRoles()
        {

            userRoles = new List<SelectListItem>();
        }

        public List<SelectListItem> getRoles(RoleManager<IdentityRole> roleManager)
        {
            var roles = roleManager.Roles.ToList();
            foreach (var Data in roles)
            {
                userRoles.Add(new SelectListItem()
                {
                    Value = Data.Id,
                    Text = Data.Name
                });
            }
            return userRoles;
        }

        public async Task<List<SelectListItem>> getRole(UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager, string ID)
        {
            userRoles = new List<SelectListItem>();
            string role;
            var users = await userManager.FindByIdAsync(ID);
            var roles = await userManager.GetRolesAsync(users);
            if (roles.Count == 0)
            {
                userRoles.Add(new SelectListItem()
                {
                    Value = "null",
                    Text = "No role"
                });
            } else
            {
                role = Convert.ToString(roles[0]);
                var rolesId = roleManager.Roles.Where(m => m.Name == role);
                foreach (var Data in rolesId)
                {
                    userRoles.Add(new SelectListItem()
                    {
                        Value = Data.Id,
                        Text = Data.Name
                    });
                }
            }

            return userRoles;
        }

    }

    

}
