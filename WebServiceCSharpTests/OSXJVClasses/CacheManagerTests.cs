
using NUnit.Framework;
using System;
using System.IO;
using Newtonsoft.Json;

namespace OSXJV.Classes.Tests
{
    [TestFixture]
    public class CacheManagerTests
    {

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
        }

        [Test]
        public void SetupTest()
        {
            string arg = "";

            string dir = Directory.GetCurrentDirectory();
            arg = dir + "/Cache/";
            if (!Directory.Exists(arg))
                Directory.CreateDirectory(arg);

            Assert.IsTrue(CacheManager.Setup(arg));
        }

        [Test]
        public void GetInstanceTest()
        {
            string arg = "";

            string dir = Directory.GetCurrentDirectory();
            arg = dir + "/Cache/";
            if (!Directory.Exists(arg))
                Directory.CreateDirectory(arg);
            CacheManager.Setup(arg);
            Assert.IsNotNull(CacheManager.GetInstance());
        }

        [Test]
        public void getFileTest()
        {
            string arg = "";

            string dir = Directory.GetCurrentDirectory();
            arg = dir + "/Cache/";
            if (!Directory.Exists(arg))
                Directory.CreateDirectory(arg);
            CacheManager.Setup(arg);
            Node root = new Node();
            root.Name = "name";
            root.Number = 1;
            root.Visited = true;
            root.Value = "jones";

            string filename = "test";
            CacheManager.GetInstance().saveFile(filename, JsonConvert.SerializeObject(root));
            Assert.IsTrue(File.Exists(arg + filename + ".json"));

            Assert.IsNotNull(CacheManager.GetInstance().getFile(filename));
        }

        [Test]
        public void saveFileTest()
        {
            string arg = "";

            string dir = Directory.GetCurrentDirectory();
            arg = dir + "/Cache/";
            if (!Directory.Exists(arg))
                Directory.CreateDirectory(arg);
            CacheManager.Setup(arg);
            Node root = new Node();
            root.Name = "name";
            root.Number = 1;
            root.Visited = true;
            root.Value = "jones";

            string filename = "test";
            CacheManager.GetInstance().saveFile(filename, JsonConvert.SerializeObject(root));
            Assert.IsTrue(File.Exists(arg + filename + ".json"));

        }

        [Test]
        public void GetCacheManagerWithoutSetupException()
        {
            Assert.Throws<Exception>(() => CacheManager.GetInstance());
        }

        [Test]
        public void TestCacheManagerClose()
        {
            string arg = "";

            string dir = Directory.GetCurrentDirectory();
            arg = dir + "/Cache/";
            if (!Directory.Exists(arg))
                Directory.CreateDirectory(arg);
            CacheManager.Setup(arg);
            CacheManager.Close();

            Assert.Throws<Exception>(() => CacheManager.GetInstance());
        }
    }
}