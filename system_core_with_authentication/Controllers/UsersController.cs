using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using system_core_with_authentication.Data;
using system_core_with_authentication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace system_core_with_authentication.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        UserManager<ApplicationUser> _userManager;
        RoleManager<IdentityRole> _roleManager;
        UsersRoles _usersRole;
        public List<SelectListItem> userRole;

        public UsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager) 
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _usersRole = new UsersRoles();
            userRole = new List<SelectListItem>();
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var ID = "";
            string role;
            List<Users> user = new List<Users>();
            var appUser = await _context.ApplicationUser.ToListAsync();

            foreach (var Data in appUser)
            {
                ID = Data.Id;
                userRole = await _usersRole.getRole(_userManager, _roleManager, ID);

                user.Add(new Users()
                {
                    Id = Data.Id,
                    UserName = Data.UserName,
                    PhoneNumber = Data.PhoneNumber,
                    Email = Data.Email,
                    Name = Data.Name,
                    LastName = Data.LastName,
                    SecondLastName = Data.SecondLastName,
                    Telephone = Data.Telephone,
                    Role = userRole[0].Text
                });
            }

            return View(user.ToList());

            //return View(await _context.ApplicationUser.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.ApplicationUser
                .SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        // GET: Users/Profile/5
        [AllowAnonymous]
        public async Task<IActionResult> Profile(string id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            var applicationUser = await _context.ApplicationUser
                .SingleOrDefaultAsync(m => m.Id == id);
            //if (applicationUser == null)
            //{
            //    return NotFound();
            //}

            return View(applicationUser);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,LastName,SecondLastName,Telephone,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(applicationUser);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(applicationUser);
        }

        public async Task<List<Users>> EditAjax(string id)
        {
           
            List<Users> user = new List<Users>();
            var appUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
            userRole = await _usersRole.getRole(_userManager, _roleManager, id);

            user.Add(new Users()
            {
                Id = appUser.Id,
                UserName = appUser.UserName,
                PhoneNumber = appUser.PhoneNumber,
                Email = appUser.Email,
                Role = userRole[0].Text,
                RoleId = userRole[0].Value,
                AccessFailedCount = appUser.AccessFailedCount,
                ConcurrencyStamp = appUser.ConcurrencyStamp,
                EmailConfirmed = appUser.EmailConfirmed,
                LockoutEnabled = appUser.LockoutEnabled,
                LockoutEnd = appUser.LockoutEnd,
                NormalizedEmail = appUser.NormalizedEmail,
                NormalizedUserName = appUser.NormalizedUserName,
                PasswordHash = appUser.PasswordHash,
                PhoneNumberConfirmed = appUser.PhoneNumberConfirmed,
                SecurityStamp = appUser.SecurityStamp,
                TwoFactorEnabled = appUser.TwoFactorEnabled,
                Name = appUser.Name,
                LastName = appUser.LastName,
                SecondLastName = appUser.SecondLastName,
                Telephone = appUser.Telephone,
            });

            return user;

            /*
             * LEGACY CODE - KEPT FOR FUTURE REFERENCES
             */

            //List<ApplicationUser> user = new List<ApplicationUser>();
            //var appUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
            //user.Add(appUser);
        }

        [HttpPost]
        public async Task<String> EditUserAjax(string id, string email, string phoneNumber, string userName,
            int accessFailedCount, string concurrencyStamp, bool emailConfirmed, bool lockoutEnabled, DateTimeOffset lockoutEnd,
            string normalizedEmail, string normalizedUserName, string passwordHash, bool phoneNumberConfirmed, string securityStamp,
            bool twoFactorEnabled, string name, string lastName, string secondLastName, string telephone, string selectRole, ApplicationUser applicationUser)
        {
            applicationUser = new ApplicationUser
            {
                Id = id,
                Email = email,
                PhoneNumber = phoneNumber,
                UserName = userName,
                AccessFailedCount = accessFailedCount,
                ConcurrencyStamp = concurrencyStamp,
                EmailConfirmed = emailConfirmed,
                LockoutEnabled = lockoutEnabled,
                LockoutEnd = lockoutEnd,
                NormalizedEmail = normalizedEmail,
                NormalizedUserName = normalizedUserName,
                PasswordHash = passwordHash,
                PhoneNumberConfirmed = phoneNumberConfirmed,
                SecurityStamp = securityStamp,
                TwoFactorEnabled = twoFactorEnabled,
                Name = name,
                LastName = lastName,
                SecondLastName = secondLastName,
                Telephone = telephone
            };

            _context.Update(applicationUser);
            await _context.SaveChangesAsync();

            if (selectRole != "No role")
            {
                var user = await _userManager.FindByIdAsync(id);
                userRole = await _usersRole.getRole(_userManager, _roleManager, id);
                if (userRole[0].Text != "No role")
                {
                    await _userManager.RemoveFromRoleAsync(user, userRole[0].Text);
                }

                if (selectRole == "No role")
                {
                    selectRole = "User";
                }

                var result = await _userManager.AddToRoleAsync(user, selectRole);

            }

            return "Save";
        }

        public async Task<List<SelectListItem>> RolesAjax()
        {
            List<SelectListItem> rolesList = new List<SelectListItem>();
            rolesList = _usersRole.getRoles(_roleManager);
            return rolesList;
        }


        // GET: Users/Edit/5
        //public async Task<IActionResult> Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
        //    if (applicationUser == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(applicationUser);
        //}

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Name,LastName,SecondLastName,Telephone,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] ApplicationUser applicationUser)
        {
            if (id != applicationUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicationUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserExists(applicationUser.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(applicationUser);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.ApplicationUser
                .SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
            _context.ApplicationUser.Remove(applicationUser);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ApplicationUserExists(string id)
        {
            return _context.ApplicationUser.Any(e => e.Id == id);
        }
    }
}
