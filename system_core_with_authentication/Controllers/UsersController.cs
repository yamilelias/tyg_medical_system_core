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
    /**
     * The UsersController is the controller in charge 
     * of managing all actions related to the creation,
     * edition, and deletion of ApplicationUsers, as well 
     * managing other related methods.
     * 
     * @author  Jonathan Torres
     * @version 1.0
     */

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

        /*
         * This method gathers the information of Users
         * and returns a list of Users that is displayed
         * in the Index view.
         * @param   Unused
         * @return  Index view with Users list
         */
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

        /*
         * This method displays the information of a single ApplicationUser,
         * making a query to the DB to find its assigned LocationSchedules,
         * and returns the Details view with the requested ApplicactionUser information
         * along with its assigned LocationSchedules.
         * @param   string id - This is the id of the single ApplicationUser
         * @return  Details view with ApplicationUser data and LocationSchedules
         */
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

        /*
         * This method displays the information of the ApplicationUser
         * that is currently signed in, making a query to the DB,
         * and returns the Profile view with the requested data.
         * @param   string id - This is the id of the ApplicationUser that is signed in
         * @return  Profile view with ApplicationUser data
         */
        // GET: Users/Profile/5
        [AllowAnonymous]
        public async Task<IActionResult> Profile(string id)
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

        /*
         * This GET method displays the form to create a User,
         * NOTE: This method is NOT used to created users in this project.
         * Instead, the POST method "Register" in the AccountController is used.
         * @param   unused
         * @return  Create view
         */
        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        /*
         * This GET method displays the form to create a User,
         * NOTE: This method is NOT used to created users in this project.
         * Instead, the POST method "Register" in the AccountController is used.
         * @return  Index view of Users
         */
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
                return RedirectToAction("Index", "Users");
            }
            return RedirectToAction("Index", "Users");
        }

        //public async Task<List<Users>> EditAjax(string id)
        //{

        //    List<Users> user = new List<Users>();
        //    var appUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
        //    userRole = await _usersRole.getRole(_userManager, _roleManager, id);

        //    user.Add(new Users()
        //    {
        //        Id = appUser.Id,
        //        UserName = appUser.UserName,
        //        PhoneNumber = appUser.PhoneNumber,
        //        Email = appUser.Email,
        //        Role = userRole[0].Text,
        //        RoleId = userRole[0].Value,
        //        AccessFailedCount = appUser.AccessFailedCount,
        //        ConcurrencyStamp = appUser.ConcurrencyStamp,
        //        EmailConfirmed = appUser.EmailConfirmed,
        //        LockoutEnabled = appUser.LockoutEnabled,
        //        LockoutEnd = appUser.LockoutEnd,
        //        NormalizedEmail = appUser.NormalizedEmail,
        //        NormalizedUserName = appUser.NormalizedUserName,
        //        PasswordHash = appUser.PasswordHash,
        //        PhoneNumberConfirmed = appUser.PhoneNumberConfirmed,
        //        SecurityStamp = appUser.SecurityStamp,
        //        TwoFactorEnabled = appUser.TwoFactorEnabled,
        //        Name = appUser.Name,
        //        LastName = appUser.LastName,
        //        SecondLastName = appUser.SecondLastName,
        //        Telephone = appUser.Telephone,
        //    });

        //    return user;
        //}

        //[HttpPost]
        //public async Task<String> EditUserAjax(string id, string email, string phoneNumber, string userName,
        //    int accessFailedCount, string concurrencyStamp, bool emailConfirmed, bool lockoutEnabled, DateTimeOffset lockoutEnd,
        //    string normalizedEmail, string normalizedUserName, string passwordHash, bool phoneNumberConfirmed, string securityStamp,
        //    bool twoFactorEnabled, string name, string lastName, string secondLastName, string telephone, string selectRole, ApplicationUser applicationUser)
        //{
        //    applicationUser = new ApplicationUser
        //    {
        //        Id = id,
        //        Email = email,
        //        PhoneNumber = phoneNumber,
        //        UserName = userName,
        //        AccessFailedCount = accessFailedCount,
        //        ConcurrencyStamp = concurrencyStamp,
        //        EmailConfirmed = emailConfirmed,
        //        LockoutEnabled = lockoutEnabled,
        //        LockoutEnd = lockoutEnd,
        //        NormalizedEmail = normalizedEmail,
        //        NormalizedUserName = normalizedUserName,
        //        PasswordHash = passwordHash,
        //        PhoneNumberConfirmed = phoneNumberConfirmed,
        //        SecurityStamp = securityStamp,
        //        TwoFactorEnabled = twoFactorEnabled,
        //        Name = name,
        //        LastName = lastName,
        //        SecondLastName = secondLastName,
        //        Telephone = telephone
        //    };

        //    _context.Update(applicationUser);
        //    await _context.SaveChangesAsync();

        //    if (selectRole != "No role")
        //    {
        //        var user = await _userManager.FindByIdAsync(id);
        //        userRole = await _usersRole.getRole(_userManager, _roleManager, id);
        //        if (userRole[0].Text != "No role")
        //        {
        //            await _userManager.RemoveFromRoleAsync(user, userRole[0].Text);
        //        }

        //        if (selectRole == "No role")
        //        {
        //            selectRole = "User";
        //        }

        //        var result = await _userManager.AddToRoleAsync(user, selectRole);

        //    }

        //    return "Save";
        //}

        //public async Task<List<SelectListItem>> RolesAjax()
        //{
        //    List<SelectListItem> rolesList = new List<SelectListItem>();
        //    rolesList = _usersRole.getRoles(_roleManager);
        //    return rolesList;
        //}


        // GET: Users/Edit/5



         /*
         * This method displays the information of the ApplicationUser
         * in an Edit form, gathering its information and its role 
         * through two queries and creating a ViewModel to display the ApplicationUser role.
         * Roles are gathered from the Roles table and passed on to the Edit view
         * in a SelectBox.
         * @param   string id - This is the id of the single ApplicationUser
         * @return  Edit view with ApplicationUser data and its role
         */
        public async Task<IActionResult> Edit(string id)
        {
            var applicationUser = await _context.ApplicationUser
                .SingleOrDefaultAsync(m => m.Id == id);

            EditUserViewModel vm = new EditUserViewModel();
            vm.appUser = applicationUser;

            var x = _context.Roles.Select(m => new {m.Id, m.Name}).ToList();
            var y = _context.UserRoles.Where(m => m.UserId == applicationUser.Id).Select(n => n.RoleId).FirstOrDefault();


            foreach (var item in x)
            {
                if (y == item.Id)
                    vm.role = item.Name;
            }

            return View(vm);
        }

        /*
         * This method gathers the data provided by the user to edit
         * the selected ApplicationUser. If a new image is not uploaded,
         * the image will not be replaced. Likewise, if a new role is not selected,
         * the ApplicationUser's role will not be replaced. An DBConcurrencyException will ALWAYS occur;
         * this exception is caught and dealt with to proceed with the correct edition of the ApplicationUser data.
         * Afterwards, the new data replaces the old ApplicationUser data.
         * @param   string id - This is the id of the single ApplicationUser
         * @param   string name - This is the name of the single ApplicationUser
         * @param   string lastName - This is the lastName of the single ApplicationUser
         * @param   string secondLastName - This is the secondLastName of the single ApplicationUser
         * @param   string telephone - This is the telephone of the single ApplicationUser
         * @param   string email - This is the email of the single ApplicationUser
         * @param   string userName - This is the user name of the single ApplicationUser
         * @param   IFormFile - This is the photo file of the single ApplicationUser
         * @param   EditUserViewModel vm - This is the view model that groups the new ApplicationUser data and its role
         * @return  Index view with ApplicationUser data and its role
         */
        //POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, string name, string lastName, string secondLastName, string telephone, string email, string userName, IFormFile imageFile, EditUserViewModel vm)
        {
            //if (id != userNew.Id)
            //{
            //    return NotFound();
            //}

            ApplicationUser userNew = _context.ApplicationUser.Where(m => m.Id == id).FirstOrDefault();

            userNew.Name = name;
            userNew.LastName = lastName;
            userNew.SecondLastName = secondLastName;
            userNew.Telephone = telephone;
            userNew.Email = email;
            userNew.UserName = userName;

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

                       userNew.UserImage = fileName;

                    }
                    else
                    {
                        ApplicationUser userWithImage = new ApplicationUser();
                        userWithImage = userNew;
                        userWithImage.UserImage = _context.ApplicationUser.Where(a => a.Id == userNew.Id).Select(a => a.UserImage).FirstOrDefault();
                    }

                    var oldRole = _context.UserRoles.Where(m => m.UserId.Equals(userNew.Id)).FirstOrDefault();

                    var newRole = _context.Roles.Where(m => m.Name.Equals(vm.role)).Select(m => m.Id).FirstOrDefault();

                    var oldRoleName = _context.Roles.Where(m => m.Id.Equals(oldRole.RoleId)).Select(m => m.Name).FirstOrDefault();

                    await _userManager.RemoveFromRoleAsync(userNew, oldRoleName);
                    await _userManager.AddToRoleAsync(userNew, vm.role);

                    _context.Update(userNew);
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
            return View(userNew);
        }

        /*
         * This method displays the information of the ApplicationUser in a Delete view.
         * Please not that the default admin account, which is hardcoded into the system,
         * CANNOT be erased by evaluating its hardcoded ID, which is 1.
         * @param   string id - This is the id of the single ApplicationUser
         * @return  Delete view with ApplicationUser data
         */
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

        /*
         * This method deletes the selected ApplicationUser from the database.
         * Before deletion, all LocationSchedules tied to the ApplicationUser are deleted
         * to avoid a DBException. Afterwards, the ApplicationUser is deleted from the database.
         * @param   string id - This is the id of the single ApplicationUser
         * @return  Index view with ApplicationUser data and its role
         */
        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {

            var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);

            var schedules = _context.LocationSchedules.Where(m => m.User.Email.Equals(applicationUser.Email)).ToList();

            foreach (var item in schedules)
            {
                _context.LocationSchedules.Remove(item);
            }

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
