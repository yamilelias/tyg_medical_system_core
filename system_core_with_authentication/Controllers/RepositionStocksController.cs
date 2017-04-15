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
    public class RepositionStocksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RepositionStocksController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: RepositionStocks
        public async Task<IActionResult> Index()
        {
            return View(await _context.RepositionStocks.ToListAsync());
        }

        // GET: RepositionStocks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repositionStock = await _context.RepositionStocks
                .SingleOrDefaultAsync(m => m.Id == id);
            if (repositionStock == null)
            {
                return NotFound();
            }

            return View(repositionStock);
        }

        // GET: RepositionStocks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RepositionStocks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdRequest,IdLocation,Solved")] RepositionStock repositionStock)
        {
            if (ModelState.IsValid)
            {
                _context.Add(repositionStock);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(repositionStock);
        }

        // GET: RepositionStocks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repositionStock = await _context.RepositionStocks.SingleOrDefaultAsync(m => m.Id == id);
            if (repositionStock == null)
            {
                return NotFound();
            }
            return View(repositionStock);
        }

        // POST: RepositionStocks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdRequest,IdLocation,Solved")] RepositionStock repositionStock)
        {
            if (id != repositionStock.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(repositionStock);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RepositionStockExists(repositionStock.Id))
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
            return View(repositionStock);
        }

        // GET: RepositionStocks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repositionStock = await _context.RepositionStocks
                .SingleOrDefaultAsync(m => m.Id == id);
            if (repositionStock == null)
            {
                return NotFound();
            }

            return View(repositionStock);
        }

        // POST: RepositionStocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var repositionStock = await _context.RepositionStocks.SingleOrDefaultAsync(m => m.Id == id);
            _context.RepositionStocks.Remove(repositionStock);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool RepositionStockExists(int id)
        {
            return _context.RepositionStocks.Any(e => e.Id == id);
        }
    }
}
