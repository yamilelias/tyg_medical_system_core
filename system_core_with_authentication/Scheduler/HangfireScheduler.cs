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
using system_core_with_authentication.Models.Alerts;
using system_core_with_authentication.Scheduler;
using Treshold_Mail.Mail;

namespace Treshold_Mail.Scheduler
{
     static class HangfireScheduler
    {
        private static IApplicationBuilder _app;

        public static void Init(IApplicationBuilder app)
        {
            _app = app;
            var context = _app.ApplicationServices.GetRequiredService<ApplicationDbContext>();
            var result = context.AlertSettings.FirstOrDefault();

            switch (result.NotificationReminderPeriodPOne)
            {
                case Periods.Cada_semana:
                    RecurringJob.AddOrUpdate("1", () => CheckMinimumStock(), Cron.Weekly);
                    break;
                case Periods.Cada_tres_días:
                    RecurringJob.AddOrUpdate("1", () => CheckMinimumStock(), Cron.DayInterval(3));
                    break;
                case Periods.Cada_dos_días:
                    RecurringJob.AddOrUpdate("1", () => CheckMinimumStock(), Cron.DayInterval(2));
                    break;
                case Periods.Todos_los_días:
                    RecurringJob.AddOrUpdate("1", () => CheckMinimumStock(), Cron.Daily);
                    break;
            }
        }

        public static void CheckMinimumStock()
        {
            var scheduler = _app.ApplicationServices.GetRequiredService<IScheduler>();
            scheduler.CheckMinimumStock();
        }
    }
}
