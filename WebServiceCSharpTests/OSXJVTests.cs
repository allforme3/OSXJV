using NUnit.Framework;
using System;
using System.IO;

namespace OSXJV.Server.Tests
{
    [TestFixture]
    public class OSXJVTests
    {
        [Test]
        public void HttpserverTest()
        {
			Console.WriteLine ("HttpserverTest");
            OSXJVServer server = new OSXJVServer();
            Assert.IsNotNull(server);
        }

        [Test]
        public void StartTest()
        {
			Console.WriteLine ("StartTest");

            Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
            string arg = "";
            string arg2 = "";

            string dir = Directory.GetCurrentDirectory();
            arg = dir + "/Logger/";
            arg2 = dir + "/Cache/";

            if (!Directory.Exists(arg))
                Directory.CreateDirectory(arg);
            if (!Directory.Exists(arg2))
                Directory.CreateDirectory(arg2);

            OSXJVServer server = new OSXJVServer();
            Assert.IsTrue(server.Start(arg,arg2));
            //server.Stop();
        }

        [Test]
        public void StopTest()
        {
			Console.WriteLine ("StopTest");

            Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
            string arg = "";
            string arg2 = "";

            string dir = Directory.GetCurrentDirectory();
            arg = dir + "/Logger/";
            arg2 = dir + "/Cache/";

            if (!Directory.Exists(arg))
                Directory.CreateDirectory(arg);
            if (!Directory.Exists(arg2))
                Directory.CreateDirectory(arg2);

            OSXJVServer server = new OSXJVServer();
            server.Start(arg,arg2);
            Assert.IsTrue(server.Stop());

        }

        [Test]
        public void StopTestAlreadyStoppedOrNotRunning()
        {
			Console.WriteLine ("StopTestAlreadyStoppedOrNotRunning");
            OSXJVServer server = new OSXJVServer();
            Assert.IsTrue(server.Stop());

        }
    }
}
