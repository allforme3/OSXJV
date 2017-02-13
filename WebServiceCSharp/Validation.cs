using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace WebServer
{
    /// <summary>
    /// 
    /// </summary>
    public class Validation
    {
        private Validation()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool CheckDocument(string data, string type)
        {
            if(string.IsNullOrEmpty(data) || string.IsNullOrEmpty(type))
            {
                throw new ArgumentException("Data or Type cannot be Null");
            }

            if (type.Equals("text/xml"))
            {
                using (XmlReader xr = XmlReader.Create(new StringReader(data)))
                {
                    try
                    {
                        while (xr.Read()) { }
                        return true;
                    }
                    catch (XmlException ex)
                    {
                        throw new XmlException(ex.Message);
                    }
                }
            }
            else if(type.Equals("application/json") || type.Equals("application/oclet-stream"))
            {
                try
                {
                    JToken.Parse(data);
                    return true;
                }
                catch (JsonReaderException ex)
                {
                    throw new JsonReaderException(ex.Message);
                }
            }

            throw new ArgumentException("Invalid data or type");
        }
    }
}
