using WebServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace OSXJV.Classes.Tests
{ 
    [TestFixture]
    public class RequestTests
    {
        [Test]
        public void GetRequestTestCorrect()
        {
			Console.WriteLine ("GetRequestTestCorrect");
            string filename = "Test";
            string contentType = "text/xml";
            string data = "<?xml version=\"1.0\" encoding=\"utf - 8\"?><test>test</test>";


            Request r = Request.GetRequest(filename, contentType, data);

            Assert.AreEqual(filename, r.Filename);
            Assert.AreEqual(contentType, "text/xml");
            Assert.AreEqual(data,r.Data);
        }

        [Test]
        public void GetRequestTestNullParams()
        {
			Console.WriteLine ("GetRequestTestNullParams");
            Assert.Throws<ArgumentException>(() => Request.GetRequest(null, null, null));
        }

        [Test]
        public void GetRequestTestNullFilename()
        {
			Console.WriteLine ("GetRequestTestNullFilename");
            string contentType = "text/xml";
            string data = "<?xml version=\"1.0\" encoding=\"utf - 8\"?><test>test</test>";
            Assert.Throws<ArgumentException>(() => Request.GetRequest(null, contentType, data));
        }


        [Test]
        public void GetRequestTestNullContentType()
        {
			Console.WriteLine ("GetRequestTestNullContentType");
            string filename = "Test";
            string data = "<?xml version=\"1.0\" encoding=\"utf - 8\"?><test>test</test>";
            Assert.Throws<ArgumentException>(() => Request.GetRequest(filename, null, data));
        }

        [Test]
        public void GetRequestTestNullData()
        {
			Console.WriteLine ("GetRequestTestNullData");
            string filename = "Test";
            string contentType = "text/xml";
            Assert.Throws<ArgumentException>(() => Request.GetRequest(filename, contentType, null));
        }

        [Test]
        public void GetRequestTestEmptyFilename()
        {
			Console.WriteLine ("GetRequestTestEmptyFilename");
            string filename = "";
            string contentType = "text/xml";
            string data = "<?xml version=\"1.0\" encoding=\"utf - 8\"?><test>test</test>";
            Assert.Throws<ArgumentException>(() => Request.GetRequest(filename, contentType, data));
        }

        [Test]
        public void GetRequestTestEmptyContentType()
        {
			Console.WriteLine ("GetRequestTestEmptyContentType");
            string filename = "Test";
            string contentType = "";
            string data = "<?xml version=\"1.0\" encoding=\"utf - 8\"?><test>test</test>";
            Assert.Throws<ArgumentException>(() => Request.GetRequest(filename, contentType, data));
        }
        [Test]
        public void GetRequestTestEmptyData()
        {
			Console.WriteLine ("GetRequestTestEmptyData");
            string filename = "Test";
            string contentType = "text/xml";
            string data = "";
            Assert.Throws<ArgumentException>(() => Request.GetRequest(filename, contentType, data));
        }
    }
}