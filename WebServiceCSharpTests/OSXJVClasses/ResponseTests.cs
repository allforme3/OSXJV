using OSXJV.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
namespace OSXJV.Classes.Tests
{
    [TestFixture]
    public class ResponseTests
    {
        [Test]
        public void GetResponseTestCorrect()
        {
			Console.WriteLine ("GetResponseTestCorrect");
            int passed = 200;
            string mime = "application/json";
            byte[] data = new byte[5];

            Assert.IsNotNull(Response.GetResponse(passed, mime, data));
        }

        [Test]
        public void GetResponseTestGetInvalidResponse()
        {
			Console.WriteLine ("GetResponseTestGetInvalidResponse");
            Response response = Response.GetInvalidRequestResponse();

            Assert.AreEqual(405, response.status);
            Assert.AreEqual("text/html", response.mime);
            Assert.IsTrue(response.data.Length == 0);
        }

        [Test]
        public void GetResponseTestGetErrorResponse()
        {
			Console.WriteLine ("GetResponseTestGetErrorResponse");
            Response response = Response.GetErrorResponse("Error");

            Assert.AreEqual(400, response.status);
            Assert.AreEqual("text/html", response.mime);
            Assert.AreEqual("Error", response.data);
        }

        [Test]
        public void GetResponseTestStatusZero()
        {
			Console.WriteLine ("GetResponseTestStatusZero");
            string mime = "application/json";
            byte[] data = new byte[0];

            Assert.Throws<ArgumentException>(() => Response.GetResponse(0, mime, data));
        }

        [Test]
        public void GetResponseTestNullMime()
        {
			Console.WriteLine ("GetResponseTestNullMime");
            int passed = 200;
            byte[] data = new byte[0];

            Assert.Throws<ArgumentException>(() => Response.GetResponse(passed, null, data));
        }

        [Test]
        public void GetResponseTestNullData()
        {
			Console.WriteLine ("GetResponseTestNullData");
            int passed = 200;
            string mime = "application/json";

            Assert.Throws<ArgumentException>(() => Response.GetResponse(passed, mime, null));
        }


        [Test]
        public void GetResponseTestEmptyMime()
        {
			Console.WriteLine ("GetResponseTestEmptyMime");
            int passed = 200;
            byte[] data = new byte[0];

            Assert.Throws<ArgumentException>(() => Response.GetResponse(passed, "", data));
        }

        [Test]
        public void GetResponseTestEmptyData()
        {
			Console.WriteLine ("GetResponseTestEmptyData");
            int passed = 200;
            string mime = "application/json";

            Assert.Throws<ArgumentException>(() => Response.GetResponse(passed, mime, new byte[0]));
        }

        //ResponseJSON
        [Test]
        public void GetResponseJSONTestCorrect()
        {
			Console.WriteLine ("GetResponseJSONTestCorrect");
            int passed = 200;
            byte[] data = new byte[5];

            Assert.IsNotNull(Response.GetResponseJSON(passed, data));
        }
        [Test]
        public void GetResponseJSONTestStatusZero()
        {
			Console.WriteLine ("GetResponseJSONTestStatusZero");
            byte[] data = new byte[0];

            Assert.Throws<ArgumentException>(() => Response.GetResponseJSON(0, data));
        }

        [Test]
        public void GetResponseJSONTestNullData()
        {
			Console.WriteLine ("GetResponseJSONTestNullData");
            int passed = 200;
            Assert.Throws<ArgumentException>(() => Response.GetResponseJSON(passed, null));
        }

        [Test]
        public void GetResponseJSONTestEmptyData()
		{
			Console.WriteLine ("GetResponseJSONTestEmptyData");
            int passed = 200;
            Assert.Throws<ArgumentException>(() => Response.GetResponseJSON(passed, new byte[0]));
        }

        //ResponseXML

        [Test]
        public void GetResponseXMLTestCorrect()
        {
			Console.WriteLine ("GetResponseXMLTestCorrect");
            int passed = 200;
            byte[] data = new byte[5];

            Assert.IsNotNull(Response.GetResponseXML(passed, data));
        }

        [Test]
        public void GetResponseXMLTestStatusZero()
        {
			Console.WriteLine ("GetResponseXMLTestStatusZero");
            byte[] data = new byte[0];

            Assert.Throws<ArgumentException>(() => Response.GetResponseXML(0, data));
        }

        [Test]
        public void GetResponseXMLTestNullData()
        {
			Console.WriteLine ("GetResponseXMLTestNullData");
            int passed = 200;
            Assert.Throws<ArgumentException>(() => Response.GetResponseXML(passed, null));
        }

        [Test]
        public void GetResponseXMLTestEmptyData()
        {
			Console.WriteLine ("GetResponseXMLTestEmptyData");
            int passed = 200;
            Assert.Throws<ArgumentException>(() => Response.GetResponseXML(passed, new byte[0]));
        }
    }
}