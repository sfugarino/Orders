using OrderSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Messaging
{
    public class OrderReceivedEventArgs : EventArgs
    {
        public Order Order { get; set; }
    }
}
