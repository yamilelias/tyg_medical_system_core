using Microsoft.AspNetCore.Identity;
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

        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any admin.
            if (context.ApplicationUser.Any())
            {
                return;   // DB has been seeded
            }

            var admin = new ApplicationUser[]
            {
            new ApplicationUser{Id="1",Name="Asti",LastName="Asti",SecondLastName="Asti",UserName="asti@asti.com",
            Email ="asti@asti.com",Telephone="11111111",PasswordHash="AQAAAAEAACcQAAAAELkUHPHlFZzboIN3qHjbcX81x5HeQvbTsWnFA7joNJJNO4VY0uvDyj3pwHfazadExg=="},
            };
            var alerts = new AlertSettings() {
                EmailNotifications = true,
                SmsNotifications = true,
                NotificationReminderPeriodPOne = Periods.Cada_dos_días,
                NotificationReminderPeriodPTwo = Periods.Cada_dos_días,
                SendToAdmins = true,
                SendToSupervisors = true
            };
            context.AlertSettings.Add(alerts);

            foreach (ApplicationUser a in admin)
            {
                context.ApplicationUser.Add(a);
            }

            context.SaveChanges();


        }
    }

}
