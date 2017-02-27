using WebServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using NUnit.Framework;

namespace WebServer.Tests
{
    [TestFixture]
    public class ProcessTests
    {
       
        [Test]
        public void ProcessTest()
        {
            string xml = "<?xml version=\"1.0\"?><doc></doc>";
            string type = "text/xml";
            
            Assert.IsNotNull(Process.GetProcess(xml, type));
        }

        [Test]
        public void ProcessTestNull()
        {
            Assert.Throws<ArgumentException>(() => Process.GetProcess(null, null));
        }

        [Test]
        public void ProcessDocumentTestXMLReturnsTypeOfNode()
        {
            string xml = "<?xml version=\"1.0\"?><doc></doc>";
            string type = "text/xml";
            Process pro = Process.GetProcess(xml,type);            
            Assert.AreEqual(typeof(Node),pro.ProcessDocument().GetType());
        }

        [Test]
        public void ProcessDocumentTestJSONReturnsTypeOfNode()
        {
            string xml = "{ 'name':'jones'}";
            string type = "application/json";
            Process pro = Process.GetProcess(xml, type);
            Assert.AreEqual(typeof(Node), pro.ProcessDocument().GetType());
        }

        [Test]
        public void ProcessDocumentTestInvalidData()
        {
            string xml = "</////trtgrg xml version=\"1.0\"?><doc></doc>";
            Assert.Throws<XmlException>(() => Process.GetProcess(xml, "text/xml"));
        }

        [Test]
        public void TestingCorrectXML()
        {
            string xml = "<?xml version=\"1.0\"?><doc></doc>";
            string type = "text/xml";
            Process process = Process.GetProcess(xml, type);
            Node test = new Node();
            test.Name = "doc";
            test.Number = 1;
            test.Visited = true;
            Node n = process.ProcessDocument();

            Assert.AreEqual(test.Value, n.Value);
            Assert.AreEqual(test.Name, n.Name);
            Assert.AreEqual(test.Children.Count, n.Children.Count);
            Assert.AreEqual(test.Visited, n.Visited);


        }

        [Test]
        public void TestingCorrectJSON()
        {
            string xml = "{ 'name':'jones'}";
            string type = "application/json";
            Process process = Process.GetProcess(xml, type);

            Node root = new Node();
            root.Name = "Root";
            root.Number = 1;
            root.Visited = true;

            Node test = new Node();
            test.Name = "name";
            test.Number = 1;
            test.Visited = true;
            test.Value = "jones";

            root.Children.Add(test);
            Node n = process.ProcessDocument();

            Assert.AreEqual(root.Value, n.Value);
            Assert.AreEqual(root.Name, n.Name);
            Assert.AreEqual(root.Children.Count, n.Children.Count);
            Assert.AreEqual(root.Visited, n.Visited);
        }
    }
}