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
			Console.WriteLine ("CheckDocumentTestXMLCorrect");
			Assert.IsTrue(Validation.GetInstance().CheckDocument(xml, type),"Validation Passed");
        }

        [Test]
        public void CheckDocumentTestXMLIncorrect()
        {
			Console.WriteLine("CheckDocumentTestXMLIncorrect");
            string xml = "<somethinghere xml version=\"1.0\"?><doc></doc>";
            string type = "XML";
			Assert.Throws<XmlException>(() => Validation.GetInstance().CheckDocument(xml, type));
        }

        [Test]
        public void CheckDocumentTestXMLEmptyData()
        {
			Console.WriteLine("CheckDocumentTestXMLEmptyData");
            string xml = "";
            string type = "XML";
			Assert.Throws<ArgumentException>(() => Validation.GetInstance().CheckDocument(xml, type));
        }

        [Test]
        public void CheckDocumentTestXMLEmptyType()
        {
			Console.WriteLine("CheckDocumentTestXMLEmptyType");
            string xml = "<xml version=\"1.0\"?><doc></doc>";
            string type = "";
			Assert.Throws<ArgumentException>(() => Validation.GetInstance().CheckDocument(xml, type));
        }

        [Test]
        public void CheckDocumentTestXMLDataNull()
        {
			Console.WriteLine("CheckDocumentTestXMLDataNull");
            string type = "XML";
			Assert.Throws<ArgumentException>(() => Validation.GetInstance().CheckDocument(null, type));
        }
			
        [Test]
        public void CheckDocumentTestXMLTypeNull()
        {
			Console.WriteLine("CheckDocumentTestXMLTypeNull");
			string xml = "<xml version=\"1.0\"?><doc></doc>";
			Assert.Throws<ArgumentException>(() => Validation.GetInstance().CheckDocument(xml, null));
        }

        //JSON
        [Test]
        public void CheckDocumentTestJSONCorrect()
        {
			Console.WriteLine ("CheckDocumentTestJSONCorrect");
            string xml = "{ 'name':'Allan' }";
            string type = "JSON";
			Assert.IsTrue(Validation.GetInstance().CheckDocument(xml, type), "Validation Passed");
        }

        [Test]
        public void CheckDocumentTestJSONIncorrect()
        { 
			Console.WriteLine ("CheckDocumentTestJSONIncorrect");
            string xml = "{ 'name':'Allan' }}";
            string type = "JSON";
			Assert.Throws<JsonReaderException>(() => Validation.GetInstance().CheckDocument(xml, type));
        }

        [Test]
        public void CheckDocumentTestJSONEmptyData()
        {
			Console.WriteLine("CheckDocumentTestJSONEmptyData");
            string xml = "";
            string type = "JSON";
			Assert.Throws<ArgumentException>(() => Validation.GetInstance().CheckDocument(xml, type));
        }

        [Test]
        public void CheckDocumentTestJSONEmptyType()
        {
			Console.WriteLine ("CheckDocumentTestJSONEmptyType");
            string xml = "{ 'name':'Allan' }}";
            string type = "";
			Assert.Throws<ArgumentException>(() => Validation.GetInstance().CheckDocument(xml, type));
        }

        [Test]
        public void CheckDocumentTestJSONDataNull()
        {
			Console.WriteLine ("CheckDocumentTestJSONDataNull");
            string type = "JSON";
			Assert.Throws<ArgumentException>(() => Validation.GetInstance().CheckDocument(null, type));
        }

        [Test]
        public void CheckDocumentTestJSONTypeNull()
        {
			Console.WriteLine ("CheckDocumentTestJSONTypeNull");
            string xml = "{ 'name':'Allan' }";
			Assert.Throws<ArgumentException>(() => Validation.GetInstance().CheckDocument(xml, null));
        }

        //Both
        [Test]
        public void CheckDocumentTestAllNull()
        {
			Console.WriteLine ("CheckDocumentTestAllNull");
			Assert.Throws<ArgumentException>(() => Validation.GetInstance().CheckDocument(null, null));
        }
    }
}