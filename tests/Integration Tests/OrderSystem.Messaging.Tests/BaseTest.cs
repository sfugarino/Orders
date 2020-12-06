using System;
using OrderSystem.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TestUtils;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace OrderSystem.Messaging.Tests
{
    public class BaseTest : DockerSetup
    {
        DockerSetup _setup = null;

        static int[] _exposedPorts = { 5672, 15672 };
        static Tuple<int, int>[] _portBindings = 
        { 
            Tuple.Create(5672, 5672), 
            Tuple.Create(15672, 15672)
        };
        static IList<string> _enviroment = new List<string>(){ "RABBITMQ_DEFAULT_USER=user", "RABBITMQ_DEFAULT_PASS=password" };
        
        protected BaseTest()
            : base("rabbitmq:3-management", _exposedPorts, _portBindings, _enviroment)
        {
 
        }
    }
}
