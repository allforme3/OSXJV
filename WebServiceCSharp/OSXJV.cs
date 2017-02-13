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
    public class OSXJV
    {
        public string path = Path.GetDirectoryName(new Uri(typeof(int).Assembly.CodeBase).LocalPath);
        private int port = 8082;

        /// <summary>
        /// True if the server is able to accept requests.
        /// </summary>
        public static bool running = false; //sets if the server is currently running
        private HttpListener listener;

        /// <summary>
        /// Keeps location of cached files
        /// </summary>
        public static List<Tuple<string,DateTime>> cache = new List<Tuple<string,DateTime>>();

        /// <summary>
        /// The Server Handler
        /// </summary>
        public OSXJV()
        {
            listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:" + port + "/");
        }

        /// <summary>
        /// Starts server in new thread
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

        private void HandleClient(HttpListenerContext c)
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

        /// <summary>
        /// Extract the files from the request
        /// </summary>
        /// <param name="input">Requests input stream</param>
        /// <returns>New Request Object</returns>
        /// <exception cref="System.InvalidOperationException">Thrown when not files are included with the request</exception>
        public Request GetData(Stream input)
        {
            string request = "";
            MultipartFormDataParser parser = new MultipartFormDataParser(input);
            if (parser.Files.Count > 0)
            {
                using (StreamReader ms = new StreamReader(parser.Files[0].Data))
                {
                    request = ms.ReadToEnd();
                }
            }
            else
            {
                throw new InvalidOperationException();
            }

            return Request.GetRequest(parser.Files[0].FileName, parser.Files[0].ContentType, request);
        }

        private Response HandlePost(HttpListenerRequest req)
        {
            if (req.RawUrl.Equals("/Process"))
            {
                if (req.HasEntityBody)
                {
                    Request r = null;

                    try
                    {
                        r = GetData(req.InputStream);
                    }
                    catch
                    {
                        return Response.GetInvalidRequestResponse();
                    }

                    if (Validation.CheckDocument(r.Data, r.Type))
                    {
                        string id = Guid.NewGuid().ToString();
                        Process pro = Process.GetProcess(r.Data, r.Type);
                        Node n = pro.ProcessDocument();
                        Output o = new Output(n); //new output object
                        try
                        {
                            SaveFile(id, n);
                            JObject response = new JObject();
                            response.Add("filename", id);
                            response.Add("grid", o.CreateGrid());
                            response.Add("view", o.CreateView(1));
                            byte[] bytes = Encoding.UTF8.GetBytes(response.ToString());
                            return Response.GetResponse(200, "application/json", bytes);
                        }
                        catch
                        {
                            return Response.GetErrorResponse();
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
                    using (StreamReader sr = new StreamReader(@"" + path + "/" + r.Filename + ".json"))
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

        /// <summary>
        /// Save request's data
        /// </summary>
        /// <param name="id">Unique ID</param>
        /// <param name="nodes">The Processed Data</param>
        /// <returns>if successful</returns>
        /// <exception cref="ArgumentException">Thrown when nodes is null or id is null or empty</exception>
        /// <exception cref="IOException">Thrown when access to the disk is unavailable</exception>
        public void SaveFile(string id, Node nodes)
        {
            if(nodes == null || string.IsNullOrEmpty(id))
            {
                throw new ArgumentException();
            }
            try
            {
                using (StreamWriter sr = new StreamWriter(@"" + path +"/" + id + ".json"))
                {
                    sr.WriteLine(JsonConvert.SerializeObject(nodes));
                }
            }
            catch(IOException e)
            {
                throw e;
            }
        }

        private void Post(Response res,HttpListenerResponse stream)
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
