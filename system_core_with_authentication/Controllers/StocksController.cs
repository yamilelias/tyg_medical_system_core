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
     * The StocksController is in charge of managing
     * all  actions related to the creation, edition, and 
     * deletion of Stocks, as well validating if the
     * medicaments are below their threshold.
     * 
     * @author  Dilan Coss
     * @version 1.0
     */
    [Authorize(Roles = "Admin,Supervisor")]
    public class StocksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StocksController(ApplicationDbContext context)
        {
            _context = context;    
        }

        /*
         * This method gathers from the database all
         * the stocks and returns a list of them
         * 
         * @param   unused
         * @return  Index View with Stocks list
         */
        // GET: Stocks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Stocks.Include(s => s.Medicament);
            return View(await applicationDbContext.ToListAsync());
        }

        /*
         * This method gathers all the information of a specific
         * stock searched with the id given
         * 
         * @param   int id - Stock's id
         * @return  Details View with one stock
         */
        // GET: Stocks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _context.Stocks
                .Include(s => s.Medicament)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (stock == null)
            {
                return NotFound();
            }

            return View(stock);
        }

        /*
         * This method returns the Create View
         * 
         * @param   unused
         * @return  Create View
         */
        // GET: Stocks/Create
        public IActionResult Create()
        {
            ViewData["MedicamentId"] = new SelectList(_context.Medicaments, "Id", "Description");
            return View();
        }

        /*
         * This method creates a stock with the parameters passed
         * 
         * 
         * @param   Stock - Object with the parameters
         * @return  Index View if success, otherwise Create View
         */
        // POST: Stocks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Total,Expiration,MedicamentId")] Stock stock)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stock);
                await _context.SaveChangesAsync();

                // Check Minimum stock
                checkMedicamentBelowThreshold(stock);

                return RedirectToAction("Index");
            }
            ViewData["MedicamentId"] = new SelectList(_context.Medicaments, "Id", "Description", stock.MedicamentId);
            return View(stock);
        }

        /*
         * This method returns the Edit View
         * 
         * 
         * @param   Id - Stock's Id
         * @return  Edit View with a stock
         */
        // GET: Stocks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _context.Stocks.SingleOrDefaultAsync(m => m.Id == id);
            if (stock == null)
            {
                return NotFound();
            }
            ViewData["MedicamentId"] = new SelectList(_context.Medicaments, "Id", "Description", stock.MedicamentId);
            return View(stock);
        }

        /*
         * This method modifies a stock with the parameters passed
         * 
         * 
         * @param   Stock - Object with the parameters
         * @return  Index View if success, Edit Create View
         */
        // POST: Stocks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Total,Expiration,MedicamentId")] Stock stock)
        {
            if (id != stock.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stock);
                    await _context.SaveChangesAsync();
                    // Check Minimum stock
                    // var IsBelowTreshold = _context.MedicamentsBelowThreshold.Any(e => e.MedicamentId == stock.MedicamentId);
                    checkMedicamentBelowThreshold(stock);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockExists(stock.Id))
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
            ViewData["MedicamentId"] = new SelectList(_context.Medicaments, "Id", "Description", stock.MedicamentId);
            return View(stock);
        }

        /*
         * This method finds the stock in order to delete it
         * 
         * 
         * @param   int id - Stock's id
         * @return  Delete View with the specifit stock
         */
        // GET: Stocks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _context.Stocks
                .Include(s => s.Medicament)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (stock == null)
            {
                return NotFound();
            }

            return View(stock);
        }

        /*
         * This method deletes a stock
         * 
         * 
         * @param   int id - Stock's Id
         * @return  Index View
         */
        // POST: Stocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stock = await _context.Stocks.SingleOrDefaultAsync(m => m.Id == id);
            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();
            // Check Minimum stock
            // var IsBelowTreshold = _context.MedicamentsBelowThreshold.Any(e => e.MedicamentId == stock.MedicamentId);
            checkMedicamentBelowThreshold(stock);

            return RedirectToAction("Index");
        }

        /*
         * Checks if a stock exists
         * 
         * 
         * @param   id - Stock's id
         * @return  boolean
         */
        private bool StockExists(int id)
        {
            return _context.Stocks.Any(e => e.Id == id);
        }

        /*
         * This method checks if one of the other methods
         * change the medicament condition of below threshold
         * It can add it or remove from BelowThreshold Table
         * 
         * 
         * @param   Stock - Object with the parameters
         * @return  nothing
         */
        public void checkMedicamentBelowThreshold(Stock stock)
        {
            if (_context.MedicamentsBelowThreshold.Any(e => e.MedicamentId == stock.MedicamentId))
            {
                var sum = _context.Stocks.Where(e => e.MedicamentId == stock.MedicamentId)
                                            .Sum(e => e.Total);

                if (sum >= _context.Medicaments.Where(e => e.Id == stock.MedicamentId).Select(e => e.MinimumStock).FirstOrDefault())
                {
                    var toRemove = _context.MedicamentsBelowThreshold.FirstOrDefault(m => m.MedicamentId == stock.MedicamentId);
                    _context.MedicamentsBelowThreshold.Remove(toRemove);
                    _context.SaveChanges();

                }
            }
            else
            {
                var sum = _context.Stocks.Where(e => e.MedicamentId == stock.MedicamentId)
                                         .Sum(e => e.Total);
                var minStock = _context.Medicaments.Where(e => e.Id == stock.MedicamentId).Select(e => e.MinimumStock).FirstOrDefault();
                if (sum < minStock)
                {
                    MedicamentBelowThreshold toadd = new MedicamentBelowThreshold();
                    toadd.MedicamentId = _context.Medicaments.Where(a => a.Id == stock.MedicamentId).Select(a => a.Id).FirstOrDefault();
                    toadd.CurrentStock = sum;
                    _context.MedicamentsBelowThreshold.Add(toadd);
                    _context.SaveChanges();

                }
            }

        }
    }
}
