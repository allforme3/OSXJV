using WebServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using NUnit.Framework;

namespace WebServer.Tests
{
    [TestFixture]
    public class OSXJVTests
    {
        [Test]
        public void HttpserverTest()
        {
            OSXJV server = new OSXJV();
            Assert.IsNotNull(server);
        }

        [Test]
        public void StartTest()
        {
            OSXJV server = new OSXJV();
            Assert.IsTrue(server.Start());
        }

        [Test]
        public void TestSaveFileCorrect()
        {
            string id = "00025148415";
            Node n = new Node();

            OSXJV server = new OSXJV();
            Assert.DoesNotThrow(() => server.SaveFile(id, n));
        }

        [Test]
        public void TestSaveFileBothNullPassed()
        {
            OSXJV server = new OSXJV();
            Assert.Throws<ArgumentException>(() => server.SaveFile(null,null));
        }

        [Test(Description ="Tests Null id passed to SaveFile")]
        public void TestSaveFileIDNullPassed()
        {
            Node n = new Node();
            OSXJV server = new OSXJV();
            Assert.Throws<ArgumentException>(() => server.SaveFile(null, n));
        }

        [Test]
        public void TestSaveFileNodesNullPassed()
        {
            string id = "00025148415";
            OSXJV server = new OSXJV();
            Assert.Throws<ArgumentException>(() => server.SaveFile(id, null));
        }
    }

}
