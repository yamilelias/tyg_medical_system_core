using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using system_core_with_authentication.Data;
using system_core_with_authentication.Models;
using Microsoft.AspNetCore.Authorization;

namespace system_core_with_authentication.Controllers
{
    /**
     * The LocationsController is the controller in charge 
     * of managing all locations and workplaces where ApplicationUsers work.
     * The controller handles all actions related to the creation, edition and deletion
     * of Locations.
     * 
     * @author  Jonathan Torres
     * @version 1.0
     */
    [Authorize(Roles = "Admin")]
    public class LocationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LocationsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        /*
         * This method displays the list of locations in the Index view of Locations.
         * @param   unused
         * @return  Index view with list of Locations
         */
        // GET: Locations
        public async Task<IActionResult> Index()
        {
            return View(await _context.Locations.ToListAsync());
        }

        /*
         * This method displays the details of the selected Location
         * in a Details view of Locations. The method gathers the data through a query
         * and passes it on to the Details view.

         * @param   int id - Id of the selected Location
         * @return  Details view of Locations with data of selected Location
         */
        // GET: Locations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Locations
                .SingleOrDefaultAsync(m => m.Id == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        /*
         * This GET method displays the Create view of Locations.
         * @param   unused
         * @return  Create view of Locations
         */
        // GET: Locations/Create
        public IActionResult Create()
        {
            return View();
        }

        /*
         * This POST method gathers all the information from the Create view
         * and creates a Location in the database, using the DB Context to do so.
         * Afterwards, the user is redirected to the Index view of Locations.
         * @param   Location location - Location object that will be created
         * @return  Index view with list of Locations
         */
        // POST: Locations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address,PhoneNumber")] Location location)
        {
            if (ModelState.IsValid)
            {
                _context.Add(location);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(location);
        }

        /*
         * This GET method displays the Edit view of Locations,
         * gathering the information of the selected Location with a query
         * and displaying it in the form of the Edit view.
         * @param   int id - Id of the selected Location
         * @return Edit view with a form filled with the selected Location data
         */
        // GET: Locations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Locations.SingleOrDefaultAsync(m => m.Id == id);
            if (location == null)
            {
                return NotFound();
            }
            return View(location);
        }

        /*
         * This POST method edits the selected Location, using the data gathered from
         * the form in the Edit view of Locations. The information is updated using the DB Context
         * and afterwards, the user is redirected to Index view of Locations.
         * @param   int id - Id of the selected Location
         * @param   Location location - Location object that will be edited
         * @return  Index view with list of Locations
         */
        // POST: Locations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address,PhoneNumber")] Location location)
        {
            if (id != location.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(location);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationExists(location.Id))
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
            return View(location);
        }

        /*
         * This method displays the Delete view of Locations with the selected Location view.
         * @param   int id - Id of the selected Location
         * @return  Delete view with information from the selected Location
         */
        // GET: Locations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Locations
                .SingleOrDefaultAsync(m => m.Id == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        /*
         * This method deletes the selected Location from the database.
         * Using the DB Context, the selected location is sought through a query,
         * as well as every LocationSchedule in which it is included.
         * After finding and deleting all LocationSchedules related to the Location,
         * the Location is deleted from the database.
         * @param   unused
         * @return  Index view with list of Locations
         */
        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var location = await _context.Locations.SingleOrDefaultAsync(m => m.Id == id);

            var schedules = _context.LocationSchedules.Where(m => m.Location.Id.Equals(id)).ToList();

            foreach (var item in schedules)
            {
                _context.LocationSchedules.Remove(item);
            }

            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool LocationExists(int id)
        {
            return _context.Locations.Any(e => e.Id == id);
        }
    }
}
