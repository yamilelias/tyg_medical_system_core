using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using system_core_with_authentication;
using system_core_with_authentication.Models;
using Treshold_Mail;

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
            return View(await _context.LocationSchedules.ToListAsync());
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: LocationSchedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdUser,IdLocation,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday")] LocationSchedule locationSchedule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(locationSchedule);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
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

            var locationSchedule = await _context.LocationSchedules.SingleOrDefaultAsync(m => m.Id == id);
            if (locationSchedule == null)
            {
                return NotFound();
            }
            return View(locationSchedule);
        }

        // POST: LocationSchedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdUser,IdLocation,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday")] LocationSchedule locationSchedule)
        {
            if (id != locationSchedule.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
                return RedirectToAction("Index");
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
            _context.LocationSchedules.Remove(locationSchedule);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool LocationScheduleExists(int id)
        {
            return _context.LocationSchedules.Any(e => e.Id == id);
        }
    }
}
