using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace WebServer
{
    /// <summary>
    /// 
    /// </summary>
    public class Node
    {
        private string name;
        private List<Attribute> attributes;
        private string value;
        private List<Node> children;
        private int number;
        private bool visited;
        private List<string> comments;
        
        /// <summary>
        /// 
        /// </summary>
        public Node()
        {
            Attributes = new List<Attribute>();
            Children = new List<Node>();
            Comments = new List<string>();
            number = 0;
            visited = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Number
        {
            get
            {
                return number;
            }

            set
            {
                number = value;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(NullValueHandling =NullValueHandling.Ignore)]        
        public string Value
        {
            get
            {
                return value;
            }

            set
            {
                this.value = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<string> Comments
        {
            get
            {
                return comments;
            }

            set
            {
                comments = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty()]
        public List<Attribute> Attributes
        {
            get
            {
                return attributes;
            }

            set
            {
                attributes = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty()]
        public List<Node> Children
        {
            get
            {
                return children;
            }

            set
            {
                children = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public bool Visited
        {
            get
            {
                return visited;
            }

            set
            {
                visited = value;
            }
        }      
    }
}
