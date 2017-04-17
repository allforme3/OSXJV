using System;
using System.Xml;
using Newtonsoft.Json;
using NUnit.Framework;
using OSXJV.Classes;
namespace OSXJV.Classes.Tests
{
    [TestFixture]
    public class ValidationTests
    {
        //XML
        [TestCase("<?xml version=\"1.0\"?><doc></doc>", "XML")]
        [TestCase("<?xml version=\"1.0\"?><doc><name>Allan</name><address>somewhere</address></doc>","XML")]
        public void CheckDocumentTestXMLCorrect(string xml,string type)
        {
            Assert.IsTrue(Validation.CheckDocument(xml, type),"Validation Passed");
        }

        [Test]
        public void CheckDocumentTestXMLIncorrect()
        {
            string xml = "<somethinghere xml version=\"1.0\"?><doc></doc>";
            string type = "XML";
            Assert.Throws<XmlException>(() => Validation.CheckDocument(xml, type));
        }

        [Test]
        public void CheckDocumentTestXMLEmptyData()
        {
            string xml = "";
            string type = "XML";
            Assert.Throws<ArgumentException>(() => Validation.CheckDocument(xml, type));
        }

        [Test]
        public void CheckDocumentTestXMLEmptyType()
        {
            string xml = "<xml version=\"1.0\"?><doc></doc>";
            string type = "";
            Assert.Throws<ArgumentException>(() => Validation.CheckDocument(xml, type));
        }

        [Test]
        public void CheckDocumentTestXMLDataNull()
        {
            string type = "XML";
            Assert.Throws<ArgumentException>(() => Validation.CheckDocument(null, type));
        }

        [Test]
        public void CheckDocumentTestXMLTypeNull()
        {
            string xml = "<?xml version=\"1.0\"?><doc></doc>";
            Assert.Throws<ArgumentException>(() => Validation.CheckDocument(xml, null));
        }

        //JSON
        [Test]
        public void CheckDocumentTestJSONCorrect()
        {
            string xml = "{ 'name':'Allan' }";
            string type = "JSON";
            Assert.IsTrue(Validation.CheckDocument(xml, type), "Validation Passed");
        }

        [Test]
        public void CheckDocumentTestJSONIncorrect()
        { 
            string xml = "{ 'name':'Allan' }}";
            string type = "JSON";
            Assert.Throws<JsonReaderException>(() => Validation.CheckDocument(xml, type));
        }

        [Test]
        public void CheckDocumentTestJSONEmptyData()
        {
            string xml = "";
            string type = "JSON";
            Assert.Throws<ArgumentException>(() => Validation.CheckDocument(xml, type));
        }

        [Test]
        public void CheckDocumentTestJSONEmptyType()
        {
            string xml = "{ 'name':'Allan' }}";
            string type = "";
            Assert.Throws<ArgumentException>(() => Validation.CheckDocument(xml, type));
        }

        [Test]
        public void CheckDocumentTestJSONDataNull()
        {
            string type = "JSON";
            Assert.Throws<ArgumentException>(() => Validation.CheckDocument(null, type));
        }

        [Test]
        public void CheckDocumentTestJSONTypeNull()
        {
            string xml = "{ 'name':'Allan' }";
            Assert.Throws<ArgumentException>(() => Validation.CheckDocument(xml, null));
        }

        //Both
        [Test]
        public void CheckDocumentTestAllNull()
        {
            Assert.Throws<ArgumentException>(() => Validation.CheckDocument(null, null));
        }
    }
}