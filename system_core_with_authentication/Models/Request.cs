using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace system_core_with_authentication.Models
{
    public class Request
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Fecha")]
        public DateTime Date { get; set; }
        [DisplayName("Notas")]
        public string Note { get; set; }
        [ForeignKey("IdUser")]
        public int IdUser { get; set; }
        public virtual User User { get; set;}
        [ForeignKey("IdUserApproved")]
        public int IdUserApproved { get; set; }
        public virtual User UserApproved {get;set;}
    }
}
