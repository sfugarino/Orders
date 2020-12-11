using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Data.Entities
{
    public class MenuItem_Size
    {
        [Key]
        [ForeignKey("MenuItem")]
        public int MenuItemId { get; set; }
        public MenuItem MenuItem { get; set; }

        [Key]
        [ForeignKey("ItemSize")]
        public int ItemSizeId { get; set; }
        public ItemSize ItemSize { get; set; }

        public decimal Price { get; set; }
    }
}
