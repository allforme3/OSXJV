using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Net;

namespace WebServer
{
    /// <summary>
    /// The Initialiser
    /// </summary>
    class Program
    {
        /// <summary>
        /// The Main function that starts the HttpServer
        /// </summary>
        /// <param name="args">Not Used</param>
        static void Main(string[] args)
        {
            string xml = "<?xml version=\"1.0\"?><doc></doc>";
            string type = "text/xml";
            Process process = Process.GetProcess(xml, type);
            Node n = process.ProcessDocument();
            Output output = new Output(n);

            Server s = new Server();
            s.Start();
        }
    }
}
