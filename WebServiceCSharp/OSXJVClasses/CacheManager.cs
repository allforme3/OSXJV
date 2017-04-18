using System;
using System.IO;

namespace OSXJV.Classes
{
    /// <summary>
    /// Manages Saving an Retrieving Processed Documents
    /// </summary>
    public class CacheManager
    {
        /// <summary>
        /// Instance of CacheManager
        /// </summary>
        private static CacheManager Inst;

        /// <summary>
        /// Cache folder location
        /// </summary>
        private static string path = null;

        /// <summary>
        /// Constructor setting store location;
        /// </summary>
        /// <param name="path"></param>
        private CacheManager(string cachePath)
        {
            path = cachePath;
        }

        /// <summary>
        /// Initialises CacheManager with input path
        /// </summary>
        /// <param name="path"></param>
        public static bool Setup(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("Path cannot be empty");

            if (!Directory.Exists(string.Format(@"{0}",path)))
                throw new Exception("Path is not a valid cache directory");

            return (Inst = new CacheManager(path)) != null ? true : false;
        }

        /// <summary>
        /// Get the single instance of the class
        /// </summary>
        /// <returns>An instance of CacheManager</returns>
        /// <exception cref="Exception">If the CacheManger has not been setup</exception>
        public static CacheManager GetInstance()
        {
            if (Inst != null)
                return Inst;
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
        /// <summary>
        /// Destroys the created instance
        /// </summary>
        /// <exception cref="Exception">If Instance isn't previously created</exception>
        public static void Close()
        {
            if (Inst == null)
                throw new Exception("CacheManager Already Closed");
            else
            {
                path = null; //Clear static path
                Inst = null; //clear static instance
            }
        }

        /// <summary>
        /// Manages files in the cache deleting old ones than 6 hours when called
        /// </summary>
        public static void ManageCache()
        {
            if (path != null)
            {
                string[] files = Directory.GetFiles(path);
                foreach (string file in files)
                {
                    if (File.GetLastAccessTime(file) < DateTime.Now.AddHours(-6.0))
                        File.Delete(file);
                }
            }
            else
                throw new Exception("CacheManger not setup");
        }
    }
}
