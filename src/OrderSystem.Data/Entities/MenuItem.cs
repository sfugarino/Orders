using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Data.Entities
{
    [Table("MenuItems")]
    public class MenuItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [ForeignKey("MenuItem")]
        public int? Parent { get; set; }

        public Station Station { get; set; }

        public virtual ICollection<MenuItem> MenuItems { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
