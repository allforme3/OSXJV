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
            OSXJV s = new OSXJV();
            s.Start();
        }
    }
}
