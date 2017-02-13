using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace WebServer.Tests
{
    [TestClass()]
    public class HttpserverTests
    {
        [TestMethod()]
        public void HttpserverTest()
        {
            Server server = new Server();
            Assert.IsNotNull(server);
        }

        [TestMethod()]
        public void StartTest()
        {
            Server server = new Server();
            Assert.IsTrue(server.Start());
        }
    }
}