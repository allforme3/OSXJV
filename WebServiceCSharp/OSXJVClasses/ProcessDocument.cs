using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Linq;

namespace OSXJV.Classes
{

    /// <summary>
    /// Class the Processes the document
    /// </summary>
    public class ProcessDocument
    {
        /// <summary>
        /// Object the contains the parsed data ready to be processed.
        /// </summary>
        private XDocument document;

        /// <summary>
        /// The Initial Node.
        /// </summary>
        private Node node = new Node();

        /// <summary>
        /// Used with threading to keep list of processed Nodes.
        /// </summary>
        private List<Tuple<Node, int>> ProcessedElements = new List<Tuple<Node, int>>();

        /// <summary>
        /// Used with threading to keep list of running threads.
        /// </summary>
        private List<Thread> ThreadList = new List<Thread>();

        /// <summary>
        /// Document Type.
        /// </summary>
        private string type;
        private Thread th;

        /// <summary>
        /// Used to by single thread operation to keep track of node id.
        /// </summary>
        int count;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="doc">Parsed document</param>
        /// <param name="type">Type of document</param>
        private ProcessDocument(XDocument doc, string type)
        {
            document = doc;
            this.type = type;
        }

        /// <summary>
        /// Extract Comment
        /// </summary>
        /// <param name="e">Comment object to be parsed</param>
        /// <param name="node">Node to input data</param>
        private void ProcessComment(XComment e, Node node)
        {
            string s = "";
            s = Regex.Replace(e.Value, @"[^\w\s\.@-]", "");
            node.Comments.Add(s);
        }

