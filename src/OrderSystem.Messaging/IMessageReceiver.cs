﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Messaging
{
    public interface IMessageReceiver: IDisposable
    {
        void Start();
        event EventHandler<OrderReceivedEventArgs> OnOrderReceived;
    }
}
