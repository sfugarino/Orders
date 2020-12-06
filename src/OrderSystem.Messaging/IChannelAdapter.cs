using OrderSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Core
{
    public interface IChannelAdapter
    {
        public void Send(Order order);
    }
}
