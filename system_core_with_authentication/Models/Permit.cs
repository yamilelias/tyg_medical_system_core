﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace system_core_with_authentication.Models
{
    public class Permit
    {
        public int Id { get; set; }
        [ForeignKey("RequestId")]
        public virtual Request Request { get; set; }
        public int Type { get; set; }
        public DateTime Date { get; set; }
        public string Start_Hour { get; set; }
        public string End_Hour { get; set; }
        public bool Solved { get; set; }
    }
}