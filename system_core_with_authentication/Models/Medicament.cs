using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace system_core_with_authentication.Models
{
    public class Medicament
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Descripcion")]
        public string Description { get; set; }
        [DisplayName("Contenido")]
        public int Content { get; set; }
        [DisplayName("Tipo")]
        public string Type { get; set; }
        [DisplayName("Precio")]
        public double Price { get; set; }
        [DisplayName("Prioridad")]
        public int Priority { get; set; }
        public int Counter { get; set; }
        public virtual ICollection<Stock> Stocks { get; set; }
    }
}
