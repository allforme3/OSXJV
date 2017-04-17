using OSXJV.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Newtonsoft.Json.Linq;

namespace OSXJV.Classes.Tests
{
    [TestFixture]
    public class OutputTests
    {
        [Test]
        public void OutputTest()
        {
            string xml = "<?xml version=\"1.0\"?><doc></doc>";
            string type = "XML";
            ProcessDocument process = ProcessDocument.GetProcess(xml, type);
            Node n = process.Process();
            Output output = new Output(n);
            Assert.IsNotNull(output);
            Assert.AreEqual(typeof(Output), output.GetType());
        }

        [Test]
        public void CreateGridTest()
        {

            JObject expected = JObject.Parse("{\"text\":\"doc\",\"id\":1,\"state\":{\"selected\":true}}");
            string xml = "<?xml version=\"1.0\"?><doc></doc>";
            string type = "XML";

            ProcessDocument process = ProcessDocument.GetProcess(xml, type);
            Node n = process.Process();
            Output output = new Output(n);

            Assert.AreEqual(expected, output.CreateGrid());
        }

        [Test]
        public void CreateViewTest()
        {
            string expected = "<div class='text-center ui-layout-center ui-layout-pane ui-layout-pane-center'><div style ='display:inline-block' class='ui-selectable ui-droppable'><div id='1'class='node type ui-draggable ui-selectee' style='left:100px; top:130px;'><div class='head'><span><button class='nameBtn' onclick='GetNode(1)'>doc</button></span></div></div></div></div></div>";
            string xml = "<?xml version=\"1.0\"?><doc></doc>";
            string type = "XML";

            ProcessDocument process = ProcessDocument.GetProcess(xml, type);
            Node n = process.Process();
            Output output = new Output(n);
            Console.WriteLine(expected);
            Console.WriteLine(output.CreateView());
            Assert.AreEqual(expected, output.CreateView());
        }

        [Test]
        public void OutputTestNodesNull()
        {
            Assert.Throws< ArgumentException>(() => new Output(null));
        }
    }
}