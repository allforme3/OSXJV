using WebServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace WebServer.Tests
{
    [TestFixture]
    public class OutputTests
    {
        [Test]
        public void OutputTest()
        {
            string xml = "<?xml version=\"1.0\"?><doc></doc>";
            string type = "text/xml";
            Process process = Process.GetProcess(xml, type);
            Node n = process.ProcessDocument();
            Output output = new Output(n);
            Assert.IsNotNull(output);
            Assert.AreEqual(typeof(Output), output.GetType());
        }

        [Test]
        public void CreateGridTest()
        {
            string expected = "{\r\n  \"text\": \"doc\",\r\n  \"id\": 1,\r\n  \"state\": {\r\n    \"selected\": true\r\n  }\r\n}";
            string xml = "<?xml version=\"1.0\"?><doc></doc>";
            string type = "text/xml";

            Process process = Process.GetProcess(xml, type);
            Node n = process.ProcessDocument();
            Output output = new Output(n);

            Assert.AreEqual(expected, output.CreateGrid());
        }

        [Test]
        public void CreateViewTest()
        {
            string expected = "<div class='text-center ui-layout-center ui-layout-pane ui-layout-pane-center'><div style ='display:inline-block' class='ui-selectable ui-droppable'></div></div>";
            string xml = "<?xml version=\"1.0\"?><doc></doc>";
            string type = "text/xml";

            Process process = Process.GetProcess(xml, type);
            Node n = process.ProcessDocument();
            Output output = new Output(n);

            Assert.AreEqual(expected, output.CreateView(0));
        }

        [Test]
        public void OutputTestNodesNull()
        {
            Assert.Throws< ArgumentException>(() => new Output(null));
        }
    }
}