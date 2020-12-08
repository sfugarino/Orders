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
        protected BaseTest()
            : base(new ContainerConfiguration(
                "rabbitmq:3-management", 
                "3-management",
                new int[] { 5672, 15672 },
                new Tuple<int, int>[] { Tuple.Create(5672, 5672), Tuple.Create(15672, 15672) }, 
                new List<string>() { "RABBITMQ_DEFAULT_USER=user", "RABBITMQ_DEFAULT_PASS=password" }
            ))
        {
 
        }
    }
}
