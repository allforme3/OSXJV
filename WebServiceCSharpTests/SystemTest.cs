using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using OSXJV.Classes;

namespace OSXJV.Server.Test
{
	/// <summary>
	/// Summary description for SystemTest
	/// </summary>

	//Running Test Correct on Window system, Mono cannot use WebRequest Unsupported.
	[TestFixture]
	public class SystemTest
	{       
		[Test]
		[TestCase(@"<?xml version='1.0'?><doc></doc>", "application/xml", "<div class='text-center ui-layout-center ui-layout-pane ui-layout-pane-center'><div style ='display:inline-block' class='ui-selectable ui-droppable'><div id='1'class='node type ui-draggable ui-selectee' style='left:100px; top:130px;'><div class='head'><span><button class='nameBtn' onclick='GetNode(1)'>doc</button></span></div></div></div></div></div>")]
		[TestCase(@"{ 'doc' : {}}", "application/json", "<div class='text-center ui-layout-center ui-layout-pane ui-layout-pane-center'><div style ='display:inline-block' class='ui-selectable ui-droppable'><div id='1'class='node type ui-draggable ui-selectee' style='left:100px; top:130px;'><div class='head'><span><button class='nameBtn' onclick='GetNode(1)'>doc</button></span></div></div></div></div></div>")]
		[TestCase("{'PportTimetable':{'@xmlns:xsi':'http://www.w3.org/2001/XMLSchema-instance','@xmlns:xsd':'http://www.w3.org/2001/XMLSchema','@timetableID':'20160607020820','@xmlns':'http://www.thalesgroup.com/rtti/XmlTimetable/v8','Journey':[{'@rid':'201606072611040','@uid':'C70888','@trainId':'5E38','@ssd':'2016-06-07','@toc':'XC','@trainCat':'EE','@isPassengerSvc':'false','OPOR':{'@tpl':'BTNUNMD','@act':'TB','@wtd':'05:24'},'PP':[{'@tpl':'BTNUNSJ','@wtp':'05:29:30'},{'@tpl':'WICHNRJ','@wtp':'05:31:30'},{'@tpl':'LNDRSTJ','@wtp':'06:06:30'},{'@tpl':'PROOFHJ','@wtp':'06:08'}],'OPDT':{'@tpl':'BHAMNWS','@act':'TF','@plat':'10A','@wta':'06:11'}}]}}","application/json","<div class='text-center ui-layout-center ui-layout-pane ui-layout-pane-center'><div style ='display:inline-block' class='ui-selectable ui-droppable'><div id='1'class='node type ui-draggable ui-selectee' style='left:100px; top:130px;'><div class='head'><span><button class='nameBtn' onclick='GetNode(1)'>PportTimetable</button></span></div><div class='attribute'><div class='aHeader'><p><button><i class='fa fa-plus'></i></button>Attributes</p></div><div class='options'><div class='blockR'><p>xsi</p></div><div class='comment'><p>http://www.w3.org/2001/XMLSchema-instance</p></div><div class='blockR'><p>xsd</p></div><div class='comment'><p>http://www.w3.org/2001/XMLSchema</p></div><div class='blockR'><p>timetableID</p></div><div class='comment'><p>20160607020820</p></div><div class='blockR'><p>xmlns</p></div><div class='comment'><p>http://www.thalesgroup.com/rtti/XmlTimetable/v8</p></div></div></div></div><div id='2'class='node-child type ui-draggable ui-selectee' style='left:500px; top:130px;'><div class='head'><span><button class='nameBtn' onclick='GetNode(2)'>Journey</button></span></div><div class='attribute'><div class='aHeader'><p><button><i class='fa fa-plus'></i></button>Attributes</p></div><div class='options'><div class='blockR'><p>rid</p></div><div class='comment'><p>201606072611040</p></div><div class='blockR'><p>uid</p></div><div class='comment'><p>C70888</p></div><div class='blockR'><p>trainId</p></div><div class='comment'><p>5E38</p></div><div class='blockR'><p>ssd</p></div><div class='comment'><p>2016-06-07</p></div><div class='blockR'><p>toc</p></div><div class='comment'><p>XC</p></div><div class='blockR'><p>trainCat</p></div><div class='comment'><p>EE</p></div><div class='blockR'><p>isPassengerSvc</p></div><div class='comment'><p>false</p></div><div class='blockR'><p>xmlns</p></div><div class='comment'><p>http://www.thalesgroup.com/rtti/XmlTimetable/v8</p></div></div></div></div></div></div>")]
		public void TestCorrectDocument(string data,string type,string expected)
		{
		
			Console.WriteLine ("TestCorrectDocument");

			Type t = Type.GetType ("Mono.Runtime");
			if (t != null) {
				Console.WriteLine ("Running Test Correct on Window system, Mono cannot use WebRequest Unsupported.");
				Console.WriteLine ("Test Skipped");
			} else {
				Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
				string arg = "";
				string arg2 = "";

				string dir = Directory.GetCurrentDirectory ();
				arg = dir + "/Logger/";
				arg2 = dir + "/Cache/";

				if (!Directory.Exists (arg))
					Directory.CreateDirectory (arg);
				if (!Directory.Exists (arg2))
					Directory.CreateDirectory (arg2);

				OSXJVServer server = new OSXJVServer ();

				server.Start (arg,arg2);

				WebRequest request = WebRequest.Create ("http://localhost:8082/Process");
				request.ContentType = type;
				request.Method = WebRequestMethods.Http.Post;

				byte[] bytes = Encoding.ASCII.GetBytes (data);
				request.ContentLength = bytes.Length;
				Stream os = request.GetRequestStream ();
				os.Write (bytes, 0, bytes.Length);
				os.Close ();
				WebResponse resp = request.GetResponse ();

				StreamReader sr =
					new StreamReader (resp.GetResponseStream ());

				JObject obj = JObject.Parse (sr.ReadToEnd ());

				Console.WriteLine (obj.ToString ());
				Console.WriteLine ("Actual: {0}", obj.Property ("view").Value.ToString ());
				Console.WriteLine ("Expected: {0}", expected);

				Assert.AreEqual (obj.Property ("view").Value.ToString (), expected);

				server.Stop ();
			}
		}

		[Test]
		public void TestInvalidDocument()
		{
			Type t = Type.GetType ("Mono.Runtime");
			if (t != null) {
				Console.WriteLine ("Running Test Correct on Window system, Mono cannot use WebRequest Unsupported.");
				Console.WriteLine ("Test Skipped");
			} else {
				Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
				string arg = "";
				string arg2 = "";

				string dir = Directory.GetCurrentDirectory ();
				arg = dir + "/Logger/";
				arg2 = dir + "/Cache/";

				if (!Directory.Exists (arg))
					Directory.CreateDirectory (arg);
				if (!Directory.Exists (arg2))
					Directory.CreateDirectory (arg2);

				OSXJVServer server = new OSXJVServer ();
				server.Start(arg,arg2);

				server.Stop ();
			}
		}
		
	}
}
