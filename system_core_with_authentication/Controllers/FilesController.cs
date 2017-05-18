using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using system_core_with_authentication.Data;

namespace system_core_with_authentication.Controllers
{
    public class FilesController : Controller
    {

        private IHostingEnvironment _environment;
        private ApplicationDbContext _context;

        public FilesController(IHostingEnvironment environment, ApplicationDbContext context)
        {

            _environment = environment;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ICollection<IFormFile> files)
        {
            var uploads = Path.Combine(_environment.WebRootPath, "files");
            Directory.CreateDirectory(Path.Combine(uploads));

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
            }
            return View();
        }

    }
}