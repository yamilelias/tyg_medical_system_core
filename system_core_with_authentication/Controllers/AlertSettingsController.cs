using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using system_core_with_authentication.Data;
using system_core_with_authentication.Models.Alerts;
using system_core_with_authentication.Scheduler;
using Microsoft.AspNetCore.Authorization;

namespace system_core_with_authentication.Controllers
{

    [Authorize(Roles = "Admin, Supervisor")]
    public class AlertSettingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IScheduler scheduler;

        public AlertSettingsController(ApplicationDbContext context, IScheduler scheduler)
        {
            _context = context;
            this.scheduler = scheduler;
        }

        // GET: AlertSettings
        public async Task<IActionResult> Index()
        {
            var alertSettings = await _context.AlertSettings
                                              .SingleOrDefaultAsync();
            if (alertSettings == null)
            {
                return NotFound();
            }

            return View(alertSettings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int id, [Bind("Id,EmailNotifications,SmsNotifications,NotificationReminderPeriodPOne,NotificationReminderPeriodPTwo,SendToAdmins,SendToSupervisors")] AlertSettings alertSettings)
        {
            if (id != alertSettings.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(alertSettings);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlertSettingsExists(alertSettings.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                scheduler.Update();
                return RedirectToAction("Index");
            }
            return View(alertSettings);
        }

        private bool AlertSettingsExists(int id)
        {
            return _context.AlertSettings.Any(e => e.Id == id);
        }
    }
}
