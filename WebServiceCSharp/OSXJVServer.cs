using System;
using System.Text;
using System.Net;
using System.Threading;
using System.IO;
using HttpMultipartParser;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using OSXJV.Classes;

namespace OSXJV.Server
{
    /// <summary>
    /// HTTPServer that process the incoming requests.
    /// </summary>
    public class OSXJVServer
    {
        private int port = 8082;

        /// <summary>
        /// True if the server is able to accept requests.
        /// </summary>
        public static bool running = false; //sets if the server is currently running
        private HttpListener listener;

        /// <summary>
        /// 
        /// </summary>
        private Thread serverThread = null;

        /// <summary>
        /// The Server Handler
        /// </summary>
        public OSXJVServer()
        {
            listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:" + port + "/"); //change if need be 
        }

        /// <summary>
        /// Starts server in new thread
        /// </summary>
        public bool Start()
        {            
            serverThread = new Thread(new ThreadStart(Run));
            try
            {
                serverThread.Start();
            }
            catch
            {}                        
            return serverThread.IsAlive;
        }

        /// <summary>
        /// Stop the listener and about all current requests
        /// </summary>
        public bool Stop()
        {
            if (listener != null)
                if (listener.IsListening)
                    listener.Abort();


            if (serverThread != null)
            {
                serverThread.Join();
                serverThread = null;
            }
            
            return serverThread == null ?true:false;
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

                //Wait for Listener
                IAsyncResult result = listener.BeginGetContext(new AsyncCallback(ListenerCallback), listener);
                result.AsyncWaitHandle.WaitOne();

                if (result.CompletedSynchronously)
                    Console.WriteLine("Completed Synchronously");

                /*
                 * Old Method of Creating a Thread
                 * 
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
                        Logger.GetInstance().WriteError(e.Message);
                        hlc.Response.StatusCode = 500;
                        hlc.Response.Close();
                    }
                });
                response.Start();
                *
                */
            }         
        }

        //Asyncronous Handler
        /// <summary>
        /// Handles Requests Asyncronously
        /// </summary>
        /// <param name="result">The Request Object Coming In.</param>
        private void ListenerCallback(IAsyncResult result)
        {
            HttpListener listener = (HttpListener)result.AsyncState;
            HttpListenerContext context = listener.EndGetContext(result);
            try
            {
                HandleClient(context);
            }
            catch (Exception e)
            {
                Logger.GetInstance().WriteError(e.Message);
                context.Response.StatusCode = 500;
                context.Response.Close();
            }
            
        }

