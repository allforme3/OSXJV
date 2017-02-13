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
using System.Net.Http;
using System.Net.Http.Headers;
using HttpMultipartParser;
using System.Web;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace WebServer
{ 
    /// <summary>
    /// HTTPServer that process the incoming requests.
    /// </summary>
    public class Server
    {
        /// <summary>
        /// Port to listen to one
        /// </summary>
        private int port = 8080;
        /// <summary>
        /// True if the server is able to accept requests.
        /// </summary>
        public static bool running = false; //sets if the server is currently running
        /// <summary>
        /// 
        /// </summary>
        private HttpListener listener;

        /// <summary>
        /// 
        /// </summary>
        public static List<Tuple<string,DateTime>> cache = new List<Tuple<string,DateTime>>();
        /// <summary>
        /// 
        /// </summary>

        public Server()
        {
            listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:" + port + "/");
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Start()
        {            
            Thread serverThread = new Thread(new ThreadStart(Run));
            try
            {
                serverThread.Start();
            }
            catch
            {}                        
            return serverThread.IsAlive;
        }

        /// <summary>
        /// Function that constantly listens for connections
        /// </summary>
        public void Run()
        {
            running = true;
            listener.Start();
            while(listener.IsListening)
            {
                
                Console.WriteLine("Waiting");
                HttpListenerContext hlc= listener.GetContext();
                Console.WriteLine("{0} {1} {2}",hlc.Request.ContentEncoding,hlc.Request.UserAgent,hlc.Request.UserHostAddress);
                Thread response = new Thread(() =>
                {
                    try
                    {
                        Console.WriteLine("Processing");
                        HandleClient(hlc);
                        Console.WriteLine("Finished");
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("Error creating Response {0}",e.ToString());
                    }
                });
                response.Start();
            }
            listener.Stop();
            running = false;
            
        }

        /// <summary>
        /// Handles the client connect and extracts the data being sent to the server.
        /// </summary>
        /// <remarks>
        /// This retrieves the user data via a stream and processes the multi form data recieved into objects and creates a response and sends it back to the user.
        /// </remarks>
        /// <param name="c"></param>
        public void HandleClient(HttpListenerContext c)
        {
            switch(c.Request.HttpMethod)
            {
                case "POST":
                    Post(HandlePost(c.Request),c.Response);
                    break;
                case "GET":
                    Post(HandleGet(c.Request), c.Response);
                    break;
                case "OPTIONS":
                    HandleOptions(c.Response);
                    break;
                default:
                    Post(Response.GetInvalidRequestResponse(), c.Response);
                    break;
            }
        }

        private void HandleOptions(HttpListenerResponse response)
        {
            response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept, X-Requested-With");
            response.AddHeader("Access-Control-Allow-Methods", "POST");
            response.AddHeader("Access-Control-Max-Age", "1728000");
            response.AppendHeader("Access-Control-Allow-Origin", "*");
            response.Close();          
        }

        private Response HandlePost(HttpListenerRequest req)
        {
            string request = "";
            MultipartFormDataParser parser = null;
            if (req.RawUrl.Equals("/Process"))
            {
                if (req.HasEntityBody)
                {
                    parser = new MultipartFormDataParser(req.InputStream);
                    if (parser.Files.Count > 0)
                    {
                        using (StreamReader ms = new StreamReader(parser.Files[0].Data))
                        {
                            request = ms.ReadToEnd();
                        }                                            
                        Request r = Request.GetRequest(parser.Files[0].FileName, parser.Files[0].ContentType, request);
                        if (Validation.CheckDocument(r.Data, r.Type))
                        {
                            string id = Guid.NewGuid().ToString();
                            Process pro = Process.GetProcess(r.Data, r.Type);
                            Node n = pro.ProcessDocument();
                            Output o = new Output(n); //new output object
                            if (SaveFile(id, n, r.Type))
                            {
                                JObject response = new JObject();
                                response.Add("filename", id);
                                response.Add("grid", o.CreateGrid());
                                response.Add("view", o.CreateView(1));
                                byte[] bytes = Encoding.UTF8.GetBytes(response.ToString());
                                return Response.GetResponse(200, "application/json", bytes);
                            }
                        }
                    }
                }
                return Response.GetErrorResponse();
            }
            else
                return Response.GetInvalidRequestResponse();
        }

        private Response HandleGet(HttpListenerRequest req)
        {
            if (req.Url.Segments[1].Equals("Process/"))
            {
                Node cached;
                Request r = Request.GetRequest(req.Url.Segments[2], null, req.Url.Segments[3]);
                try
                {
                    
                    using (StreamReader sr = new StreamReader(@"/" + r.Filename + ".json"))
                    {
                        cached = JsonConvert.DeserializeObject<Node>(sr.ReadToEnd());
                    }
                }
                catch
                {
                    return Response.GetErrorResponse();
                }

                Output o = new Output(cached);
                JObject response = new JObject();
                response.Add("view", o.CreateView(int.Parse(r.Data)));
                byte[] bytes = Encoding.UTF8.GetBytes(response.ToString());           
                return Response.GetResponse(200,"application/json",bytes);
            }
            else
                return Response.GetInvalidRequestResponse();
        }

        private bool SaveFile(string id, Node nodes,string type)
        {
            try
            {
                using (StreamWriter sr = new StreamWriter(@"/" + id + ".json"))
                {
                    sr.WriteLine(JsonConvert.SerializeObject(nodes));
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="res"></param>
        /// <param name="stream"></param>
        public void Post(Response res,HttpListenerResponse stream)
        {
            stream.ProtocolVersion = new Version(1, 1);
            stream.StatusCode = res.status;
            stream.ContentType = res.mime;
            stream.ContentLength64 = res.data.Length;
            stream.OutputStream.Write(res.data, 0, res.data.Length);
            stream.Close();
        }
    }
}
