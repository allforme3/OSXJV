using NUnit.Framework;
using OSXJV.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSXJV.Classes.Tests
{
    [TestFixture]
    public class LoggerTests
    {
        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
			Console.WriteLine ("Setup Logger");
            Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
        }

        [Test]
        public void SetupTest()
        {
			Console.WriteLine ("SetupTest");
            string arg = "";

            string dir = Directory.GetCurrentDirectory();
            arg = dir + "/Logger/";
            if (!Directory.Exists(arg))
                Directory.CreateDirectory(arg);

            Assert.IsTrue(Logger.Setup(arg));

            Logger.Close();
        }

        [Test]
        public void GetInstanceTest()
		{
			Console.WriteLine ("SetupTest");
            string arg = "";

            string dir = Directory.GetCurrentDirectory();
            arg = dir + "/Logger/";
            if (!Directory.Exists(arg))
                Directory.CreateDirectory(arg);
            Logger.Setup(arg);
            Assert.IsNotNull(Logger.GetInstance());

            Logger.Close();
        }

        [Test]
        public void TestLoggerClose()
        {
			Console.WriteLine ("TestLoggerClose");
            string arg = "";

            string dir = Directory.GetCurrentDirectory();
            arg = dir + "/Logger/";
            if (!Directory.Exists(arg))
                Directory.CreateDirectory(arg);
            Logger.Setup(arg);
            Logger.Close();

            Assert.Throws<Exception>(() => Logger.GetInstance());
        }

        [Test]
        public void WriteErrorTest()
        {
			Console.WriteLine ("WriteErrorTest");
            string arg = "";

            string dir = Directory.GetCurrentDirectory();
            arg = dir + "/Logger/";
            if (!Directory.Exists(arg))
                Directory.CreateDirectory(arg);
            Logger.Setup(arg);

            Assert.IsNotNull(Logger.GetInstance());

            Assert.DoesNotThrow(() => Logger.GetInstance().WriteError("Test Error"));

            Logger.Close();
        }

        [Test]
        public void GetLoggerWithoutSetupException() 
        {
			Console.WriteLine ("GetLoggerWithoutSetupException");
            Assert.Throws<Exception>(() => Logger.GetInstance());
        }
    }
}