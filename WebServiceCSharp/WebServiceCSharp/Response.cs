using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Globalization;
using System.Threading;
using System.Net;
using Newtonsoft.Json;

namespace Web
{
    public class file
    {
        private string f_location;

        [JsonProperty("file_location")]
        public string F_location
        {
            get
            {
                return f_location;
            }

            set
            {
                f_location = value;
            }
        }
    }
    class Response
    {
        private byte[] _buffer = null;
        private int status;
        private string mime;
        private Array[] objects;
        private const string dir = @"C:\wamp64\www\Assignment\";
        //static string format = "yyyy-MM-dd HH:mm:ss";

        public static DateTime now = DateTime.Now;

        private Response(int status,string mime,byte[] buffer)
        {
            this.status = status;
            _buffer = buffer;
            this.mime = mime;
        }
        public static Response From(Request request)
        {
            now = DateTime.Now;

            if (request == null)
            {
                return new Response(410, "text/html", new byte[0]);
            }
            if (request.Value != null)
            {
                string value = request.Value;
                string result = "";
                file f = JsonConvert.DeserializeObject<file>(value);
                using (StreamReader sr = new StreamReader(dir + f.F_location))
                {
                    result = sr.ReadToEnd();
                }
                byte[] Buffer = Encoding.ASCII.GetBytes(result);
                return new Response(200, "text/xml", Buffer);
            }
            return new Response(410, "text/html", new byte[0]);
        }

        public void Post(HttpListenerResponse stream)
        {
            stream.ProtocolVersion = new Version(1,1);
            stream.StatusCode = status;
            stream.ContentType = mime;
            stream.ContentLength64 = _buffer.Length;
            stream.OutputStream.Write(_buffer, 0, _buffer.Length);
            stream.Close();
        }
    }
}
