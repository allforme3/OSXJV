using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WebServer
{
    /// <summary>
    /// 
    /// </summary>
    public class Process
    {

        private XDocument document;
        private Node node = new Node();
        private int location = 0;
        private int count = 0;
        private string type;
        
        /// <summary>
        /// Initalisation of the object that prepares before processing
        /// </summary>
        /// <param name="doc">The document</param>
        /// <param name="type">The document file type</param>
        private Process(XDocument doc,string type)
        {
            document = doc;
            this.type = type;
        }
        
        /// <summary>
        /// Creates and returns a new process object
        /// </summary>
        /// <param name="data">The document in string form</param>
        /// <param name="type">The type of document</param>
        /// <returns>New Process object</returns>
        public static Process GetProcess(string data,string type)
        {
            if(string.IsNullOrEmpty(data) || string.IsNullOrEmpty(type))
            {
                throw new ArgumentException();
            }           
            try
            {
                XDocument doc = null;
                doc = Prepare(data, type);
                return new Process(doc, type);
            }
            catch(System.Xml.XmlException e)
            {
                throw e;
            }            
        }

        /// <summary>
        /// Processes the document sent via server
        /// </summary>
        /// <returns>Returns a root node that has been calculated from the document</returns>
        public Node ProcessDocument()
        {            
            if (document.Nodes() != null)
            {
                foreach (XNode n in document.Nodes())
                {
                    switch (n.NodeType)
                    {
                        case System.Xml.XmlNodeType.Element:
                            count++;
                            location++;
                            ProcessElement(XElement.Parse(n.ToString()), node);
                            location--;
                            break;
                        case System.Xml.XmlNodeType.Comment:
                            ProcessComment(n as XComment, node);
                            break;
                        case System.Xml.XmlNodeType.Text:
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
            Console.WriteLine(location);
            //SortArray(ref node);
            document = null;
            return node;
        }

        /// <summary>
        /// Processes an Element within the document
        /// </summary>
        /// <param name="e">The element</param>
        /// <param name="node">The parent node</param>
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
                    if (!ax.IsNamespaceDeclaration)
                    {
                        Attribute att = new Attribute();
                        att.Name = ax.Name.LocalName;
                        att.Value = ax.Value;
                        node.Attributes.Add(att);
                    }
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
                            location++;
                            Node n2 = new Node();
                            node.Children.Add(ProcessElement(XElement.Parse(n.ToString()), n2));

                            location--;
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
            Console.WriteLine(location);
            node.Visited = true;
            return node;
        }

        /// <summary>
        /// Process a comment that was in the document
        /// </summary>
        /// <param name="e">The comment element</param>
        /// <param name="node">The parent node</param>
        private void ProcessComment(XComment e, Node node)
        {
            string s = "";
            s = Regex.Replace(e.Value, @"[^\w\.@-]", "");
            node.Comments.Add(s);
        }

        /// <summary>
        /// Processes text that was in the document
        /// </summary>
        /// <param name="e">A Text element</param>
        /// <param name="n">The parent node</param>
        private void ProcessText(XText e, Node n)
        {
            n.Value = e.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static XDocument Prepare(string data,string type)
        {
            if (type.Equals("application/json"))
                return JsonConvert.DeserializeXNode(data, "Root", false);
            else if(type.Equals("text/xml"))
                return XDocument.Parse(data);

            return null;
        }
    }
}
