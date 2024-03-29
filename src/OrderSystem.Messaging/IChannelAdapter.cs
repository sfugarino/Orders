﻿using OrderSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Messaging
{
    public interface IChannelAdapter: IDisposable
    {
        public void Send(Order order);
    }
}
