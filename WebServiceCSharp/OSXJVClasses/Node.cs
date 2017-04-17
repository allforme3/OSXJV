using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace OSXJV.Classes
{
    /// <summary>
    /// Contain Processed Document Information
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
        /// Constructor
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
        /// The Number of the Node
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
        /// The Name of Node
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
        /// The Value of the Node
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
        /// Comments That the Node Has.
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
        /// Attributes the Node has.
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
        /// Children Nodes the Node is linked to.
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
        /// If the node has been visited previous by the ProcessDocument, prevent multiple same Nodes.
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
