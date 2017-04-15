using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using system_core_with_authentication.Data;
using system_core_with_authentication.Models;

namespace system_core_with_authentication.Controllers
{
    public class RepositionStockDetailedsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RepositionStockDetailedsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: RepositionStockDetaileds
        public async Task<IActionResult> Index()
        {
            return View(await _context.RepositionStockDetailed.ToListAsync());
        }

        // GET: RepositionStockDetaileds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repositionStockDetailed = await _context.RepositionStockDetailed
                .SingleOrDefaultAsync(m => m.Id == id);
            if (repositionStockDetailed == null)
            {
                return NotFound();
            }

            return View(repositionStockDetailed);
        }

        // GET: RepositionStockDetaileds/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RepositionStockDetaileds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdRepositionStock,IdMedicament,CurrentStock,RequestStock")] RepositionStockDetailed repositionStockDetailed)
        {
            if (ModelState.IsValid)
            {
                _context.Add(repositionStockDetailed);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(repositionStockDetailed);
        }

        // GET: RepositionStockDetaileds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repositionStockDetailed = await _context.RepositionStockDetailed.SingleOrDefaultAsync(m => m.Id == id);
            if (repositionStockDetailed == null)
            {
                return NotFound();
            }
            return View(repositionStockDetailed);
        }

        // POST: RepositionStockDetaileds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdRepositionStock,IdMedicament,CurrentStock,RequestStock")] RepositionStockDetailed repositionStockDetailed)
        {
            if (id != repositionStockDetailed.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(repositionStockDetailed);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RepositionStockDetailedExists(repositionStockDetailed.Id))
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
            return View(repositionStockDetailed);
        }

        // GET: RepositionStockDetaileds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repositionStockDetailed = await _context.RepositionStockDetailed
                .SingleOrDefaultAsync(m => m.Id == id);
            if (repositionStockDetailed == null)
            {
                return NotFound();
            }

            return View(repositionStockDetailed);
        }

        // POST: RepositionStockDetaileds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var repositionStockDetailed = await _context.RepositionStockDetailed.SingleOrDefaultAsync(m => m.Id == id);
            _context.RepositionStockDetailed.Remove(repositionStockDetailed);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool RepositionStockDetailedExists(int id)
        {
            return _context.RepositionStockDetailed.Any(e => e.Id == id);
        }
    }
}
