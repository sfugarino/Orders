using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Core.Business
{
    public class Order
    {
        public IList<MenuItem> MenuItems { get; set; }
        public Guid ServerId { get; set; }
        public int Table { get; set; }

        public DateTime ReceivedAt { get; private set; }

        public Order()
        {
            MenuItems = new List<MenuItem>();
            ReceivedAt = DateTime.Now;
        }

        public Order(Guid serverId, int table)
        {
            MenuItems = new List<MenuItem>();
            Table = table;
            ReceivedAt = DateTime.Now;
        }
    }
}
