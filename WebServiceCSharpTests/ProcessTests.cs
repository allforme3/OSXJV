using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;

namespace WebServer.Tests
{
    [TestClass()]
    public class ProcessTests
    {
        
        [TestCategory("Process Tests")]
        [TestMethod()]
        public void ProcessTest()
        {
            string xml = "<?xml version=\"1.0\"?><doc></doc>";
            string type = "text/xml";
            
            Assert.IsNotNull(Process.GetProcess(xml, type));
        }

        [TestCategory("Process Tests")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "Raise exception when data is null")]
        public void ProcessTestNull()
        {
            Assert.IsNull(Process.GetProcess(null, null));
        }

        [TestCategory("Process Tests")]
        [TestMethod()]
        public void ProcessDocumentTestXMLReturnsTypeOfNode()
        {
            string xml = "<?xml version=\"1.0\"?><doc></doc>";
            string type = "text/xml";
            Process pro = Process.GetProcess(xml,type);            
            Assert.AreEqual(typeof(Node),pro.ProcessDocument().GetType());
        }

        [TestCategory("Process Tests")]
        [TestMethod()]
        public void ProcessDocumentTestJSONReturnsTypeOfNode()
        {
            string xml = "{ 'name':'jones'}";
            string type = "application/json";
            Process pro = Process.GetProcess(xml, type);
            Assert.AreEqual(typeof(Node), pro.ProcessDocument().GetType());
        }

        [TestCategory("Process Tests")]
        [TestMethod()]
        [ExpectedException(typeof(XmlException))]
        public void ProcessDocumentTestInvalidData()
        {
            string xml = "</////trtgrg xml version=\"1.0\"?><doc></doc>";
            Process pro = Process.GetProcess(xml, "text/xml");
        }

        [TestCategory("Process Tests")]
        [TestMethod()]
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

        [TestCategory("Process Tests")]
        [TestMethod()]
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