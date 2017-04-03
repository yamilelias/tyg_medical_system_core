using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TYG_Medical_System_Core.Models;

namespace TYG_Medical_System_Core.Controllers
{
    public class MedicineItemsController : Controller
    {
        private readonly TYG_Medical_System_CoreContext _context;

        public MedicineItemsController(TYG_Medical_System_CoreContext context)
        {
            _context = context;    
        }

        // GET: MedicineItems
        public async Task<IActionResult> Index()
        {
            return View(await _context.MedicineItem.ToListAsync());
        }

        // GET: MedicineItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicineItem = await _context.MedicineItem
                .SingleOrDefaultAsync(m => m.Id == id);
            if (medicineItem == null)
            {
                return NotFound();
            }

            return View(medicineItem);
        }

        // GET: MedicineItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MedicineItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,Serial,Note,MaintenanceDate,Image")] MedicineItem medicineItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medicineItem);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(medicineItem);
        }

        // GET: MedicineItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicineItem = await _context.MedicineItem.SingleOrDefaultAsync(m => m.Id == id);
            if (medicineItem == null)
            {
                return NotFound();
            }
            return View(medicineItem);
        }

        // POST: MedicineItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Serial,Note,MaintenanceDate,Image")] MedicineItem medicineItem)
        {
            if (id != medicineItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicineItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicineItemExists(medicineItem.Id))
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
            return View(medicineItem);
        }

        // GET: MedicineItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicineItem = await _context.MedicineItem
                .SingleOrDefaultAsync(m => m.Id == id);
            if (medicineItem == null)
            {
                return NotFound();
            }

            return View(medicineItem);
        }

        // POST: MedicineItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicineItem = await _context.MedicineItem.SingleOrDefaultAsync(m => m.Id == id);
            _context.MedicineItem.Remove(medicineItem);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool MedicineItemExists(int id)
        {
            return _context.MedicineItem.Any(e => e.Id == id);
        }
    }
}
