using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using system_core_with_authentication.Data;
using Microsoft.EntityFrameworkCore;
using system_core_with_authentication.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using system_core_with_authentication.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace system_core_with_authentication.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            MedicamentLowHighViewModel vhvm = new MedicamentLowHighViewModel();
            var y = _context.MedicamentsBelowThreshold.Join(
                _context.Medicaments,
                mbt => mbt.MedicamentId,
                med => med.Id,
                (mbt, med) => new { mbt, med }
               );
            y.ToList().OrderByDescending(m => m.mbt.CurrentStock / m.med.MinimumStock * 100);

            List<Medicament> list = new List<Medicament>();
            foreach (var item in y)
            {
                list.Add(_context.Medicaments.Where(a => a.Id == item.med.Id).FirstOrDefault());
            }
            var list2 = list.Distinct();
            vhvm.MedicamentHigh = list2.ToList();

            //-----
            //ViewBag.a = new SelectList(list, "Description", "Content", "Type","Price");

            //Most requested
            var x = _context.Medicaments.ToList().OrderByDescending(m => m.Counter).Take(10);
            vhvm.MedicamentLow = x.ToList();
            return View(vhvm);
        }

        public IActionResult LowStock()
        {
            //MBT.currentStock / medicament.Where(a.ID=mtb.Id)minimumStock *100

            //var y = _context.MedicamentsBelowThreshold.Where(a=>a.MedicamentId==);

            var x = _context.MedicamentsBelowThreshold.Join(
                _context.Medicaments,
                mbt => mbt.MedicamentId,
                med => med.Id,
                (mbt,med) => new { mbt,med}
               );
            x.ToList().OrderByDescending(m=>m.mbt.CurrentStock/m.med.MinimumStock*100);

            List<Medicament> list = new List<Medicament>();
            foreach (var item in x)
            {
                list.Add(_context.Medicaments.Where(a => a.Id==item.med.Id).FirstOrDefault());
            }
            //var list = _context.MedicamentsBelowThreshold.OrderBy(a => a.CurrentStock).ToList();
            return View(list.Distinct());
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
