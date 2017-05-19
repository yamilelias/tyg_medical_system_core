using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using system_core_with_authentication.Data;
using system_core_with_authentication.Models;
using system_core_with_authentication.Models.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Diagnostics;
using Microsoft.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace system_core_with_authentication.Controllers
{
    public class MaterialInventoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _hostingEnvironment;

        public MaterialInventoriesController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: MaterialInventories
        public async Task<IActionResult> Index()
        {
            return View(await _context.MaterialsInventory.ToListAsync());
        }

        // GET: MaterialInventories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materialInventory = await _context.MaterialsInventory
                .SingleOrDefaultAsync(m => m.Id == id);
            if (materialInventory == null)
            {
                return NotFound();
            }

            return View(materialInventory);
        }

        // GET: MaterialInventories/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaterialInventoryViewModel mIVM)
        {
            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "images/uploads");
            var fileNameToHash = mIVM.File.FileName + DateTime.Now;

            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(fileNameToHash))
                sb.Append(b.ToString("X2"));

            var fileName = $"{sb.ToString()}.jpeg";
            Debug.WriteLine(fileName);

            using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
            {
                await mIVM.File.CopyToAsync(fileStream);
            }

            var m = new MaterialInventory
            {
                Description = mIVM.Description,
                Serial = mIVM.Serial,
                Note = mIVM.Note,
                MaintananceDate = mIVM.MaintananceDate,
                ImageName = fileName
            };
            _context.MaterialsInventory.Add(m);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        private static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = SHA256.Create();  //or use SHA1.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }


        // GET: MaterialInventories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materialInventory = await _context.MaterialsInventory.SingleOrDefaultAsync(m => m.Id == id);
            if (materialInventory == null)
            {
                return NotFound();
            }
            var materialInventoryViewModel = new MaterialInventoryViewModel {
                Id = materialInventory.Id,
                Description = materialInventory.Description,
                Serial = materialInventory.Serial,
                Note = materialInventory.Note,
                MaintananceDate = materialInventory.MaintananceDate,
                ImageName = materialInventory.ImageName,
            };

            return View(materialInventoryViewModel);
        }

        // POST: MaterialInventories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MaterialInventoryViewModel mIVM)
        {
            if (id != mIVM.Id)
            {
                return NotFound();
            }

            try
            {
                var material = _context.MaterialsInventory.Find(id);
                material.Description = mIVM.Description;
                material.Serial = mIVM.Serial;
                material.Note = mIVM.Note;
                material.MaintananceDate = mIVM.MaintananceDate;
                mIVM.ImageName = material.ImageName;
                if (mIVM.File != null)
                {
                    Debug.WriteLine("Image not null");
                    var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "images/uploads");
                    var fileNameToHash = mIVM.File.FileName + DateTime.Now;

                    StringBuilder sb = new StringBuilder();
                    foreach (byte b in GetHash(fileNameToHash))
                        sb.Append(b.ToString("X2"));

                    var fileName = $"{sb.ToString()}.jpeg";
                    Debug.WriteLine(fileName);

                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                    {
                        await mIVM.File.CopyToAsync(fileStream);
                    }
                    material.ImageName = fileName;
                    mIVM.ImageName = fileName;
                }
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaterialInventoryExists(mIVM.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return View(mIVM);
        }

        // GET: MaterialInventories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materialInventory = await _context.MaterialsInventory
                .SingleOrDefaultAsync(m => m.Id == id);
            if (materialInventory == null)
            {
                return NotFound();
            }

            return View(materialInventory);
        }

        // POST: MaterialInventories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var materialInventory = await _context.MaterialsInventory.SingleOrDefaultAsync(m => m.Id == id);
            _context.MaterialsInventory.Remove(materialInventory);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool MaterialInventoryExists(int id)
        {
            return _context.MaterialsInventory.Any(e => e.Id == id);
        }
    }
}