        /// <summary>
        /// Gets an instance of the ProcessDocument and prepare object.
        /// </summary>
        /// <param name="data">String of the document</param>
        /// <param name="type">Type of document</param>
        /// <returns></returns>
        public static ProcessDocument GetProcess(string data, string type)
        {
            if (string.IsNullOrEmpty(data) || string.IsNullOrEmpty(type))
            {
                throw new ArgumentException();
            }
            try
            {
                XDocument doc = null;
                doc = Prepare(data, type);
                return new ProcessDocument(doc, type);
            }
            catch (System.Xml.XmlException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Get text from the data
        /// </summary>
        /// <param name="e">Text Element</param>
        /// <param name="n">Node to input data</param>
        private void ProcessText(XText e, Node n)
        {
            n.Value = e.Value;
        }

        /// <summary>
        /// Prepares the object with setting the XDocument object to process
        /// </summary>
        /// <param name="data">String of data</param>
        /// <param name="type">Data type</param>
        /// <returns>A XDocument object</returns>
        private static XDocument Prepare(string data, string type)
        {

            if (type.Equals("JSON"))
                return new XDocument(JsonConvert.DeserializeXNode(data, "Root", false).Root.FirstNode);
            else if (type.Equals("XML") || type.Equals("HTML"))
                return XDocument.Parse(data);

            return null;
        }


        /// <summary>
        /// Single Threaded Process.
        /// </summary>
        /// <returns>Object of Nodes</returns>
        public Node Process()
        {
            if (document.Nodes() != null)
            {
                foreach (XNode n in document.Nodes())
                {
                    switch (n.NodeType)
                    {
                        case System.Xml.XmlNodeType.Element:
                            count++;
                            ProcessElement(XElement.Parse(n.ToString()), node);
                            break;
                        case System.Xml.XmlNodeType.Comment:
                            ProcessComment(n as XComment, node);
                            break;
                        case System.Xml.XmlNodeType.Text:
                            ProcessText(n as XText, node);
                            break;
                        case System.Xml.XmlNodeType.Notation:
                            break;
                        case System.Xml.XmlNodeType.EndElement:
                            break;
                        default:
                            break;
                    }
                }
            }
            //SortArray(ref node);
            document = null;
            return node;
        }

        /// <summary>
        /// Single Threaded Process Element Version
        /// </summary>
        /// <param name="e">Element to Process</param>
        /// <param name="node">The Node to fill data with</param>
        /// <returns></returns>
        private Node ProcessElement(XElement e, Node node)
        {
            if (node.Number == 0)
            {
                node.Number = count;
            }
            if (!node.Visited)
            {

                node.Name = e.Name.LocalName;
                foreach (XAttribute ax in e.Attributes())
                {
                    if (ax.Name == "id")
                    {
                        node.Name = node.Name + " #" + ax.Value;
                    }

                    if (type == "HTML")
                    {
                        if (ax.IsNamespaceDeclaration)
                            continue;
                    }
                    Attribute att = new Attribute();
                    att.Name = ax.Name.LocalName;
                    att.Value = ax.Value;
                    node.Attributes.Add(att);
                }
            }

            if (e.Nodes() != null)
            {
                foreach (XNode n in e.Nodes())
                {
                    switch (n.NodeType)
                    {
                        case System.Xml.XmlNodeType.EndElement:
                            break;
                        case System.Xml.XmlNodeType.Element:
                            count++;
                            Node n2 = new Node();
                            node.Children.Add(ProcessElement(XElement.Parse(n.ToString()), n2));
                            break;
                        case System.Xml.XmlNodeType.Comment:
                            ProcessComment(n as XComment, node);
                            break;
                        case System.Xml.XmlNodeType.Text:
                            ProcessText(n as XText, node);
                            break;
                        case System.Xml.XmlNodeType.Notation:
                            break;

                        default:
                            break;
                    }
                }
            }
            node.Visited = true;
            return node;
        }

        /// <summary>
        /// Multi-Threaded Version to process element
        /// </summary>
        /// <param name="e">Element to process</param>
        /// <param name="node">Node to extract data from</param>
        /// <param name="nodeNumber">The Thread internal node number</param>
        /// <returns></returns>
        private Node ProcessElement(XElement e, Node node, ref int nodeNumber)
        {
            if (!node.Visited)
            {
                if (node.Number == 0)
                {
                    node.Number = nodeNumber;
                }
                if (!node.Visited)
                {

                    node.Name = e.Name.LocalName;
                    foreach (XAttribute ax in e.Attributes())
                    {
                        if (ax.Name == "id")
                        {
                            node.Name = node.Name + " #" + ax.Value;
                        }

                        if (type == "HTML")
                        {
                            if (ax.IsNamespaceDeclaration)
                                continue;
                        }
                        Attribute att = new Attribute();
                        att.Name = ax.Name.LocalName;
                        att.Value = ax.Value;
                        node.Attributes.Add(att);
                    }
                }

                if (e.Nodes() != null)
                {
                    List<XNode> list = e.Nodes().ToList();

                    foreach (XNode n in e.Nodes())
                    {
                        switch (n.NodeType)
                        {
                            case System.Xml.XmlNodeType.EndElement:
                                break;
                            case System.Xml.XmlNodeType.Element:
                                nodeNumber++;
                                Node n2 = new Node();
                                node.Children.Add(ProcessElement(XElement.Parse(n.ToString()), n2, ref nodeNumber));
                                break;
                            case System.Xml.XmlNodeType.Comment:
                                ProcessComment(n as XComment, node);
                                break;
                            case System.Xml.XmlNodeType.Text:
                                ProcessText(n as XText, node);
                                break;
                            case System.Xml.XmlNodeType.Notation:
                                break;

                            default:
                                break;
                        }
                    }
                }
                node.Visited = true;
            }
            return node;
        }

        /// <summary>
        /// Processes first element in the document.
        /// </summary>
        /// <param name="e">Element object to process</param>
        /// <param name="node">Node to insert data to</param>
        /// <returns></returns>
        private Node ProcessRoot(XElement e, Node node)
        {            
            node.Number = 1;

            if (!node.Visited)
            {

                node.Name = e.Name.LocalName;
                foreach (XAttribute ax in e.Attributes())
                {
                    if (ax.Name == "id")
                    {
                        node.Name = node.Name + " #" + ax.Value;
                    }

                    if (type == "HTML")
                    {
                        if (ax.IsNamespaceDeclaration)
                            continue;
                    }
                    Attribute att = new Attribute();
                    att.Name = ax.Name.LocalName;
                    att.Value = ax.Value;
                    node.Attributes.Add(att);
                }
            }
            node.Visited = true;
            return node;
        }

        /// <summary>
        /// Method that each thread uses to process the document
        /// </summary>
        /// <param name="doc">A subset of the full document</param>
        /// <param name="start">Start index number</param>
        private void ProcessDocumentParallelInit(XDocument doc,int start)
        {
            int nodeNum = start;

            Node node = new Node();
            if (doc.Root.Nodes() != null)
            {
                List<XNode> list = doc.Root.Nodes().ToList();                
                foreach (XNode n in doc.Root.Nodes())
                {
                    switch (n.NodeType)
                    {
                        case System.Xml.XmlNodeType.Element:
                            nodeNum++;
                            Node n2 = new Node();
                            node.Children.Add(ProcessElement(XElement.Parse(n.ToString()), n2, ref nodeNum));
                            break;
                        case System.Xml.XmlNodeType.Comment:
                            ProcessComment(n as XComment, node);
                            break;
                        case System.Xml.XmlNodeType.Text:
                            ProcessText(n as XText, node);
                            break;
                        case System.Xml.XmlNodeType.Notation:
                            break;
                        case System.Xml.XmlNodeType.EndElement:
                            break;
                        default:
                            break;
                    }
                }
            }
            document = null;
            ProcessedElements.Add(new Tuple<Node, int>(node, start));
        }

        /// <summary>
        /// Parse Document Using Multiple Threads
        /// </summary>
        /// <param name="pCount">Number of Threads to run Default = 4</param>
        /// <returns>A object of Node that has been processed</returns>
        public Node ProcessParallel(int pCount = 4)
        {
            node = ProcessRoot(document.Root, node);

            int nodeCount = document.Root.Nodes().Count();

            if(nodeCount <= pCount)
            {
                return Process();
            }
            else if (nodeCount > pCount)
            {

                List<XNode> List = document.Root.Nodes().ToList();
                int spread = 0;

                spread = (int)Math.Ceiling((double)nodeCount / (double)pCount);

                int totalNodes = 1;

                for (int i = 0; i < pCount; i++)
                {
                    int neg = 0;
                    int start = totalNodes;
                    if ((spread * (i+1)) > nodeCount)
                    {
                        neg = nodeCount - (spread * (i + 1)); 
                    }

                    List<XNode> list = List.GetRange((spread * i), spread + neg);
                    XElement root = new XElement("Root", list);                  
                    XDocument doc = new XDocument(root);

                    (th = new Thread(() => ProcessDocumentParallelInit(doc, start))).Start();

                    ThreadList.Add(th); //Add to Threads list to keep recored of threads running
                    totalNodes += root.Descendants().Count(); //Increment start position.
                }
                document = null;
                foreach (Thread t in ThreadList)
                {                    
                    t.Join(); //Wait for threads to join
                }

                ProcessedElements.Sort((x, y) => x.Item2.CompareTo(y.Item2)); //Sort List by start index so they are in order.

                foreach(Tuple<Node,int> tup in ProcessedElements)
                {
                    foreach(Node n in tup.Item1.Children)
                    {
                        node.Children.Add(n);
                    }
                }
            }
            return node;
        }
    }
}
