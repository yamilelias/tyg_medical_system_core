using Hangfire;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using system_core_with_authentication.Data;
using system_core_with_authentication.Models;
using Treshold_Mail.Mail;

namespace Treshold_Mail.Scheduler
{
     static class HangfireScheduler
    {
        private static IApplicationBuilder _app;

        public static void Init(IApplicationBuilder app)
        {
            _app = app;
            RecurringJob.AddOrUpdate("1", () => CheckMinimumStock(), Cron.Daily);
        }

        public static void CheckMinimumStock()
        {     
            try
            {
                var _context = _app.ApplicationServices.GetService<ApplicationDbContext>();
                var x = _context.MedicamentsBelowThreshold.Any();
                if (x)
                {
                    String message = "Se necesita resuplir los siguientes medicamentos: \n";
                    _context.MedicamentsBelowThreshold.ToList().ForEach(e => {
                        String medicineName = "";
                        medicineName = _context.Medicaments.Where(g => g.Id == e.Id)
                                                   .Select(g => g.Description).FirstOrDefault();

                        message += $"Medicamento: {medicineName} - Unidades actuales: {e.CurrentStock}\n";
                    });
                    var _mail = _app.ApplicationServices.GetService<IMail>();
                        _mail.SendToAdmin(message, "Recordatorio");
                    Debug.WriteLine("Message sent CheckMinimumStock()");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
              
        }
    }
}
