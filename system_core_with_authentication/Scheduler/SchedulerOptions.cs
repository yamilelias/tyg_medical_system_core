using Hangfire;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using system_core_with_authentication.Data;
using system_core_with_authentication.Models.Alerts;
using Treshold_Mail.Mail;

namespace system_core_with_authentication.Scheduler
{
    public class SchedulerOptions : IScheduler
    {
        private readonly ApplicationDbContext context;
        private readonly IMail mail;


        public SchedulerOptions(ApplicationDbContext context, IMail mail)
        {
            this.context = context;
            this.mail = mail;
        }

        public void CheckMinimumStock()
        {
            try
            {
                var x = context.MedicamentsBelowThreshold.Any();
                if (x)
                {
                    String message = "Se necesita resuplir los siguientes medicamentos: \n";
                    context.MedicamentsBelowThreshold.ToList().ForEach(e => {
                        String medicineName = "";
                        medicineName = context.Medicaments.Where(g => g.Id == e.Id)
                                                   .Select(g => g.Description).FirstOrDefault();

                        message += $"Medicamento: {medicineName} - Unidades actuales: {e.CurrentStock}\n";
                    });
                    if(context.AlertSettings.FirstOrDefault().EmailNotifications == true)
                    {
                        if(context.AlertSettings.FirstOrDefault().SendToAdmins == true)
                        {
                            mail.SendToAdmin(message, "Recordatorio");
                        }
                        if (context.AlertSettings.FirstOrDefault().SendToSupervisors == true)
                        {
                            mail.SendToSupervisor(message, "Recordatorio");
                        }
                    }

                    mail.SendToAdmin(message, "Recordatorio");
                    Debug.WriteLine("Message sent CheckMinimumStock()");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        public void Update()
        {
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
    }
}
