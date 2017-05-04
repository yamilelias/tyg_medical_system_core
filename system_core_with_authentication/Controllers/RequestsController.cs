using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using system_core_with_authentication.Data;
using Newtonsoft.Json.Linq;
using system_core_with_authentication.Models;
using Microsoft.EntityFrameworkCore;
using system_core_with_authentication.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace system_core_with_authentication.Controllers
{
    public class RequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Admin,Supervisor,User")]
        public ActionResult CreateReposition(string searchString)
        {
            var medicaments = _context.Medicaments.Select(m=>m) ;

            if (!String.IsNullOrEmpty(searchString))
            {
                medicaments = medicaments.Where(m => m.Description.Contains(searchString));
            }
            return View(medicaments.ToList());
        }
        [Authorize(Roles = "Admin,Supervisor,User")]
        public ActionResult SaveReposition(string values,string username, string notes)
        {
            Request _request = new Request();
            _request.User = username;
            _request.Note = notes;
            _request.Date = DateTime.UtcNow;
            RepositionStock _repositionStock = new RepositionStock();
            _repositionStock.Request = _request;

            List<RepositionStockDetailed> medicamentsList = new List<RepositionStockDetailed>();

            JArray array = JArray.Parse(values);
            foreach (JObject obj in array.Children<JObject>())
            {
                
                int actualQuantity = 0;
                int requestQuantity = 0;
                int ID = 0;
                foreach (JProperty singleProp in obj.Properties())
                {
                    string name = singleProp.Name;
                    string value = singleProp.Value.ToString();

                    if (name.Equals("ActualQuantity"))
                        actualQuantity = Int32.Parse(value);
                    if (name.Equals("RequestQuantity"))
                        requestQuantity = Int32.Parse(value);
                    if(name.Equals("Id"))
                        ID = Int32.Parse(value);

                }
                //Add to medicament counter
                Medicament med = _context.Medicaments.Where(i => i.Id == ID).FirstOrDefault();
                med.Counter += requestQuantity;
                _context.Medicaments.Update(med);

                //Create RepositionStockDetailed
                RepositionStockDetailed _repositionStockDetailed = new RepositionStockDetailed();
                _repositionStockDetailed.CurrentStock = actualQuantity;
                _repositionStockDetailed.RequestStock = requestQuantity;
                _repositionStockDetailed.Medicament = _context.Medicaments.Where(i=>i.Id==ID).FirstOrDefault();
                _context.RepositionStockDetailed.Add(_repositionStockDetailed);

                medicamentsList.Add(_repositionStockDetailed);
                _repositionStock.RepositionStockDetailed = medicamentsList;

            }
            _context.Requests.Add(_request);
            _context.RepositionStocks.Add(_repositionStock);
            _context.SaveChanges();
            return Json(new { success = true });
        }

        public ActionResult ShowAllRequests()
        {
            var x = _context.RepositionStocks.Include(r => r.Request).OrderByDescending(r=>r.Request.Date).ToList();

            return View(x);
        }
        public ActionResult ViewRepositionDetails(int RepositionStockId)
        {
            var x = _context.RepositionStocks.Include(r => r.Request).Include(r=>r.RepositionStockDetailed).ThenInclude(m=>m.Medicament).Where(i=>i.Id==RepositionStockId).ToList().FirstOrDefault();
            return View(x);
        }

        public ActionResult ChangeSolved(int? id)
        {
            var x = _context.RepositionStocks.Include(r => r.Request).Include(r => r.RepositionStockDetailed).ThenInclude(m => m.Medicament).Where(m => m.Id == id).ToList().FirstOrDefault();
            return View(x);
        }

        [HttpPost]
        public ActionResult ChangeSolved(RepositionStock repositionStock)
        {
            _context.Update(repositionStock);
            _context.SaveChanges();
            return RedirectToAction("ShowAllRequests");
        }
    }
}