using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using system_core_with_authentication.Data;
using system_core_with_authentication.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace system_core_with_authentication.Controllers
{
    [Authorize(Roles = "Admin,Supervisor")]
    public class MedicamentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _environment;

        public MedicamentsController(ApplicationDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Medicaments
        public async Task<IActionResult> Index()
        {
            return View(await _context.Medicaments.ToListAsync());
        }

        // GET: Medicaments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicament = await _context.Medicaments
                .SingleOrDefaultAsync(m => m.Id == id);
            if (medicament == null)
            {
                return NotFound();
            }

            return View(medicament);
        }

        // GET: Medicaments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Medicaments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,Content,Type,Price,Priority,Counter,MinimumStock,MedicamentImage,ImageFile")] Medicament medicament, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {

                if (imageFile != null)
                {
                    string uploadPath = Path.Combine(_environment.WebRootPath, "images", "uploads");
                    Directory.CreateDirectory(Path.Combine(uploadPath));

                    string fileName = Path.GetFileName(imageFile.FileName);

                    using (FileStream fs = new FileStream(Path.Combine(uploadPath, fileName), FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fs);
                    }

                    medicament.MedicamentImage = fileName;

                }

                _context.Add(medicament);
                await _context.SaveChangesAsync();
                checkMedicamentBelowThershold(medicament);
                return RedirectToAction("Index");
            }
            return View(medicament);
        }

        // GET: Medicaments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicament = await _context.Medicaments.SingleOrDefaultAsync(m => m.Id == id);
            if (medicament == null)
            {
                return NotFound();
            }
            return View(medicament);
        }

        // POST: Medicaments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Content,Type,Price,Priority,Counter,MinimumStock,MedicamentImage,ImageFile")] Medicament medicament, IFormFile imageFile)
        {
            if (id != medicament.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                try
                {
                    if (imageFile != null)
                    {
                        string uploadPath = Path.Combine(_environment.WebRootPath, "images", "uploads");
                        Directory.CreateDirectory(Path.Combine(uploadPath));

                        string fileName = Path.GetFileName(imageFile.FileName);

                        Debug.WriteLine("Accepted image");

                        using (FileStream fs = new FileStream(Path.Combine(uploadPath, fileName), FileMode.Create))
                        {
                            await imageFile.CopyToAsync(fs);
                        }

                        medicament.MedicamentImage = fileName;

                    }

                    _context.Update(medicament);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicamentExists(medicament.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                checkMedicamentBelowThershold(medicament);
                return RedirectToAction("Index");
            }
            return View(medicament);
        }

        // GET: Medicaments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicament = await _context.Medicaments
                .SingleOrDefaultAsync(m => m.Id == id);
            if (medicament == null)
            {
                return NotFound();
            }

            return View(medicament);
        }

        // POST: Medicaments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicament = await _context.Medicaments.SingleOrDefaultAsync(m => m.Id == id);
            _context.Medicaments.Remove(medicament);
            await _context.SaveChangesAsync();
            checkMedicamentBelowThershold(medicament);
            return RedirectToAction("Index");
        }

        private bool MedicamentExists(int id)
        {
            return _context.Medicaments.Any(e => e.Id == id);
        }

        public void checkMedicamentBelowThershold(Medicament medicament)
        {
            if (_context.MedicamentsBelowThreshold.Any(e => e.MedicamentId == medicament.Id))
            {
                var sum = _context.Stocks.Where(e => e.MedicamentId == medicament.Id)
                                            .Sum(e => e.Total);

                if (sum >= _context.Medicaments.Where(e => e.Id == medicament.Id).Select(e => e.MinimumStock).FirstOrDefault())
                {
                    var toRemove = _context.MedicamentsBelowThreshold.FirstOrDefault(m => m.MedicamentId == medicament.Id);
                    _context.MedicamentsBelowThreshold.Remove(toRemove);
                    _context.SaveChanges();

                }
            }
            else
            {
                var sum = _context.Stocks.Where(e => e.MedicamentId == medicament.Id)
                                         .Sum(e => e.Total);
                var minStock = _context.Medicaments.Where(e => e.Id == medicament.Id).Select(e => e.MinimumStock).FirstOrDefault();
                if (sum < minStock)
                {
                    MedicamentBelowThreshold toadd = new MedicamentBelowThreshold();
                    toadd.MedicamentId = _context.Medicaments.Where(a => a.Id == medicament.Id).Select(a => a.Id).FirstOrDefault();
                    toadd.CurrentStock = sum;
                    _context.MedicamentsBelowThreshold.Add(toadd);
                    _context.SaveChanges();

                }
            }

        }
    }
}
