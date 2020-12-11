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

        [ForeignKey("ParentItem")]
        public int? ParentId { get; set; }

        public Station Station { get; set; }

        public virtual ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

        public virtual ICollection<MenuItem_Size> Sizes { get; set; } = new List<MenuItem_Size>();


    }
}
