using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Data.Entities
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

        [ForeignKey("Employee")]
        public int ServerId { get; set; }

        [Required]
        public Employee Server { get; set; }

        [DefaultValue(OrderStatus.Processing)]
        public OrderStatus Status { get; set; }
    }
}
