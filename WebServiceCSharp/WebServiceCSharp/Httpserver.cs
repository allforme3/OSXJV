using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace Web
{ 
    public class Httpserver
    {
        private int port = 8080;
        public static bool running = false;
        private HttpListener listener;
        public static DateTime lastRequest;
        public const string VERSION = "HTTP/1.1";
        public const string NAME = "OSXJV Server";
        public static int numRequests = 0;
        public static string lastclientaddress = "";
        public static int numOfConnections = 0;

        public static TimeSpan time; 
        public static bool computing;
        public static string lastrequest = "";
        public static Thread search;

        public Httpserver()
        {
            listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:" + port + "/");
        }
        public void Start()
        {            
            Thread serverThread = new Thread(new ThreadStart(Run));
            serverThread.Start();
        }
        public void Run()
        {
            running = true;
            listener.Start();
            while(listener.IsListening)
            {
                
                Console.WriteLine("Waiting");
                HttpListenerContext hlc= listener.GetContext();
                lastRequest = DateTime.Now;
                numRequests++;
                lastclientaddress = hlc.Request.UserHostAddress;
                Console.WriteLine("{0} {1} {2}",hlc.Request.ContentEncoding,hlc.Request.UserAgent,hlc.Request.UserHostAddress);
                Thread response = new Thread(() =>
                {
                    try
                    {
                        numOfConnections++;                        
                        HandleClient(hlc);
                        numOfConnections--;
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("Error creating Response {0}",e.ToString());
                        numOfConnections--;
                    }
                });
                response.Start();
            }
            listener.Stop();
            
        }

        private void HandleClient(HttpListenerContext c)
        {
            string request = "";
            if (c.Request.HttpMethod.Equals("POST"))
            {
                
                if (c.Request.HasEntityBody)
                {
                    using (StreamReader sr = new StreamReader(c.Request.InputStream))
                    {
                        request += sr.ReadToEnd();
                    }
                }
            }
            Request req = Request.GetRequest(request);
            Response resp = Response.From(req);
            resp.Post(c.Response);
        }
    }
}
