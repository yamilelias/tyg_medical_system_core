using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace system_core_with_authentication.Models.Alerts
{
    public class AlertSettings
    {
        public int Id { get; set; }

        // STOCK BELOW THRESHOLD
        public bool EmailNotifications { get; set; }
        public bool SmsNotifications { get; set; }
        // Priority 1 items
        public Periods NotificationReminderPeriodPOne { get; set; }

        // Priority 2 items
        public Periods NotificationReminderPeriodPTwo { get; set; }

        public bool SendToAdmins { get; set; }
        public bool SendToSupervisors { get; set; }

    }

    public enum Periods
    {
        Todos_los_días = 1,
        Cada_dos_días = 2,
        Cada_tres_días = 3,
        Cada_semana = 7
    }

}
