﻿using System;
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

            var y = _context.MedicamentsBelowThreshold.ToList();

            List<MedicamentWithTotalStock> list = new List<MedicamentWithTotalStock>();

            foreach (var item in y)
            {
                MedicamentWithTotalStock m = new MedicamentWithTotalStock();
                m.medicament = _context.Medicaments.Where(a => a.Id == item.MedicamentId).FirstOrDefault();
                m.sumTotal = _context.Stocks.Where(f => f.MedicamentId == item.MedicamentId)
                                              .Sum(f => f.Total);
                var minstock = m.medicament.MinimumStock;
                var sum = m.sumTotal;
                var percent = (double) sum / (double) minstock * 100;
                m.percentage = percent;
                m.budget = (m.medicament.MinimumStock - m.sumTotal) * m.medicament.Price;
                list.Add(m);
            }

            vhvm.MedicamentLow = list.OrderBy(a=>(Convert.ToDouble(a.sumTotal)/ Convert.ToDouble(a.medicament.MinimumStock))*100).Take(10).ToList();


            //Most requested
            var x = _context.Medicaments.ToList().OrderByDescending(m => m.Counter).Take(10);
            vhvm.MedicamentHigh = x.ToList();

            //user list
            var usersList = _context.ApplicationUser.ToList();
            vhvm.Users = usersList;

            //request list
            var requestsList = _context.Requests.ToList();
            vhvm.Requests = requestsList;

            //price
            var totalBudget = vhvm.MedicamentLow.Sum(a => a.budget);
            vhvm.sumBudget = totalBudget;

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
