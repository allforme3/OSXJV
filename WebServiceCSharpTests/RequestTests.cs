using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.Tests
{
    [TestClass()]
    public class RequestTests
    {
        [TestCategory("Request")]
        [TestMethod()]
        public void GetRequestTestCorrect()
        {
            string filename = "Test";
            string contentType = "text/xml";
            string data = "<?xml version=\"1.0\" encoding=\"utf - 8\"?><test>test</test>";


            Request r = Request.GetRequest(filename, contentType, data);

            Assert.AreEqual(filename, r.Filename);
            Assert.AreEqual(contentType, r.Type);
            Assert.AreEqual(data,r.Data);
        }

        [TestCategory("Request")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "Raise exception when filename is null")]
        public void GetRequestTestNullParams()
        {
            Request.GetRequest(null, null, null);
        }

        [TestCategory("Request")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "Raise exception when data is null")]
        public void GetRequestTestNullFilename()
        {
            string contentType = "text/xml";
            string data = "<?xml version=\"1.0\" encoding=\"utf - 8\"?><test>test</test>";
            Request.GetRequest(null, contentType, data);
        }

        [TestCategory("Request")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "Raise exception when contentType is null")]
        public void GetRequestTestNullContentType()
        {
            string filename = "Test";
            string data = "<?xml version=\"1.0\" encoding=\"utf - 8\"?><test>test</test>";
            Request.GetRequest(filename, null, data);
        }

        [TestCategory("Request")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "Raise exception when data is null")]
        public void GetRequestTestNullData()
        {
            string filename = "Test";
            string contentType = "text/xml";
            Request.GetRequest(filename, contentType, null);
        }

        [TestCategory("Request")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "Raise exception when filename is empty")]
        public void GetRequestTestEmptyFilename()
        {
            string filename = "";
            string contentType = "text/xml";
            string data = "<?xml version=\"1.0\" encoding=\"utf - 8\"?><test>test</test>";
            Request.GetRequest(filename, contentType, data);
        }

        [TestCategory("Request")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "Raise exception when contentType is empty")]
        public void GetRequestTestEmptyContentType()
        {
            string filename = "Test";
            string contentType = "";
            string data = "<?xml version=\"1.0\" encoding=\"utf - 8\"?><test>test</test>";
            Request.GetRequest(filename, contentType, data);
        }

        [TestCategory("Request")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "Raise exception when data is empty")]
        public void GetRequestTestEmptyData()
        {
            string filename = "Test";
            string contentType = "text/xml";
            string data = "";
            Request.GetRequest(filename, contentType, data);
        }
    }
}