using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using system_core_with_authentication.Models;
using system_core_with_authentication.Models.Alerts;

namespace system_core_with_authentication.Data
{
    public class DbInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DbInitializer(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any admin.
            if (context.ApplicationUser.Any())
            {
                return;   // DB has been seeded
            }

            var alerts = new AlertSettings()
            {
                EmailNotifications = true,
                SmsNotifications = true,
                NotificationReminderPeriodPOne = Periods.Cada_dos_días,
                NotificationReminderPeriodPTwo = Periods.Cada_dos_días,
                SendToAdmins = true,
                SendToSupervisors = true
            };
            context.AlertSettings.Add(alerts);

            context.SaveChanges();
        }

        

    }

}
