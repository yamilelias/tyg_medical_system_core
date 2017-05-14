﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace system_core_with_authentication.Models
{
    public class LocationSchedule
    {
        [Key]
        public int Id { get; set; }
        //[ForeignKey("User")]
        //public int IdUser { get; set; }
        public virtual ApplicationUser User { get; set; }

        //[ForeignKey("Location")]
        //public int IdLocation { get; set; }
        public virtual Location Location { get; set; }

        public string Monday { get; set; }
        public string Tuesday { get; set; }
        public string Wednesday { get; set; }
        public string Thursday { get; set; }
        public string Friday { get; set; }
        public string Saturday { get; set; }
        public string Sunday { get; set; }

    }
}
