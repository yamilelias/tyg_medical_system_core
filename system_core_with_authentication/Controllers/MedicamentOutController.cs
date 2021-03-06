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
using Treshold_Mail.Mail;
using Microsoft.AspNetCore.Authorization;

namespace system_core_with_authentication.Controllers
{
    [Authorize(Roles = "Admin,Supervisor")]
    public class MedicamentOutController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMail MailService;

        public MedicamentOutController(ApplicationDbContext context, IMail MailService)
        {
            _context = context;
            this.MailService = MailService;
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
            Dictionary<int, int> ToCheckMini = new Dictionary<int, int>();
            foreach (JObject obj in array.Children<JObject>())
            {
                int id = 0;
                int quantity = 0;

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
                ToCheckMini.Add(item.MedicamentId, item.Total);
                _context.Update(item);
                _context.SaveChanges();

            }
            // ToDo - Add Async method to check current stock
            //Task taskA = Task.Run(() => {
            Dictionary<String, int> belowThreshold = new Dictionary<String, int>();
            ToCheckMini.ToList().ForEach(e => {
                var medId = e.Key;

                var sumTotal = _context.Stocks.Where(f => f.MedicamentId == medId)
                                              .Sum(f => f.Total);
                if (sumTotal <= e.Value)
                {
                    // Below Threshold
                    var name = _context.Medicaments.Where(a => a.Id == medId)
                                                   .Select(a => a.Description)
                                                   .FirstOrDefault();
                    belowThreshold.Add(name, sumTotal);
                    _context.MedicamentsBelowThreshold.Add(new MedicamentBelowThreshold()
                    {
                        MedicamentId = e.Key,
                        CurrentStock = e.Value
                    });
                    _context.SaveChanges();
                }
            });
            String medicineToSupply = "";
            foreach (KeyValuePair<String, int> x in belowThreshold.ToList())
            {
                medicineToSupply += $"Medicina: {x.Key}, Stock actual: {x.Value} \n";
            }
            MailService.SendToAdmin("Se necesita resuplir los siguientes medicamentos \n" +
                                    medicineToSupply, "Medicamentos para resuplir");
            //});
            // taskA.Wait();
            return Json(new { success = true });
        }


        private bool StockExists(int id)
        {
            return _context.Stocks.Any(e => e.Id == id);
        }
    }
}