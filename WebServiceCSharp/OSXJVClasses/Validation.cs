﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Xml;

namespace OSXJV.Classes
{
    /// <summary>
    /// Preform validation
    /// </summary>
    public class Validation
    {
        /// <summary>
        /// Constructor
        /// </summary>
        private Validation()
        {

        }

        /// <summary>
        /// Checks the document and if it is valid
        /// </summary>
        /// <param name="data">Document contents</param>
        /// <param name="type">Type of document</param>
        /// <returns>True if valid, else false</returns>
        /// <exception cref="ArgumentException">Invalid data type or data and type cannot be null</exception>
        /// <exception cref="XmlException">Invalid XML or HTML</exception>
        /// <exception cref="JsonReaderException">Invalid JSON</exception>
        public static bool CheckDocument(string data, string type)
        {
            if(string.IsNullOrEmpty(data) || string.IsNullOrEmpty(type))
            {
                throw new ArgumentException("Data or Type cannot be Null");
            }

            if (type.Equals("XML") || type.Equals("HTML"))
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.DtdProcessing = DtdProcessing.Parse;
                settings.MaxCharactersFromEntities = 2048;
                using (XmlReader xr = XmlReader.Create(new StringReader(data),settings))
                {
                    try
                    {
                        while (xr.Read()) { }
                        return true;
                    }
                    catch (XmlException ex)
                    {
                        throw ex;
                    }
                }
            }
            else if(type.Equals("JSON"))
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
