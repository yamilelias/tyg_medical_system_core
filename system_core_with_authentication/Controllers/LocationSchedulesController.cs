using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using system_core_with_authentication.Data;
using system_core_with_authentication.Models;
using system_core_with_authentication.Models.ViewModels;

namespace system_core_with_authentication.Controllers
{
    public class LocationSchedulesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LocationSchedulesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: LocationSchedules
        public async Task<IActionResult> Index()
        {
            return View(await _context.LocationSchedules.Include(a=>a.User).Include(b=>b.Location).ToListAsync());
        }

        // GET: LocationSchedules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locationSchedule = await _context.LocationSchedules
                .SingleOrDefaultAsync(m => m.Id == id);
            if (locationSchedule == null)
            {
                return NotFound();
            }

            return View(locationSchedule);
        }

        // GET: LocationSchedules/Create
        public IActionResult Create(string id)
        {
            var selectedUser = _context.ApplicationUser.Where(m => m.Id.Equals(id)).FirstOrDefault();

            var SelectListu = new SelectList(_context.ApplicationUser, "Id", "Email");
            ViewData["Location"] = new SelectList(_context.Locations, "Id", "Name");

            foreach (var user in SelectListu)
            {
                if (user.Value.Equals(selectedUser.Id))
                {
                    user.Selected = true;
                }
            }

            ViewData["User"] = SelectListu;

            return View();
        }

        // POST: LocationSchedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday")] LocationSchedule locationSchedule, string location, string user)
        {
            if (ModelState.IsValid)
            {

                locationSchedule.Location = _context.Locations.Where(a => a.Id == Int32.Parse(location)).FirstOrDefault();
                locationSchedule.User = _context.ApplicationUser.Where(a => a.Id.Equals(user)).FirstOrDefault();

                var userId = locationSchedule.User.Id;

                _context.Add(locationSchedule);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Users", new {id = userId });

            }
            return View(locationSchedule);
        }

        // GET: LocationSchedules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //ViewData["Location"] = new SelectList(_context.Locations, "Id", "Name");    
            EditLocationScheduleViewModel vm = new EditLocationScheduleViewModel();

            var locationSchedule = await _context.LocationSchedules.Include(m => m.Location).SingleOrDefaultAsync(m => m.Id == id);
            vm.ls = locationSchedule;

            var listOfLocations = _context.Locations.Select(m => m.Name).ToList();
            vm.Locations = listOfLocations;

            if (locationSchedule == null)
            {
                return NotFound();
            }
            return View(vm);
        }

        // POST: LocationSchedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Location,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday")] LocationSchedule locationSchedule, string location)
        {
            if (id != locationSchedule.Id)
            {
                return NotFound();
            }

            var userId = _context.LocationSchedules.Where(m => m.Id == locationSchedule.Id).Include(m => m.User).Select(m => m.User.Id).FirstOrDefault();

            if (ModelState.IsValid)
            {
                try
                {
                    locationSchedule.Location = _context.Locations.Where(a => a.Name.Equals(location)).FirstOrDefault();

                    _context.Update(locationSchedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationScheduleExists(locationSchedule.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Users", new { id = userId });
            }
            return View(locationSchedule);
        }

        // GET: LocationSchedules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locationSchedule = await _context.LocationSchedules
                .SingleOrDefaultAsync(m => m.Id == id);
            if (locationSchedule == null)
            {
                return NotFound();
            }

            return View(locationSchedule);
        }

        // POST: LocationSchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var locationSchedule = await _context.LocationSchedules.SingleOrDefaultAsync(m => m.Id == id);

            var userId = _context.LocationSchedules.Where(m => m.Id == locationSchedule.Id).Include(m => m.User).Select(m => m.User.Id).FirstOrDefault();

            _context.LocationSchedules.Remove(locationSchedule);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Users", new {id = userId });
        }

        private bool LocationScheduleExists(int id)
        {
            return _context.LocationSchedules.Any(e => e.Id == id);
        }
    }
}
