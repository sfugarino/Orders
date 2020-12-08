using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Core.Business
{
    public class OrderDto
    {
        public IList<MenuItemDto> MenuItems { get; set; }
        public Guid ServerId { get; set; }
        public int Table { get; set; }

        public DateTime ReceivedAt { get; private set; }

        public OrderDto()
        {
            MenuItems = new List<MenuItemDto>();
            ReceivedAt = DateTime.Now;
        }

        public OrderDto(Guid serverId, int table)
        {
            MenuItems = new List<MenuItemDto>();
            Table = table;
            ReceivedAt = DateTime.Now;
        }
    }
}
