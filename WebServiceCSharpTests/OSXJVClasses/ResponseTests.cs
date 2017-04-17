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
            int passed = 200;
            string mime = "application/json";
            byte[] data = new byte[5];

            Assert.IsNotNull(Response.GetResponse(passed, mime, data));
        }

        [Test]
        public void GetResponseTestGetInvalidResponse()
        {
            Response response = Response.GetInvalidRequestResponse();

            Assert.AreEqual(405, response.status);
            Assert.AreEqual("text/html", response.mime);
            Assert.IsTrue(response.data.Length == 0);
        }

        [Test]
        public void GetResponseTestGetErrorResponse()
        {
            Response response = Response.GetErrorResponse("Error");

            Assert.AreEqual(400, response.status);
            Assert.AreEqual("text/html", response.mime);
            Assert.AreEqual("Error", response.data);
        }

        [Test]
        public void GetResponseTestStatusZero()
        {
            string mime = "application/json";
            byte[] data = new byte[0];

            Assert.Throws<ArgumentException>(() => Response.GetResponse(0, mime, data));
        }

        [Test]
        public void GetResponseTestNullMime()
        {
            int passed = 200;
            byte[] data = new byte[0];

            Assert.Throws<ArgumentException>(() => Response.GetResponse(passed, null, data));
        }

        [Test]
        public void GetResponseTestNullData()
        {
            int passed = 200;
            string mime = "application/json";

            Assert.Throws<ArgumentException>(() => Response.GetResponse(passed, mime, null));
        }


        [Test]
        public void GetResponseTestEmptyMime()
        {
            int passed = 200;
            byte[] data = new byte[0];

            Assert.Throws<ArgumentException>(() => Response.GetResponse(passed, "", data));
        }

        [Test]
        public void GetResponseTestEmptyData()
        {
            int passed = 200;
            string mime = "application/json";

            Assert.Throws<ArgumentException>(() => Response.GetResponse(passed, mime, new byte[0]));
        }

        //ResponseJSON
        [Test]
        public void GetResponseJSONTestCorrect()
        {
            int passed = 200;
            byte[] data = new byte[5];

            Assert.IsNotNull(Response.GetResponseJSON(passed, data));
        }
        [Test]
        public void GetResponseJSONTestStatusZero()
        {
            byte[] data = new byte[0];

            Assert.Throws<ArgumentException>(() => Response.GetResponseJSON(0, data));
        }

        [Test]
        public void GetResponseJSONTestNullData()
        {
            int passed = 200;
            Assert.Throws<ArgumentException>(() => Response.GetResponseJSON(passed, null));
        }

        [Test]
        public void GetResponseJSONTestEmptyData()
        {
            int passed = 200;
            Assert.Throws<ArgumentException>(() => Response.GetResponseJSON(passed, new byte[0]));
        }

        //ResponseXML

        [Test]
        public void GetResponseXMLTestCorrect()
        {
            int passed = 200;
            byte[] data = new byte[5];

            Assert.IsNotNull(Response.GetResponseXML(passed, data));
        }

        [Test]
        public void GetResponseXMLTestStatusZero()
        {
            byte[] data = new byte[0];

            Assert.Throws<ArgumentException>(() => Response.GetResponseXML(0, data));
        }

        [Test]
        public void GetResponseXMLTestNullData()
        {
            int passed = 200;
            Assert.Throws<ArgumentException>(() => Response.GetResponseXML(passed, null));
        }

        [Test]
        public void GetResponseXMLTestEmptyData()
        {
            int passed = 200;
            Assert.Throws<ArgumentException>(() => Response.GetResponseXML(passed, new byte[0]));
        }
    }
}