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

            List<MedicamentWithTotalStock> list = new List<MedicamentWithTotalStock>();
            foreach (var item in y)
            {
                MedicamentWithTotalStock m = new MedicamentWithTotalStock();
                m.medicament = _context.Medicaments.Where(a => a.Id == item.med.Id).FirstOrDefault();
                m.sumTotal = _context.Stocks.Where(f => f.MedicamentId == item.med.Id)
                                              .Sum(f => f.Total);
                list.Add(m);
            }
            var list2 = list.Take(10).Distinct();

            vhvm.MedicamentLow = list2.ToList();




            //Most requested
            var x = _context.Medicaments.ToList().OrderByDescending(m => m.Counter).Take(10);
            vhvm.MedicamentHigh = x.ToList();
            return View(vhvm);
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
