using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer
{
    /// <summary>
    /// 
    /// </summary>
    public class Output
    {
        private Node nodes;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodes"></param>
        public Output(Node nodes)
        {
            if (nodes == null)
                throw new ArgumentException();
            this.nodes = nodes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string CreateGrid()
        {
            JObject obj = new JObject();
            obj.Add("text", nodes.Name);
            obj.Add("id", nodes.Number);
            obj.Add("state", new JObject(new JProperty("selected", true)));
            if(nodes.Children.Count > 0)
            {
                JArray array = new JArray();
                foreach (Node n2 in nodes.Children)
                {
                    array.Add(GridGetChidren(n2));
                }
                obj.Add("children", array);
            }
            return obj.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n">Child Node</param>
        /// <returns></returns>
        private JObject GridGetChidren(Node n)
        {
            JObject child = new JObject();
            child.Add("id", n.Number);
            child.Add("text", n.Name);
            if (n.Children.Count > 0)
            {
                JArray array = new JArray();
                foreach(Node n2 in n.Children)
                {
                    array.Add(GridGetChidren(n2));
                }
                child.Add("children", array);
            }
            return child;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public string CreateView(int node)
        {

            string output = "<div class='text-center ui-layout-center ui-layout-pane ui-layout-pane-center'><div style ='display:inline-block' class='ui-selectable ui-droppable'>";
            if (nodes.Number.Equals(node))
            {
                int count = 0;
                output += CreateNodeView(nodes, "node");
                foreach (Node n in nodes.Children)
                {
                    count++;
                    output += CreateNodeView(n, "node-child");
                    if (count == 100)
                        break;
                }
            }
            else
            {
                string temp = "";
                foreach (Node n2 in nodes.Children)
                {
                    temp += CheckChildren(n2, node);
                }
                if (!string.IsNullOrEmpty(temp))
                    output += temp;
            }
            output += "</div></div>";
            return output;
        }
        private string CheckChildren(Node n, int number)
        {
            string output = "";
            if (CheckNodeNumber(n, number))
            {
                int count = 0;
                output += CreateNodeView(n, "node");
                foreach (Node n2 in n.Children)
                {
                    count++;
                    output += CreateNodeView(n2, "node-child");
                    if (count == 100)
                        break;
                }
            }
            else if (n.Children.Count > 0)
            {
                foreach (Node n2 in n.Children)
                {
                    output += CheckChildren(n2, number);
                }
            }

            return output;
        }

        private string CreateNodeView(Node n, string type)
        {
            string node = "";
            node += "<div id='" + n.Number + "'class='" + type + " type ui-draggable ui-selectee' style='left:5 %; top:30 %;'>";
            node += "<div class='head'><span>" + n.Name + "</span></div>";
            if (!string.IsNullOrEmpty(n.Value))
            {
                node += string.Format("<div class='blockR'><p>Value</p></div><div class=comment><span>{0}</span></div>", n.Value);
            }
            if (n.Attributes.Count > 0)
            {
                node += "<div class='attribute'><div class='aHeader'><p><button><i class='fa fa-plus'></i></button>Attributes</p></div><div class='options'>";
                foreach (Attribute a in n.Attributes)
                {
                    node += string.Format("<div class='blockR'><p>{0}</p></div><div class='comment'><p>{1}</p></div>", a.Name, a.Value);
                }
                node += "</div>";
            }
            node += "</div></div>";

            return node;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        private bool CheckNodeNumber(Node n, int number)
        {
            return n.Number.Equals(number);
        }
    }
}
