using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core
{
    public class OrderItem
    {
        [Key]
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }
        public Order Order { get; set; }
    }
}
