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
    public class ResponseTests
    {
        [TestCategory("Response")]
        [TestMethod()]
        public void GetResponseTestCorrect()
        {
            int passed = 200;
            string mime = "application/json";
            byte[] data = new byte[5];

            Assert.IsNotNull(Response.GetResponse(passed, mime, data));
        }

        [TestCategory("Response")]
        [TestMethod()]
        public void GetResponseTestGetInvalidResponse()
        {
            Response response = Response.GetInvalidRequestResponse();

            Assert.AreEqual(405, response.status);
            Assert.AreEqual("text/html", response.mime);
            Assert.IsTrue(response.data.Length == 0);
        }

        [TestCategory("Response")]
        [TestMethod()]
        public void GetResponseTestGetErrorResponse()
        {
            Response response = Response.GetErrorResponse();

            Assert.AreEqual(400, response.status);
            Assert.AreEqual("text/html", response.mime);
            Assert.IsTrue(response.data.Length == 0);
        }

        [TestCategory("Response")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "Raise exception when Status is zero")]
        public void GetResponseTestStatusZero()
        {
            string mime = "application/json";
            byte[] data = new byte[0];

            Response.GetResponse(0, mime, data);
        }

        [TestCategory("Response")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "Raise exception when Mime is null")]
        public void GetResponseTestNullMime()
        {
            int passed = 200;
            byte[] data = new byte[0];

            Response.GetResponse(passed, null, data);
        }

        [TestCategory("Response")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "Raise exception when Data is null")]
        public void GetResponseTestNullData()
        {
            int passed = 200;
            string mime = "application/json";

            Response.GetResponse(passed, mime, null);
        }


        [TestCategory("Response")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "Raise exception when Mime is null")]
        public void GetResponseTestEmptyMime()
        {
            int passed = 200;
            byte[] data = new byte[0];

            Response.GetResponse(passed, "", data);
        }

        [TestCategory("Response")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "Raise exception when Data is empty")]
        public void GetResponseTestEmptyData()
        {
            int passed = 200;
            string mime = "application/json";

            Response.GetResponse(passed, mime, new byte[0]);
        }

        //ResponseJSON

        [TestCategory("Response")]
        [TestMethod()]
        public void GetResponseJSONTestCorrect()
        {
            int passed = 200;
            byte[] data = new byte[5];

            Assert.IsNotNull(Response.GetResponseJSON(passed, data));
        }

        [TestCategory("Response")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "Raise exception when Status is zero")]
        public void GetResponseJSONTestStatusZero()
        {
            byte[] data = new byte[0];

            Response.GetResponseJSON(0, data);
        }

        [TestCategory("Response")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "Raise exception when Data is null")]
        public void GetResponseJSONTestNullData()
        {
            int passed = 200;
            Response.GetResponseJSON(passed, null);
        }

        [TestCategory("Response")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "Raise exception when Data is empty")]
        public void GetResponseJSONTestEmptyData()
        {
            int passed = 200;
            Response.GetResponseJSON(passed, new byte[0]);
        }

        //ResponseXML

        [TestCategory("Response")]
        [TestMethod()]
        public void GetResponseXMLTestCorrect()
        {
            int passed = 200;
            byte[] data = new byte[5];

            Assert.IsNotNull(Response.GetResponseXML(passed, data));
        }

        [TestCategory("Response")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "Raise exception when Status is zero")]
        public void GetResponseXMLTestStatusZero()
        {
            byte[] data = new byte[0];

            Response.GetResponseXML(0, data);
        }

        [TestCategory("Response")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "Raise exception when Data is null")]
        public void GetResponseXMLTestNullData()
        {
            int passed = 200;
            Response.GetResponseXML(passed, null);
        }

        [TestCategory("Response")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "Raise exception when Data is empty")]
        public void GetResponseXMLTestEmptyData()
        {
            int passed = 200;
            Response.GetResponseXML(passed, new byte[0]);
        }
    }
}