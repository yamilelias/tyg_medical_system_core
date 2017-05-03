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
        [Required]
        [DisplayName("Descripcion")]
        public string Description { get; set; }
        [Required]
        [DisplayName("Contenido")]
        public int Content { get; set; }
        [Required]
        [DisplayName("Tipo")]
        public string Type { get; set; }
        [Required]
        [DisplayName("Precio")]
        public double Price { get; set; }
        [Required]
        [DisplayName("Prioridad")]
        public int Priority { get; set; }
        public int Counter { get; set; }
        
        [DisplayName("Imagen del Medicamento")]
        public string MedicamentImage { get; set; }

        public virtual ICollection<Stock> Stocks { get; set; }

        [DisplayName("Stock Mínimo")]
        public int MinimumStock { get; set; }


    }
}
