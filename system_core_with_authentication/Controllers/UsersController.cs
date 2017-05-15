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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using system_core_with_authentication.Models.ViewModels;

namespace system_core_with_authentication.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _environment;

        UserManager<ApplicationUser> _userManager;
        RoleManager<IdentityRole> _roleManager;
        UsersRoles _usersRole;

        public List<SelectListItem> userRole;

        public UsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, IHostingEnvironment environment) 
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _usersRole = new UsersRoles();
            _environment = environment;

            userRole = new List<SelectListItem>();
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var ID = "";
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
                    Role = userRole[0].Text,
                    UserImage = Data.UserImage
                });
            }

            return View(user.ToList());
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

            DetailsUserWithLocationViewModel duwlm = new DetailsUserWithLocationViewModel();
            duwlm.user = applicationUser;
            duwlm.ls = _context.LocationSchedules.Where(a => a.User.Equals(applicationUser.Id)).Include(b=>b.Location).ToList();

            return View(duwlm);
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
        public async Task<IActionResult> Create([Bind("Name,LastName,SecondLastName,Telephone,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount,ImageFile")] ApplicationUser applicationUser, IFormFile imageFile)
        {

            if (ModelState.IsValid)
            {

                ApplicationUser currentUser = await _userManager.GetUserAsync(User);

                string pathID = currentUser.Id + "";

                if (imageFile != null)
                {
                    string uploadPath = Path.Combine(_environment.WebRootPath, "users", "uploads");
                    Directory.CreateDirectory(Path.Combine(uploadPath, pathID));

                    string fileName = Path.GetFileName(imageFile.FileName);

                    using (FileStream fs = new FileStream(Path.Combine(uploadPath, fileName), FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fs);
                    }

                    applicationUser.UserImage = fileName;

                }

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
        public async Task<IActionResult> Edit(string id)
        {
            var applicationUser = await _context.ApplicationUser
                .SingleOrDefaultAsync(m => m.Id == id);

            return View(applicationUser);
        }

        //POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Name,LastName,SecondLastName,Telephone,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount,ImageFile")] ApplicationUser applicationUser, IFormFile imageFile)
        {
            if (id != applicationUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                try
                {
                    if (imageFile != null)
                    {
                        string uploadPath = Path.Combine(_environment.WebRootPath, "users", "uploads");
                        Directory.CreateDirectory(Path.Combine(uploadPath));

                        string fileName = Path.GetFileName(imageFile.FileName);

                        using (FileStream fs = new FileStream(Path.Combine(uploadPath, fileName), FileMode.Create))
                        {
                            await imageFile.CopyToAsync(fs);
                        }

                        applicationUser.UserImage = fileName;

                    }
                    else
                    {
                        ApplicationUser userWithImage = new ApplicationUser();
                        userWithImage = applicationUser;
                        userWithImage.UserImage = _context.ApplicationUser.Where(a => a.Id == applicationUser.Id).Select(a => a.UserImage).FirstOrDefault();
                    }

                    _context.Update(applicationUser);
                    await _context.SaveChangesAsync();


                }
                catch (DbUpdateConcurrencyException ex)
                {
                   
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.Entity is ApplicationUser)
                        {
                            var dbEntity = _context.ApplicationUser.AsNoTracking().Single(p => p.Id == ((ApplicationUser)entry.Entity).Id);
                            var dbEntry = _context.Entry(dbEntity);

                            foreach (var property in entry.Metadata.GetProperties())
                            {

                                var proposedValue = entry.Property(property.Name).CurrentValue;
                                var originalValue = entry.Property(property.Name).OriginalValue;
                                var databaseValue = dbEntry.Property(property.Name).CurrentValue;

                                entry.Property(property.Name).OriginalValue = dbEntry.Property(property.Name).CurrentValue;

                            }

                        } else
                        {
                            throw new NotSupportedException("Don't know how to handle concurrency conflicts for " + entry.Metadata.Name);
                        }
                    }

                    _context.SaveChanges();

                }

                return RedirectToAction("Index");
            }
            return View(applicationUser);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if ((id == null) || (id == "1"))
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
