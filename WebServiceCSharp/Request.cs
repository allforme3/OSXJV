using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WebServer
{
    /// <summary>
    /// 
    /// </summary>
    public class Request
    {
        /// <summary>
        /// 
        /// </summary>
        private string filename;
        private string type;
        private string data;

        /// <summary>
        /// Initialises the Request object, can only be called from GetRequest(...).
        /// </summary>
        /// <param name="filename">The document filename e.g. Test</param>
        /// <param name="type">The document file type e.g. text/xml</param>
        /// <param name="data">The document data e.g. {"name":"bob,"address":"123 Somewhere"}"</param>
        private Request(string filename, string type, string data)
        {
            this.filename = filename;
            this.type = type;
            this.data = data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename">The document filename e.g. Test</param>
        /// <param name="type">The document file type e.g. text/xml</param>
        /// <param name="data">The document data e.g. {"name":"bob,"address":"123 Somewhere"}"</param>
        /// <returns></returns>
        public static Request GetRequest(string filename, string type, string data)
        {
            if (string.IsNullOrEmpty(filename) || string.IsNullOrEmpty(type) || string.IsNullOrEmpty(data))
                throw new ArgumentException();

            return new Request(filename,type,data);
        }

        /// <summary>
        /// To retrieve the filename of the document
        /// </summary>
        public string Filename
        {
            get
            {
                return filename;
            }

            set
            {
                filename = value;
            }
        }

        /// <summary>
        /// To retrieve type of document
        /// </summary>
        public string Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }

        /// <summary>
        /// To retrieve the document data
        /// </summary>
        public string Data
        {
            get
            {
                return data;
            }

            set
            {
                data = value;
            }
        }
    }
}