        //Handles the client request
        /// <summary>
        /// Handles the client
        /// </summary>
        /// <param name="c">The Request</param>
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
                    c.Response.Close();
                    break;
                default:
                    Post(Response.GetInvalidRequestResponse(), c.Response);
                    break;
            }
        }

        /// <summary>
        /// Sends to the Client What the Server Supports
        /// </summary>
        /// <param name="response">The Request Response Object</param>
        private void HandleOptions(HttpListenerResponse response)
        {
            response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept, X-Requested-With");
            response.AddHeader("Access-Control-Allow-Methods", "POST");
            response.AddHeader("Access-Control-Allow-Methods", "GET");
            response.AddHeader("Access-Control-Max-Age", "1728000");
            response.AppendHeader("Access-Control-Allow-Origin", "*");      
        }

        /// <summary>
        /// Extract the files from the request
        /// </summary>
        /// <param name="input">Requests input stream</param>
        /// <returns>New Request Object</returns>
        /// <exception cref="System.InvalidOperationException">Thrown when no files are included with the request</exception>
        public Request GetFormData(Stream input)
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

        /// <summary>
        /// Get Data if the data is retrieved
        /// </summary>
        /// <param name="input">Client Stream Input</param>
        /// <param name="type">The MIME type</param>
        /// <returns>A Response object to send to the user</returns>
        private Request GetFileData(Stream input,string type)
        {
            string request = "";
            using (StreamReader ms = new StreamReader(input))
            {
                request = ms.ReadToEnd();
            }
            string filename = "temp";

            if (type == "text/xml")
                filename += ".xml";
            else if(type == "application/json")
                filename += ".json";
            else
                filename += ".html";

            return Request.GetRequest(filename,type, request);
        }

        /// <summary>
        /// Handles a POST request.
        /// </summary>
        /// <param name="req">The request to be processed.</param>
        /// <returns>A Response object to send to the user</returns>
        private Response HandlePost(HttpListenerRequest req)
        {
        
            JObject eRes = new JObject();

            if (SegmentNormalize(req.RawUrl).Equals("Process"))
            {
                if (req.HasEntityBody)
                {


                    Request r = null;
                    try
                    {
                        r = GetData(req);
                        if (r == null)
                            return Response.GetInvalidRequestResponse();
                    }
                    catch
                    {
                       return Response.GetInvalidRequestResponse();
                    }
                    
                       

                    try
                    {
                        Validation.CheckDocument(r.Data, r.Type);
                    }
                    catch (Exception e)
                    {
                        eRes.Add("Error", e.Message);
                        return Response.GetErrorResponse(eRes.ToString());
                    }

                    string id = Guid.NewGuid().ToString();
                    ProcessDocument pro = ProcessDocument.GetProcess(r.Data, r.Type);
                    Node n = pro.ProcessParallel();
                    Output o = new Output(n); //new output object
                    try
                    {
                        CacheManager.GetInstance().saveFile(id, JsonConvert.SerializeObject(n));
                        JObject response = new JObject();

                        n = null; //remove node as its completed;

                        response.Add("filename", id);
                        response.Add("grid", o.CreateGrid());
                        response.Add("view", o.CreateView());

                        

                        byte[] bytes = Encoding.UTF8.GetBytes(response.ToString());
                        return Response.GetResponse(200, "application/json", bytes);
                    }
                    catch (Exception e)
                    {
                        Logger.GetInstance().WriteError(e.Message);
                        eRes.Add("Error", "Error Creating Response");
                        return Response.GetErrorResponse(eRes.ToString());
                    }

                }
                eRes.Add("Error", "No File Recieved By Server");
                return Response.GetErrorResponse(eRes.ToString());
            }
            else if (req.RawUrl.Equals("/Output"))
            {
                return Response.GetInvalidRequestResponse();
            }
            else
                return Response.GetInvalidRequestResponse();
        }

        /// <summary>
        /// Handles a GET request.
        /// </summary>
        /// <param name="req">The request to be processed.</param>
        /// <returns>A Response object to send to the user</returns>
        private Response HandleGet(HttpListenerRequest req)
        {
            if (SegmentNormalize(req.Url.Segments[1]).Equals("Process"))
            {
                if (req.Url.Segments.Length == 4)
                {
                    
                    Node cached;
                    try
                    {
                        cached = JsonConvert.DeserializeObject<Node>(CacheManager.GetInstance().getFile(req.Url.Segments[2]));
                    }
                    catch (Exception e)
                    {
                        Logger.GetInstance().WriteError(e.Message);
                        JObject eRes = new JObject();
                        eRes.Add("Error", "Error Creating Response");
                        return Response.GetErrorResponse(eRes.ToString());
                    }
                    Output o = new Output(cached);
                    JObject response = new JObject();
                    response.Add("view", o.CreateView(int.Parse(req.Url.Segments[3])));
                    byte[] bytes = Encoding.UTF8.GetBytes(response.ToString());
                    return Response.GetResponse(200, "application/json", bytes);                    
                }
                else if (req.Url.Segments.Length == 5)
                {
                    
                    Node cached;
                    try
                    {
                        cached = JsonConvert.DeserializeObject<Node>(CacheManager.GetInstance().getFile(req.Url.Segments[2]));
                    }
                    catch (Exception e)
                    {
                        Logger.GetInstance().WriteError(e.Message);
                        JObject eRes = new JObject();
                        eRes.Add("Error", "Error Creating Response");
                        return Response.GetErrorResponse(eRes.ToString());
                    }
                    Output o = new Output(cached);
                    JObject response = new JObject();
                    response.Add("view", o.CreateView(int.Parse(SegmentNormalize(req.Url.Segments[3])), 4, int.Parse(SegmentNormalize(req.Url.Segments[4]))));
                    byte[] bytes = Encoding.UTF8.GetBytes(response.ToString());
                    return Response.GetResponse(200, "application/json", bytes);                    
                }
                else
                    return Response.GetInvalidRequestResponse();
            }
            //If it got here its an invalid response.
            return Response.GetInvalidRequestResponse();
        }

        /// <summary>
        /// Save data recievied from client.
        /// </summary>
        /// <param name="id">Unique ID</param>
        /// <param name="nodes">The Processed Data</param>
        /// <exception cref="ArgumentException">Thrown when nodes is null or id is null or empty</exception>
        private void SaveFile(string id, Node nodes)
        {
            if(nodes == null || string.IsNullOrEmpty(id))
            {
                throw new ArgumentException();
            }

            try
            {
                CacheManager.GetInstance().saveFile(id, JsonConvert.SerializeObject(nodes));
            }
            catch (Exception e)
            {
                Logger.GetInstance().WriteError(e.Message);
            }
        }

        /// <summary>
        /// Send data to the client.
        /// </summary>
        /// <param name="res">The Response Object</param>
        /// <param name="stream">The Client Output Stream</param>
        /// /// <exception cref="ArgumentException">Thrown when Response is null or HttpListenerResponse is null or empty</exception>
        private void Post(Response res,HttpListenerResponse stream)
        {
            if (res == null || stream == null)
                throw new ArgumentException("Response or Client Stream cannot be NULL");

            HandleOptions(stream);
            stream.ProtocolVersion = new Version(1, 1);
            stream.StatusCode = res.status;
            stream.ContentType = res.mime;
            stream.ContentLength64 = res.data.Length;
            stream.OutputStream.Write(res.data, 0, res.data.Length);
            stream.Close();
        }

        /// <summary>
        /// Get the data from the client.
        /// </summary>
        /// <param name="req">The request from the client</param>
        /// <returns>A Request Object</returns>
        private Request GetData(HttpListenerRequest req)
        {
            Request r = null;

            if (req.ContentType.Contains("application/x-www-form-urlencoded"))
            {
                r = GetFormData(req.InputStream);
            }
            else if (req.ContentType.Contains("application/json") || req.ContentType.Contains("application/oclet-stream"))
            {
                r = GetFileData(req.InputStream, "application/json");
            }
            else if (req.ContentType.Contains("application/xml") || req.ContentType.Contains("text/xml"))
            {
                r = GetFileData(req.InputStream, "text/xml");
            }         
            return r;
        }

        /// <summary>
        /// Removes '/' from the string.
        /// </summary>
        /// <param name="input">A string from the URL</param>
        /// <returns>Normalised String</returns>
        private string SegmentNormalize(string input)
        {
            return input.Replace("/", "");
        }
    }
}
