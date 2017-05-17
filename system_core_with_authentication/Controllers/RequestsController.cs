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
            AllRequestsViewModel arvm = new AllRequestsViewModel();
            arvm.RepositionStock = _context.RepositionStocks.Include(r => r.Request)
                                             .OrderByDescending(r=>r.Request.Date)
                                             .ToList();
            arvm.ShiftChange = _context.ShiftChange.Include(r => r.Request)
                                             .OrderByDescending(r => r.Request.Date)
                                             .ToList();
            arvm.BreastFeeding = _context.BreastFeeding.Include(r => r.Request)
                                             .OrderByDescending(r => r.Request.Date)
                                             .ToList();
            arvm.Permit = _context.Permit.Include(r => r.Request)
                                             .OrderByDescending(r => r.Request.Date)
                                             .ToList();
            arvm.AllowanceWithoutPayment = _context.AllowanceWithoutPayment.Include(r => r.Request)
                                             .OrderByDescending(r => r.Request.Date)
                                             .ToList();
            arvm.Vacations = _context.Vacations.Include(r => r.Request)
                                             .OrderByDescending(r => r.Request.Date)
                                             .ToList();
            arvm.Viatical = _context.Viatical.Include(r => r.Request)
                                             .OrderByDescending(r => r.Request.Date)
                                             .ToList();


            return View(arvm);
        }
        public ActionResult ViewRepositionDetails(int RepositionStockId)
        {
            var x = _context.RepositionStocks.Include(r => r.Request)
                                             .Include(r => r.RepositionStockDetailed)
                                             .ThenInclude(m => m.Medicament)
                                             .Where(i => i.Id == RepositionStockId)
                                             .ToList()
                                             .FirstOrDefault();
            double Totalcost=0;
            double costPerOrder = 0;
            foreach (var item in x.RepositionStockDetailed)
            {
                costPerOrder = item.RequestStock * item.Medicament.Price;
                
                Totalcost += costPerOrder;
            }

            RepositionStockDetailedWithCostViewModel rsdwc = new RepositionStockDetailedWithCostViewModel();
            rsdwc.RsList = x;
            rsdwc.cost = Totalcost;

            return View(rsdwc);
        }

        public ActionResult ChangeSolved(int? id)
        {
            var x = _context.RepositionStocks.Include(r => r.Request)
                                             .Include(r => r.RepositionStockDetailed)
                                             .ThenInclude(m => m.Medicament)
                                             .Where(m => m.Id == id)
                                             .ToList()
                                             .FirstOrDefault();
            return View(x);
        }

        [HttpPost]
        public ActionResult ChangeSolved(RepositionStock repositionStock)
        {
            _context.Update(repositionStock);
            _context.SaveChanges();
            return RedirectToAction("ShowAllRequests");
        }


        /*
         * Methods for Request r_Shift_Change
         */
        public ActionResult r_Shift_Change()
        {
            return View();
        }
        public ActionResult Create_r_Shift_Change(string username, r_Shift_Change_ViewModel rscvm)
        {
            if (ModelState.IsValid)
            {
                Request r = new Request();
                r.User = username;
                r.Date = DateTime.UtcNow;
                r.Note = rscvm.Notes;

                ShiftChange rsc = new ShiftChange();
                rsc.Request = r;
                rsc.Start_Date = rscvm.Start_Date;
                rsc.End_Date = rscvm.End_Date;
                rsc.Return_Date = rscvm.Return_Date;

                _context.Requests.Add(r);
                _context.ShiftChange.Add(rsc);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(rscvm);
        }
        public ActionResult ViewShiftChangeDetails(int ShiftChangeId)
        {
            ShiftChange sc = _context.ShiftChange.Include(r=>r.Request).Where(a => a.Id == ShiftChangeId).FirstOrDefault();
            return View(sc);
        }
        public ActionResult ChangeSolved_ShiftChange(int Id)
        {
            ShiftChange sc = _context.ShiftChange.Include(r => r.Request).Where(a => a.Id == Id).FirstOrDefault();
            return View(sc);
        }
        [HttpPost]
        public ActionResult ChangeSolved_ShiftChange(ShiftChange shiftChange)
        {
            _context.Update(shiftChange);
            _context.SaveChanges();
            return RedirectToAction("ShowAllRequests");
        }

        /*
         * Methods for Request r_BreastFeeding
         */
        public ActionResult r_BreastFeeding()
        {
            return View();
        }
        public ActionResult Create_r_BreastFeeding(string username, r_BreastFeeding_ViewModel rbfvm)
        {
            if (ModelState.IsValid)
            {
                Request r = new Request();
                r.User = username;
                r.Date = DateTime.UtcNow;
                r.Note = rbfvm.Notes;

                BreastFeeding bf = new BreastFeeding();
                bf.Request = r;
                bf.Start_Date = rbfvm.Start_Date;
                bf.End_Date = rbfvm.End_Date;
                bf.Start_Hour = rbfvm.Start_Hour;
                bf.End_Hour = rbfvm.End_Hour;

                _context.Requests.Add(r);
                _context.BreastFeeding.Add(bf);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(rbfvm);
        }
        public ActionResult ViewBreastFeedingDetails(int BreastFeedingId)
        {
            BreastFeeding bf = _context.BreastFeeding.Include(r => r.Request).Where(a => a.Id == BreastFeedingId).FirstOrDefault();
            return View(bf);
        }
        public ActionResult ChangeSolved_BreastFeeding(int Id)
        {
            BreastFeeding bf = _context.BreastFeeding.Include(r => r.Request).Where(a => a.Id == Id).FirstOrDefault();
            return View(bf);
        }
        [HttpPost]
        public ActionResult ChangeSolved_BreastFeeding(BreastFeeding breastFeeding)
        {
            _context.Update(breastFeeding);
            _context.SaveChanges();
            return RedirectToAction("ShowAllRequests");
        }
    }
}