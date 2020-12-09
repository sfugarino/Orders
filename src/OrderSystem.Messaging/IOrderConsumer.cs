using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrderSystem.Messaging
{
    public interface IOrderConsumer
    {
        Task BeginConsumeAsync(CancellationToken cancellationToken = default);
    }
}
