using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using system_core_with_authentication.Data;
using Microsoft.AspNetCore.Authorization;

namespace system_core_with_authentication.Controllers
{
    /**
     * The FilesController is the controller in charge 
     * of managing all files and their actions, which include
     * uploading and deleting files into and from a local folder of the project. 
     * @author  Jonathan Torres
     * @version 1.0
     */

    public class FilesController : Controller
    {

        private IHostingEnvironment _environment;
        private ApplicationDbContext _context;

        public FilesController(IHostingEnvironment environment, ApplicationDbContext context)
        {

            _environment = environment;
            _context = context;
        }

        /*
         * This method displays the list of files uploaded in the
         * /wwwroot/files/ folder of the project. The methods collects a list of Strings, 
         * each bound to an existing file. This list is passed on to the Index view.
         * @param   unused
         * @return  Index view with list of Strings
         */
        public IActionResult Index()
        {
            string path = "wwwroot/files/";

            List<String> listOfFiles = new List<String>();

                foreach (string fileName in Directory.GetFiles(path))
                {
                var fileNameTrimmed = fileName.Substring("wwwroot/".Length);
                    listOfFiles.Add(fileNameTrimmed);
                }
            
            return View(listOfFiles);
        }

        /*
         * This method uploads files into the /wwwroot/files/ folder of the project.
         * The method gathers the file through an <input> tag in the Index view of AlertSettings,
         * then uses a file stream to upload the files into the folder.
         * Finally redirects the user to the same page after the upload has been successful.
         * @param   IFormFile files - the file that will be uploaded
         * @return  Index view of AlertSettings
         */

        
        [HttpPost]
        [Authorize(Roles = "Admin")]
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
            return RedirectToAction("Index", "AlertSettings");
        }


        /*
         * This method deletes the selected file, displayed in the Index view of Files.
         * A string parameter with the filename is used to look for the file and delete it.
         * Afterwards, the user is redirected to the Index view of Files.
         * @param   string filename - String which contains the name of the file that will be deleted
         * @return  Index view of files
         */

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string fileName)
        {

            System.IO.File.Delete("wwwroot/" + fileName);

            return RedirectToAction("Index", "Files");
        }

    }

}