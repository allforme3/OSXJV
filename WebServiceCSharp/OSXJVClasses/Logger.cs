using System;
using System.IO;

namespace OSXJV.Classes
{
    /// <summary>
    /// A simple class that writes errors to a single file.
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// Singleton instance of Logger
        /// </summary>
        private static Logger inst;
        private string location;

        private Logger(string location)
        {
            this.location = location;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        public static bool Setup(string location)
        {
            if (string.IsNullOrEmpty(location))
                throw new ArgumentException("Location cannot be empty");

            if (!Directory.Exists(string.Format(@"{0}", location)))
                throw new Exception("Location is not a valid logger directory");

            return (inst = new Logger(location)) != null ? true:false;
        }

        /// <summary>
        /// Gets the single instance of Logger
        /// </summary>
        /// <returns>Instance of Logger</returns>
        public static Logger GetInstance()
        {
            if (inst != null)
                return inst;
            else
                throw new Exception("Logger has not been setup");
        }

        /// <summary>
        /// Writes an error the location provided
        /// </summary>
        /// <param name="error">The error message</param>
        public void WriteError(string error)
        {
            try
            {
                if (!string.IsNullOrEmpty(error))
                {
                    string file = string.Format(@"{0}/Error-{1}.txt", location, DateTime.Now.ToString("dd-MM-yy hh-MM-ss"));
                    StreamWriter sw = new StreamWriter(file);
                    sw.WriteLine(error);
                    sw.WriteLine();
                    sw.Close();
                }
            }
            catch (IOException e)
            {
                throw e;
            } 
        }

        public static void Close()
        {
            if (inst == null)
                throw new Exception("Logger Already Closed");
            else
                inst = null;
        }
    }
}
