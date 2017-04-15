using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using system_core_with_authentication.Data;
using system_core_with_authentication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using system_core_with_authentication.Models.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace system_core_with_authentication.Controllers
{
    public class MedicamentOutController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MedicamentOutController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var stock = _context.Stocks.Include(s => s.Medicament)
                                                        .Select(s => s);
            //var stock = from s in _context.Stocks
            //            join m in _context.Medicaments on s.MedicamentId equals m.Id
            //            select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                stock = stock.Where(s => s.Medicament.Description.Contains(searchString));
            }


            return View(await stock.ToListAsync());
        }

        //[httppost]
        public ActionResult MedicamentOut(string values)
        {
            JArray array = JArray.Parse(values);
            foreach (JObject obj in array.Children<JObject>())
            {
                int id = 0;
                int quantity= 0;
                foreach (JProperty singleProp in obj.Properties())
                {
                    string name = singleProp.Name;
                    string value = singleProp.Value.ToString();

                    if (name.Equals("Id"))
                        id = Int32.Parse(value);
                    if (name.Equals("Quantity"))
                        quantity = Int32.Parse(value);
                }
                Stock item = _context.Stocks.FirstOrDefault(s => s.Id == id);
                item.Total = item.Total - quantity;
                _context.Update(item);
                _context.SaveChanges();

            }

            return RedirectToAction("Index","Stocks");
        }


        private bool StockExists(int id)
        {
            return _context.Stocks.Any(e => e.Id == id);
        }
    }
}