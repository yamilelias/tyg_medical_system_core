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
using Microsoft.AspNetCore.Identity;

namespace system_core_with_authentication.Controllers
{
    public class RequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RequestsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Admin,Supervisor,Supervisor de RH,Supervisor de Inventario,Medico")]
        public ActionResult CreateReposition(string searchString)
        {
            var medicaments = _context.Medicaments.Select(m => m);

            if (!String.IsNullOrEmpty(searchString))
            {
                medicaments = medicaments.Where(m => m.Description.Contains(searchString));
            }

            MedicamentsWithUserLocationsViewModel vm = new MedicamentsWithUserLocationsViewModel();
            vm.MedicamentsForCreateReposition = medicaments.ToList();
            var actualUserId = _userManager.GetUserId(User);
            var x = _context.LocationSchedules.Include(b=>b.Location).Where(a => a.User.Id.Equals(actualUserId)).ToList().Select(a=>a.Location).Distinct();
            List<string> loc = new List<string>();
            foreach(var item in x)
            {
                loc.Add(item.Name);
                
            }
            vm.UserLocations=loc;

            return View(vm);
        }

        [Authorize(Roles = "Admin,Supervisor,Supervisor de RH,Supervisor de Inventario,Medico")]
        public ActionResult SaveReposition(string values, string username, string notes, string locationUser)
        {
            Request _request = new Request();
            _request.User = username;
            _request.Note = notes;
            _request.Date = DateTime.UtcNow;
            RepositionStock _repositionStock = new RepositionStock();
            _repositionStock.Request = _request;
            _repositionStock.Location = _context.Locations.Where(a => a.Name.Equals(locationUser)).FirstOrDefault();

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
                    if (name.Equals("Id"))
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
                _repositionStockDetailed.Medicament = _context.Medicaments.Where(i => i.Id == ID).FirstOrDefault();
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
                                             .OrderByDescending(r => r.Request.Date)
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
            arvm.Maternity_Leave = _context.Maternity_Leave.Include(r => r.Request)
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
                                             .Include(r => r.Location)
                                             .Include(r => r.RepositionStockDetailed)
                                             .ThenInclude(m => m.Medicament)
                                             .Where(i => i.Id == RepositionStockId)
                                             .ToList()
                                             .FirstOrDefault();
            double Totalcost = 0;
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
                                             .Include(r => r.Location)
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
         /// <summary>
         ///  GET: r_Shift_Change view
         /// </summary>
         /// <returns>  view  </returns>
        public ActionResult r_Shift_Change()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create_r_Shift_Change(string username, r_Shift_Change_ViewModel rscvm)
        {
            DateTime actualDate = DateTime.UtcNow;
            bool validDate = ((rscvm.Start_Date - actualDate).TotalDays > 3);

            if (ModelState.IsValid&& validDate)
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

            return View("r_Shift_Change", rscvm);
        }
        public ActionResult ViewShiftChangeDetails(int ShiftChangeId)
        {
            ShiftChange sc = _context.ShiftChange.Include(r => r.Request).Where(a => a.Id == ShiftChangeId).FirstOrDefault();
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
            var sc = _context.ShiftChange.Where(a => a.Id == shiftChange.Id).FirstOrDefault();
            sc.Solved = shiftChange.Solved;
            _context.Update(sc);
            _context.SaveChanges();
            return RedirectToAction("ShowAllRequests");
        }
        /*
         * End of Methods for Request r_Shift_Change
         */

        /*
         * Methods for Request r_BreastFeeding
         */
        public ActionResult r_BreastFeeding()
        {
            return View();
        }
        public ActionResult Create_r_BreastFeeding(string username, r_BreastFeeding_ViewModel rbfvm)
        {
            DateTime actualDate = DateTime.UtcNow;
            bool validDate = ((rbfvm.Start_Date - actualDate).TotalDays > 3);

            if (ModelState.IsValid && validDate)
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
            return View("r_BreastFeeding", rbfvm);
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
            var bf = _context.BreastFeeding.Where(a => a.Id == breastFeeding.Id).FirstOrDefault();
            bf.Solved = breastFeeding.Solved;
            _context.Update(bf);
            _context.SaveChanges();
            return RedirectToAction("ShowAllRequests");
        }
        /*
         * End Methods for Request r_BreastFeeding
         */

        /*
         * Methods for Request r_Permit
         */
        public ActionResult r_Permit()
        {
            return View();
        }
        public ActionResult Create_r_Permit(string username, r_Permit_ViewModel rpvm)
        {
            DateTime actualDate = DateTime.UtcNow;
            bool validDate = ((rpvm.Date - actualDate).TotalDays > 3);

            if (ModelState.IsValid && validDate)
            {
                Request r = new Request();
                r.User = username;
                r.Date = DateTime.UtcNow;
                r.Note = rpvm.Notes;

                Permit p = new Permit();
                p.Request = r;
                p.Type = rpvm.Type;
                p.Date = rpvm.Date;
                p.Start_Hour = rpvm.Start_Hour;
                p.End_Hour = rpvm.End_Hour;

                _context.Requests.Add(r);
                _context.Permit.Add(p);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View("r_Permit", rpvm);
        }
        public ActionResult ViewPermitDetails(int PermitId)
        {
            Permit p = _context.Permit.Include(r => r.Request).Where(a => a.Id == PermitId).FirstOrDefault();
            return View(p);
        }
        public ActionResult ChangeSolved_Permit(int Id)
        {
            Permit p = _context.Permit.Include(r => r.Request).Where(a => a.Id == Id).FirstOrDefault();
            return View(p);
        }
        [HttpPost]
        public ActionResult ChangeSolved_Permit(Permit permit)
        {
            var p = _context.Permit.Where(a => a.Id == permit.Id).FirstOrDefault();
            p.Solved = permit.Solved;
            _context.Update(p);
            _context.SaveChanges();
            return RedirectToAction("ShowAllRequests");
        }
        /*
         * End of Methods for Request r_Permit
         */

        /*
         * Methods for Request r_Allowance_Without_Payment
         */
        public ActionResult r_Allowance_Without_Payment()
        {
            return View();
        }
        public ActionResult Create_r_Allowance_Without_Payment(string username, r_Allowance_Without_Payment_ViewModel rawpvm)
        {
            DateTime actualDate = DateTime.UtcNow;
            bool validDate = ((rawpvm.Start_Date - actualDate).TotalDays > 3);

            if (ModelState.IsValid && validDate)
            {
                Request r = new Request();
                r.User = username;
                r.Date = DateTime.UtcNow;
                r.Note = rawpvm.Notes;

                AllowanceWithoutPayment awp = new AllowanceWithoutPayment();
                awp.Request = r;
                awp.Start_Date = rawpvm.Start_Date;
                awp.End_Date = rawpvm.End_Date;
                awp.Comeback_Date = rawpvm.Comeback_Date;

                _context.Requests.Add(r);
                _context.AllowanceWithoutPayment.Add(awp);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View("r_Allowance_Without_Payment", rawpvm);
        }
        public ActionResult ViewAllowanceWithoutPaymentDetails(int AllowanceWithoutPaymentId)
        {
            AllowanceWithoutPayment awp = _context.AllowanceWithoutPayment.Include(r => r.Request).Where(a => a.Id == AllowanceWithoutPaymentId).FirstOrDefault();
            return View(awp);
        }
        public ActionResult ChangeSolved_AllowanceWithoutPayment(int Id)
        {
            AllowanceWithoutPayment p = _context.AllowanceWithoutPayment.Include(r => r.Request).Where(a => a.Id == Id).FirstOrDefault();
            return View(p);
        }
        [HttpPost]
        public ActionResult ChangeSolved_AllowanceWithoutPayment(AllowanceWithoutPayment allowanceWithoutPayment)
        {
            var awp = _context.AllowanceWithoutPayment.Where(a => a.Id == allowanceWithoutPayment.Id).FirstOrDefault();
            awp.Solved = allowanceWithoutPayment.Solved;
            _context.Update(awp);
            _context.SaveChanges();
            return RedirectToAction("ShowAllRequests");
        }
        /*
        * End Methods for Request r_Allowance_Without_Payment
        */

        /*
         * Methods for Request r_Vacations
         */
        public ActionResult r_Vacations()
        {
            return View();
        }
        public ActionResult Create_r_Vacations(string username, r_Vacations_ViewModel rvvm)
        {
            DateTime actualDate = DateTime.UtcNow;
            bool validDate = ((rvvm.Start_Date - actualDate).TotalDays > 3);

            if (ModelState.IsValid && validDate)
            {
                Request r = new Request();
                r.User = username;
                r.Date = DateTime.UtcNow;
                r.Note = rvvm.Notes;

                Vacations v = new Vacations();
                v.Request = r;
                v.Vacation_Period_Completed = rvvm.Vacation_Period_Completed;
                v.Available_Days = rvvm.Available_Days;
                v.Requested_Days = rvvm.Requested_Days;
                v.Start_Date = rvvm.Start_Date;
                v.End_Date = rvvm.End_Date;
                v.Comeback_Date = rvvm.Comeback_Date;
                v.Pending_Days = rvvm.Pending_Days;

                _context.Requests.Add(r);
                _context.Vacations.Add(v);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View("r_Vacations", rvvm);
        }
        public ActionResult ViewVacationsDetails(int VacationsId)
        {
            Vacations v = _context.Vacations.Include(r => r.Request).Where(a => a.Id == VacationsId).FirstOrDefault();
            return View(v);
        }
        public ActionResult ChangeSolved_Vacations(int Id)
        {
            Vacations p = _context.Vacations.Include(r => r.Request).Where(a => a.Id == Id).FirstOrDefault();
            return View(p);
        }
        [HttpPost]
        public ActionResult ChangeSolved_Vacations (Vacations vacations)
        {
            var v = _context.Vacations.Where(a => a.Id == vacations.Id).FirstOrDefault();
            v.Solved = vacations.Solved;
            _context.Update(v);
            _context.SaveChanges();
            return RedirectToAction("ShowAllRequests");
        }
        /*
        * End Methods for Request r_Vacations
        */

        /*
         * Methods for Request r_Maternity_Leave
         */
        public ActionResult r_Maternity_Leave()
        {
            return View();
        }
        public ActionResult Create_r_Maternity_Leave(string username, r_Maternity_Leave_ViewModel rmlvm)
        {
            DateTime actualDate = DateTime.UtcNow;
            bool validDate = ((rmlvm.Start_Date - actualDate).TotalDays > 3);

            if (ModelState.IsValid && validDate)
            {
                Request r = new Request();
                r.User = username;
                r.Date = DateTime.UtcNow;
                r.Note = rmlvm.Notes;

                Maternity_Leave ml = new Maternity_Leave();
                ml.Request = r;
                ml.Start_Date = rmlvm.Start_Date;
                ml.End_Date = rmlvm.End_Date;
                ml.Covered_Days = rmlvm.Covered_Days;
                ml.Folio = rmlvm.Folio;
                ml.Labor_Start_Date = rmlvm.Labor_Start_Date;
                ml.Medic_Unit = rmlvm.Medic_Unit;

                _context.Requests.Add(r);
                _context.Maternity_Leave.Add(ml);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View("r_Maternity_Leave", rmlvm);
        }
        public ActionResult ViewMaternity_LeaveDetails(int Maternity_LeaveId)
        {
            Maternity_Leave v = _context.Maternity_Leave.Include(r => r.Request).Where(a => a.Id == Maternity_LeaveId).FirstOrDefault();
            return View(v);
        }
        public ActionResult ChangeSolved_Maternity_Leave(int Id)
        {
            Maternity_Leave p = _context.Maternity_Leave.Include(r => r.Request).Where(a => a.Id == Id).FirstOrDefault();
            return View(p);
        }
        [HttpPost]
        public ActionResult ChangeSolved_Maternity_Leave(Maternity_Leave maternity_Leave)
        {
            var m = _context.Maternity_Leave.Where(a => a.Id == maternity_Leave.Id).FirstOrDefault();
            m.Solved = maternity_Leave.Solved;
            _context.Update(m);
            _context.SaveChanges();
            return RedirectToAction("ShowAllRequests");
        }
        /*
        * End Methods for Request r_Maternity_Leave
        */

        /*
         * Methods for Request r_Viatical
         */
        public ActionResult r_Viatical()
        {
            return View();
        }
        public ActionResult Create_r_Viatical(string username, r_Viatical_ViewModel rvvm)
        {
            DateTime actualDate = DateTime.UtcNow;
            bool validDate = ((rvvm.Start_Date - actualDate).TotalDays > 3);

            if (ModelState.IsValid && validDate)
            {
                Request r = new Request();
                r.User = username;
                r.Date = DateTime.UtcNow;
                r.Note = rvvm.Notes;

                Viatical v = new Viatical();
                v.Request = r;
                v.Travel_Destination = rvvm.Travel_Destination;
                v.Start_Date = rvvm.Start_Date;
                v.End_Date = rvvm.End_Date;

                _context.Requests.Add(r);
                _context.Viatical.Add(v);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View("r_Viatical",rvvm);
        }
        public ActionResult ViewViaticalDetails(int ViaticalId)
        {
            Viatical v = _context.Viatical.Include(r => r.Request).Where(a => a.Id == ViaticalId).FirstOrDefault();
            return View(v);
        }
        public ActionResult ChangeSolved_Viatical(int Id)
        {
            Viatical p = _context.Viatical.Include(r => r.Request).Where(a => a.Id == Id).FirstOrDefault();
            return View(p);
        }
        [HttpPost]
        public ActionResult ChangeSolved_Viatical(Viatical viatical)
        {
            var v = _context.Viatical.Where(a => a.Id == viatical.Id).FirstOrDefault();
            v.Solved = viatical.Solved;
            _context.Update(v);
            _context.SaveChanges();
            return RedirectToAction("ShowAllRequests");
        }
        /*
        * End Methods for Request r_Viatical
        */
    }
}