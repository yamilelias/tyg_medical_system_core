using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Treshold_Mail;
using system_core_with_authentication.Models;
using system_core_with_authentication.Data;
using system_core_with_authentication.Models.ViewModels;

namespace Treshold_Mail.Controllers
{
    public class MedicamentBelowThresholdsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MedicamentBelowThresholdsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: MedicamentBelowThresholds
        public IActionResult Index()
        {
            var m = _context.MedicamentsBelowThreshold.ToList();
            List<MedicamentTresholdViewModel> meds = new List<MedicamentTresholdViewModel>();
            m.ForEach(e => {
                meds.Add(new MedicamentTresholdViewModel() {
                    Id = e.Id,
                    Name = _context.Medicaments.Where(f => f.Id == e.MedicamentId)
                                               .Select(f => f.Description)
                                                .FirstOrDefault(),
                    CurrentStock = e.CurrentStock
                });
            });

            return View(meds);
        }


        // GET: MedicamentBelowThresholds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var medicamentBelowThreshold = await _context.MedicamentsBelowThreshold
                .SingleOrDefaultAsync(m => m.Id == id);
            if (medicamentBelowThreshold == null)
            {
                return NotFound();
            }
            var medicamentViewModel = new MedicamentTresholdViewModel() {
                Id = medicamentBelowThreshold.Id,
                Name = _context.Medicaments.Where(f => f.Id == medicamentBelowThreshold.MedicamentId)
                                               .Select(f => f.Description)
                                                .FirstOrDefault(),
                CurrentStock = medicamentBelowThreshold.CurrentStock
            };
            return View(medicamentViewModel);
        }

        // POST: MedicamentBelowThresholds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicamentBelowThreshold = await _context.MedicamentsBelowThreshold.SingleOrDefaultAsync(m => m.Id == id);
            _context.MedicamentsBelowThreshold.Remove(medicamentBelowThreshold);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool MedicamentBelowThresholdExists(int id)
        {
            return _context.MedicamentsBelowThreshold.Any(e => e.Id == id);
        }
    }
}
