using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using system_core_with_authentication.Data;
using Newtonsoft.Json.Linq;
using system_core_with_authentication.Models;

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

        public ActionResult CreateReposition(string searchString)
        {
            var medicaments = _context.Medicaments.Select(m=>m) ;

            if (!String.IsNullOrEmpty(searchString))
            {
                medicaments = medicaments.Where(m => m.Description.Contains(searchString));
            }
            return View(medicaments.ToList());
        }

        public ActionResult SaveReposition(string values,string username)
        {
            Request _request = new Request();
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


                RepositionStockDetailed _repositionStockDetailed = new RepositionStockDetailed();
                _repositionStockDetailed.CurrentStock = actualQuantity;
                _repositionStockDetailed.RequestStock = requestQuantity;
                _repositionStockDetailed.RepositionStock = _repositionStock;
                _repositionStockDetailed.Medicament = _context.Medicaments.Where(i=>i.Id==ID).FirstOrDefault();
                _context.RepositionStockDetailed.Add(_repositionStockDetailed);

                medicamentsList.Add(_repositionStockDetailed);
                _repositionStock.rpd = medicamentsList;

            }
            _context.Requests.Add(_request);
            _context.RepositionStocks.Add(_repositionStock);
            _context.SaveChanges();
            return Json(new { success = true });
        }
    }
}