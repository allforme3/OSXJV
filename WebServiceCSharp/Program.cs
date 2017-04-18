using System;
using System.Threading;
using System.IO;
using OSXJV.Classes;
using OSXJV.Server;

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
        /// <param name="args">Pass Cache Folder and Logger (Optional)</param>
        static void Main(string[] args)
        {

            if (args.Length == 0)
            {
                Console.WriteLine("Using Default Cache Directory Path and Logger Directory Path");
                string dir = Directory.GetCurrentDirectory();
                Array.Resize(ref args, 2);
                args[0] = dir + "/Cache/";
                args[1] = dir + "/Logger/";
                if (!Directory.Exists(args[0]))
                    Directory.CreateDirectory(args[0]);
                if (!Directory.Exists(args[1]))
                    Directory.CreateDirectory(args[1]);
            }

            if (args[0] == args[1])
            {
                Console.WriteLine("Cache location and Log location is the same. Please enter two different locations");
            }
            else
            {
                try
                {
                    OSXJVServer s = new OSXJVServer();
                    s.Start(args[0], args[1]);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Press any key to exit");
                    Console.Read();
                }
                             
            }
        }
    }
}
