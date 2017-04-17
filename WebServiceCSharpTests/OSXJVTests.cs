using NUnit.Framework;
using System;

namespace OSXJV.Server.Tests
{
    [TestFixture]
    public class OSXJVTests
    {
        [Test]
        public void HttpserverTest()
        {
            OSXJVServer server = new OSXJVServer();
            Assert.IsNotNull(server);
        }

        [Test]
        public void StartTest()
        {
            OSXJVServer server = new OSXJVServer();
            Assert.IsTrue(server.Start());
            //server.Stop();
        }

        [Test]
        public void StopTest()
        {
            OSXJVServer server = new OSXJVServer();
            server.Start();
            Assert.IsTrue(server.Stop());

        }

        [Test]
        public void StopTestAlreadyStoppedOrNotRunning()
        {
            OSXJVServer server = new OSXJVServer();
            Assert.IsTrue(server.Stop());

        }
    }

}
