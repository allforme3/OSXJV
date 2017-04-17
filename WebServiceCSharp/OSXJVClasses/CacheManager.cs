using System;
using System.IO;

namespace OSXJV.Classes
{
    /// <summary>
    /// Manages Saving an Retrieving Filesexi
    /// </summary>
    public class CacheManager
    {
        private static CacheManager inst;
        private string path;

        private CacheManager(string path)
        {
            this.path = path;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public static bool Setup(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("Path cannot be empty");

            if (!Directory.Exists(string.Format(@"{0}",path)))
                throw new Exception("Path is not a valid cache directory");

            return (inst = new CacheManager(path)) != null ? true : false;
        }

        /// <summary>
        /// Get the single instance of the class
        /// </summary>
        /// <returns>An instance of CacheManager</returns>
        public static CacheManager GetInstance()
        {
            if (inst != null)
                return inst;
            else
                throw new Exception("CacheManger has not been setup");
        }

        /// <summary>
        /// Retrieve the file from caching
        /// </summary>
        /// <param name="ID">Unique ID of the file</param>
        /// <returns></returns>
        public string getFile(string ID)
        {
            if (string.IsNullOrEmpty(ID))
                throw new ArgumentException("ID cannot be null or empty");

            string filePath = path + "/" + ID.Replace("/","") + ".json";
            string output = "";

            using (StreamReader sr = new StreamReader(filePath))
            {
                output = sr.ReadToEnd();
            }

            if (!string.IsNullOrEmpty(output))
                return output;
            else
                throw new Exception("Error Reading From File");
        }

        /// <summary>
        /// Save the file to the local system for caching
        /// </summary>
        /// <param name="ID">Unique ID of the file</param>
        /// <param name="nodes">The document to be saved</param>
        public bool saveFile(string ID, string nodes)
        {
            if (string.IsNullOrEmpty(ID))
                throw new ArgumentException("ID cannot be null or empty");

            if (string.IsNullOrEmpty(nodes))
                throw new ArgumentException("Document cannot be null or empty");

            string filePath = path + "/" + ID + ".json";
            try
            {
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.WriteLine(nodes);
                }
            }
            catch
            {
                throw new Exception("Failed to save file");
            }

            return true;
        }

        public static void Close()
        {
            if (inst == null)
                throw new Exception("CacheManager Already Closed");
            else
                inst = null;
        }
    }
}
