using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }

        public ICollection<OrderItem> Items { get; set; }
    }
}
