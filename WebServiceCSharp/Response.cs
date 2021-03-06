﻿using System;
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
using System.Xml.Linq;

namespace WebServer
{
    /// <summary>
    /// 
    /// </summary>
    public class Response
    {
        /// <summary>
        /// 
        /// </summary>
        public byte[] data = null;

        /// <summary>
        /// 
        /// </summary>
        public int status;

        /// <summary>
        /// 
        /// </summary>
        public string mime;
        //static string format = "yyyy-MM-dd HH:mm:ss";


        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        /// <param name="mime"></param>
        /// <param name="buffer"></param>
        private Response(int status,string mime,byte[] buffer)
        {
            this.status = status;
            this.data = buffer;
            this.mime = mime;
        }

        /// <summary>
        /// A custom response object
        /// </summary>
        /// <param name="status">The HTTP Code to send back e.g. 200 for success</param>
        /// <param name="type">Data type to send back e.g. application/json</param>
        /// <param name="data">The data to send</param>
        /// <returns></returns>
        public static Response GetResponse(int status,string type,byte[] data) 
        {                         
            if(string.IsNullOrEmpty(type))
                throw new ArgumentException("Type cannot be Null or empty");

            if (status.Equals(null))
                throw new ArgumentException("Status cannot be Null");
            else
               if (status == 0)
                throw new ArgumentException("Status cannot be 0");

            if (data == null)
                throw new ArgumentException("Data cannot be null");
            else
                if (data.Length == 0)
                throw new ArgumentException("No data, use invalid or error response");

            return new Response(status, type, data);
        }

        /// <summary>
        /// Return an application/json response
        /// </summary>
        /// <param name="status">The HTTP Code to send back e.g. 200 for success</param>
        /// <param name="data">The data to send</param>
        /// <returns>New response object</returns>
        public static Response GetResponseJSON(int status,byte[] data)
        {
            if (status.Equals(null))
                throw new ArgumentException("Status cannot be Null");
            else
                if (status == 0)
                    throw new ArgumentException("Status cannot be 0");

            if (data == null)
                throw new ArgumentException("Data cannot be null");
            else
                if (data.Length == 0)
                    throw new ArgumentException("No data, use invalid or error response");

            return new Response(status,"application/json", data);
        }

        /// <summary>
        /// Return an text/xml response
        /// </summary>
        /// <param name="status">The HTTP Code to send back e.g. 200 for success</param>
        /// <param name="data">The data to send</param>
        /// <returns>New response object</returns>
        public static Response GetResponseXML(int status, byte[] data)
        {
            if (status.Equals(null))
                throw new ArgumentException("Status cannot be Null");
            else
                if(status == 0)
                    throw new ArgumentException("Status cannot be 0");

            if (data == null)
                throw new ArgumentException("Data cannot be null");
            else
                if (data.Length == 0)
                    throw new ArgumentException("No data, use invalid or error response");

            return new Response(status, "text/xml", data);
        }

        /// <summary>
        /// Return a error response object
        /// </summary>
        /// <returns>New response object</returns>
        public static Response GetErrorResponse()
        {
            return new Response(400, "text/html", new byte[0]);
        }

        /// <summary>
        /// Returns an invalid response object
        /// </summary>
        /// <returns>New response object</returns>
        public static Response GetInvalidRequestResponse()
        {
            return new Response(405, "text/html", new byte[0]);
        }
    }
}
