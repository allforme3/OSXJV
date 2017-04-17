using OSXJV.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using NUnit.Framework;

namespace OSXJV.Classes.Tests
{
    [TestFixture]
    public class ProcessTests
    {
       
        [Test]
        public void ProcessTest()
        {
            string xml = "<?xml version=\"1.0\"?><doc></doc>";
            string type = "XML";
            
            Assert.IsNotNull(ProcessDocument.GetProcess(xml, type));
        }

        [Test]
        public void ProcessTestNull()
        {
            Assert.Throws<ArgumentException>(() => ProcessDocument.GetProcess(null, null));
        }

        [Test]
        public void ProcessDocumentTestXMLReturnsTypeOfNode()
        {
            string xml = "<?xml version=\"1.0\"?><doc></doc>";
            string type = "XML";
            ProcessDocument pro = ProcessDocument.GetProcess(xml,type);            
            Assert.AreEqual(typeof(Node),pro.Process().GetType());
        }

        [Test]
        public void ProcessDocumentTestJSONReturnsTypeOfNode()
        {
            string json = "{ 'name':'jones'}";
            string type = "JSON";
            ProcessDocument pro = ProcessDocument.GetProcess(json, type);
            Assert.AreEqual(typeof(Node), pro.ProcessParallel().GetType());
        }

        [Test]
        public void ProcessDocumentTestInvalidData()
        {
            string xml = "</////trtgrg xml version=\"1.0\"?><doc></doc>";
            Assert.Throws<XmlException>(() => ProcessDocument.GetProcess(xml, "XML"));
        }

        [Test]
        public void TestingCorrectXML()
        {
            string xml = "<?xml version=\"1.0\"?><doc></doc>";
            string type = "XML";
            ProcessDocument process = ProcessDocument.GetProcess(xml, type);
            Node test = new Node();
            test.Name = "doc";
            test.Number = 1;
            test.Visited = true;
            Node n = process.ProcessParallel();

            Assert.AreEqual(test.Value, n.Value);
            Assert.AreEqual(test.Name, n.Name);
            Assert.AreEqual(test.Children.Count, n.Children.Count);
            Assert.AreEqual(test.Visited, n.Visited);


        }

        [Test]
        public void TestingCorrectJSON()
        {
            string json = "{ 'name':'jones'}";
            string type = "JSON";
            ProcessDocument process = ProcessDocument.GetProcess(json, type);

            Node root = new Node();
            root.Name = "name";
            root.Number = 1;
            root.Visited = true;
            root.Value = "jones";
          
            Node n = process.Process();

            Assert.AreEqual(root.Value, n.Value);
            Assert.AreEqual(root.Name, n.Name);
            Assert.AreEqual(root.Visited, n.Visited);
        }

        [Test]
        [TestCase("{ 'name':'jones'}")]
        public void TestingSingleThreadAndMultiThreadSimularity(string json)
        {
            string type = "JSON";
            ProcessDocument process = ProcessDocument.GetProcess(json, type);
            Console.WriteLine(process);

            Node n = process.Process();
            process = ProcessDocument.GetProcess(json, type);
            Node n2 = process.ProcessParallel();

            Assert.AreEqual(n2.Value, n.Value);
            Assert.AreEqual(n2.Name, n.Name);
            Assert.AreEqual(n2.Children.Count, n.Children.Count);
            Assert.AreEqual(n2.Visited, n.Visited);
        }
    }
}