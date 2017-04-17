using System;

namespace OSXJV.Classes
{
    /// <summary>
    /// A object containing the document to process, filename and type.
    /// </summary>
    public class Request
    {
        /// <summary>
        /// Document Filename.
        /// </summary>
        private string filename;

        /// <summary>
        /// Type of document.
        /// </summary>
        private string type;

        /// <summary>
        /// Contents of documents.
        /// </summary>
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
        /// Creates an instance of Request.
        /// </summary>
        /// <param name="filename">The document filename e.g. Test</param>
        /// <param name="type">The document file type e.g. text/xml</param>
        /// <param name="data">The document data e.g. {"name":"bob,"address":"123 Somewhere"}"</param>
        /// <returns>Object of Request</returns>
        public static Request GetRequest(string filename, string type, string data)
        {
            string Type = "";
            if (string.IsNullOrEmpty(filename) || string.IsNullOrEmpty(type) || string.IsNullOrEmpty(data))
                throw new ArgumentException();
            if (type.Equals("text/xml") || type.Equals("application/xml"))
            {
                Type = "XML";
            }
            else if(type.Equals("text/html"))
            {
                Type = "HTML";
            }
            else if (type.Equals("application/json") || type.Equals("application/octet-stream"))
            {
                Type = "JSON";
            }
            return new Request(filename,Type,data);
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
