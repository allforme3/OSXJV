using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading;

namespace OSXJV.Classes
{
    /// <summary>
    /// Creates the Output for the web page to display.
    /// </summary>
    public class Output
    {
        /// <summary>
        /// 
        /// </summary>
        private int left = 100, top = 130;

        /// <summary>
        /// 
        /// </summary>
        private Node nodes;

        /// <summary>
        /// 
        /// </summary>
        private bool GotParent = false;

        /// <summary>
        /// Parent of node when building output (Used when getting Node other than root).
        /// </summary>
        private int Parent = 0;

        /// <summary>
        /// Used in Threading, list of calculated HTML strings.
        /// </summary>
        private List<Tuple<int, string>> cNodes = new List<Tuple<int, string>>();

        /// <summary>
        /// Creation of a Output object.
        /// </summary>
        /// <param name="nodes">A processed object of Nodes</param>
        public Output(Node nodes)
        {
            if (nodes == null)
                throw new ArgumentException();
            this.nodes = nodes;
        }

        /// <summary>
        /// Creates the grid data.
        /// </summary>
        /// <returns>A JSON object</returns>
        public JObject CreateGrid()
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
            return obj;
        }

        /// <summary>
        /// Recursive function to get all the nodes data for the grid .
        /// </summary>
        /// <param name="n">Child Node</param>
        /// <returns>JSON object</returns>
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
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// CreateView using a Single Thread
        /// </summary>
        /// <param name="node">Index of node to start from</param>
        /// <param name="nodeStart">Where to start the child nodes from</param>
        /// <returns>String of calculated HTML</returns>
        public string CreateViewSingle(int node, int nodeStart = 0)
        {
            string output = "<div class='text-center ui-layout-center ui-layout-pane ui-layout-pane-center'><div style ='display:inline-block' class='ui-selectable ui-droppable'>";

            if (nodes.Number.Equals(node))
            {
                int count = 0;
                output += CreateNodeView(nodes, "node");  


                foreach (Node n in nodes.Children)
                {
                    if(nodeStart > 0)
                    {
                        if (count != nodeStart)
                            continue;
                    }
                    count++;
                    output += CreateNodeView(n, "node-child"); //Child(Nodes) Thread

                    if ((count-nodeStart) == 200)
                    {
                        output += CreateExtraNode("node-child",count);
                        break;
                    }
                }
            }

            else
            {
                GetParent(nodes, node);
                string temp = "";
                if (GotParent)
                {
                    if (nodes.Number == Parent)
                    {
                        output += CreateNodeView(nodes, "node-parent");
                    }
                }
                foreach (Node n2 in nodes.Children)
                {
                    if (GotParent)
                    {
                        if (n2.Number == Parent)
                        {
                            output += CreateNodeView(n2, "node-parent");
                        }
                    }
                    temp += CheckChildren(n2, node);
                }
                if (!string.IsNullOrEmpty(temp))
                    output += temp;
            }
            output += "</div></div>"; //Close out divs
            return output;
        }

        /// <summary>
        /// Builds a get more button to display
        /// </summary>
        /// <param name="type">Node type e.g. 'node-child'</param>
        /// <param name="id">The id of the node to start from</param>
        /// <returns>String of calculated HTML</returns>
        private string CreateExtraNode(string type,int id)
        {
            string node = "";

            if (type == "node")
            {
                if (GotParent)
                {
                    left = left + 400;
                }
            }
            if (type == "node-child")
            {
                left = left + 400;
            }

            node += "<div class='node-child type ui-draggable ui-selectee' style='left:" + left + "px; top:" + top + "px;margin-bottom:50px;'>";
            node += "<div class='head'><span><button class='nameBtn' onclick='GetMoreNodes(" + id + ")'>Show Lower</button></span></div>";
            node += "</div></div>";
            return node;
        }

        /// <summary>
        /// Create a previous node button
        /// </summary>
        /// <param name="type">Node type e.g. 'node-child'</param>
        /// <param name="leftVal">Margin from the left of the display</param>
        /// <param name="topVal">Margin from the top of the display</param>
        /// <param name="id">The id of the node to start from</param>
        /// <returns>String of calculated HTML</returns>
        private string CreatePreviousNode(string type, int leftVal, int topVal, int id)
        {
            string node = "";

            if (type == "node")
            {
                if (GotParent)
                {
                    leftVal = leftVal + 400;
                }
            }
            if (type == "node-child")
            {
                leftVal = leftVal + 400;
            }
            node += "<div class='node-child type ui-draggable ui-selectee' style='left:" + leftVal + "px; top:" + topVal + "px;'>";
            node += "<div class='head'><span><button class='nameBtn' onclick='GetMoreNodes(" + id + ")'>Show Higher</button></span></div>";
            node += "</div></div>";
            return node;
        }

        /// <summary>
        /// Create a extra node button
        /// </summary>
        /// <param name="type">Node type e.g. 'node-child'</param>
        /// <param name="leftVal">Margin from the left of the display</param>
        /// <param name="topVal">Margin from the top of the display</param>
        /// <param name="id">The id of the node to start from</param>
        /// <returns>String of calculated HTML</returns>
        private string CreateExtraNode(string type, int leftVal, int topVal,int id)
        {
            string node = "";

            if (type == "node")
            {
                if (GotParent)
                {
                    leftVal = leftVal + 400;
                }
            }
            if (type == "node-child")
            {
                leftVal = leftVal + 400;
            }
            node += "<div class='node-child type ui-draggable ui-selectee' style='left:" + leftVal + "px; top:" + topVal + "px;margin-bottom:50px;'>";
            node += "<div class='head'><span><button class='nameBtn' onclick='GetMoreNodes(" + id + ")'>Show Lower</button></span></div>";            
            node += "</div></div>";
            return node;
        }

        /// <summary>
        /// Generate Output HTML when using multi-threads
        /// </summary>
        /// <param name="job">The Nodes to process</param>
        /// <param name="start">Start index</param>
        /// <param name="showHigher">if the are nodes higher up, show previous button</param>
        /// <param name="next">Next value for next button</param>
        /// <param name="previous">Previous value for previous button</param>
        private void CreateNodeChildViewsParallel(List<Node> job,int start, bool showHigher,int next,int previous)
        {
            int threadID = int.Parse(Thread.CurrentThread.Name);
            string type = "node-child";
            string output = "";
            
            if(start == 0 && showHigher)
            {
                output += CreatePreviousNode(type, left, top, previous);
            }
            bool hadCommentsPrev = false;
            int numCommentsPrevious = 0;

            foreach(Node n in job)
            {                    
                int extra = showHigher ? 130 * (start +1) : 130 * start;

                if (hadCommentsPrev)
                    extra += (numCommentsPrevious * 25);

                if (n.Comments.Count > 0)
                {
                    hadCommentsPrev = true;
                    numCommentsPrevious = n.Comments.Count;
                }
                else
                    hadCommentsPrev = false;

                output += CreateNodeView(n, type,left,top + extra);
                start++;
                if (start == 200)
                {
                    output += CreateExtraNode(type, left, top + extra + 130,next);
                    break;
                }
                
            }

            cNodes.Add(new Tuple<int, string>(threadID, output));
        }

        /// <summary>
        /// Creates the view of nodes using multiple threads.
        /// </summary>
        /// <param name="node">Number of node to start from. Default is 1(Root)</param>
        /// <param name="pCount">Number of Threads to use. Default is 4</param>
        /// <param name="nodeStart">Where to start the child nodes from</param>
        /// <returns>String of calculated HTML</returns>
        public string CreateView(int node = 1,int pCount = 4,int nodeStart = 0) //Setting Defaults
        {

            List<Thread> threadList = new List<Thread>();

            string output = "<div class='text-center ui-layout-center ui-layout-pane ui-layout-pane-center'><div style ='display:inline-block' class='ui-selectable ui-droppable'>";
            if (nodes.Number.Equals(node))
            {
                int childCount = 0;

                if (nodes.Children.Count < 200)
                    childCount = nodes.Children.Count;
                else
                {
                    childCount = 200;
                }

                if(childCount < pCount * 2)
                {
                    output += CreateNodeView(nodes, "node", left, top);
                    foreach(Node n2 in nodes.Children)
                    {
                        output += CreateNodeView(n2, "node-child");
                    }
                }
                else
                {                                 
                    int spread = (int)Math.Ceiling((double)childCount / (double)pCount);

                    output += CreateNodeView(nodes, "node",left,top);  //Parent(Node) Thread  
              
                    for (int i = 0; i < pCount; i++)
                    {
                        int neg = 0;
                        if ((spread * (i + 1)) > childCount)
                        {
                            neg = childCount - (spread * (i + 1));
                        }
                        int start = (spread * i) ;
                        int rangeStart = (spread * i) + nodeStart;
                        bool showHigher = nodeStart != 0 ? true : false;

                        List<Node> NodesToProcess = nodes.Children.GetRange(rangeStart, spread + neg);
                        Thread threadJob = new Thread(() => CreateNodeChildViewsParallel(NodesToProcess, start, showHigher, childCount + nodeStart, nodeStart - childCount));
                        threadJob.Name = i.ToString();
                        threadJob.Start();
                        threadList.Add(threadJob);
                    }              
                    foreach(Thread t in threadList)
                    {
                        t.Join();
                    }

                    cNodes.Sort((x, y) => x.Item1.CompareTo(y.Item1));

                    foreach(Tuple<int,string> tup in cNodes)
                    {
                        output += tup.Item2;
                    }
                }
            }
            else
            {
                GetParent(nodes, node);
                string temp = "";
                if (GotParent)
                {
                    if (nodes.Number == Parent)
                    {
                        output += CreateNodeView(nodes, "node-parent");
                    }
                }
                bool found =false;
                foreach (Node n2 in nodes.Children)
                {
                    if (GotParent)
                    {
                        if (n2.Number == Parent)
                        {
                            output += CreateNodeView(n2, "node-parent");
                        }
                    }
                    temp += CheckChildren(n2, node,pCount,nodeStart,ref found);
                    if (found)
                        break;
                }
                if (!string.IsNullOrEmpty(temp))
                    output += temp;
            }
            output += "</div></div>";
            return output;
        }

        /// <summary>
        /// Check child nodes if the are to be part of the output.
        /// </summary>
        /// <param name="n">Node to search</param>
        /// <param name="number">Number to check</param>
        /// <returns>String of calculated HTML</returns>
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
                    
                }
            }
            else if (n.Children.Count > 0)
            {
                foreach (Node n2 in n.Children)
                {
                    if (GotParent)
                    {
                        if (n2.Number == Parent)
                        {
                            output += CreateNodeView(n2, "node-parent");
                        }
                    }
                    output += CheckChildren(n2, number);
                }
            }

            return output;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <param name="number"></param>
        /// <param name="pCount"></param>
        /// <param name="nodeStart"></param>
        /// <param name="found"></param>
        /// <returns>String of calculated HTML</returns>
        private string CheckChildren(Node n, int number,int pCount, int nodeStart, ref bool found)
        {
            string output = "";
            if (CheckNodeNumber(n, number))
            {
                found = true;
                List<Thread> threadList = new List<Thread>();

                int count = 0;
                output += CreateNodeView(n, "node");
                count++;
                //output += CreateNodeView(n2, "node-child");
                int childCount = 0;

                if (n.Children.Count < 200)
                    childCount = n.Children.Count;
                else
                {
                    childCount = 200;
                }
                if (childCount < pCount * 2)
                {
                    foreach(Node n2 in n.Children)
                    {
                        output += CreateNodeView(n2, "node-child");
                    }
                }
                else
                {
                    int spread = (int)Math.Ceiling((double)childCount / (double)pCount);

                    if (childCount > 0)
                    {
                        for (int i = 0; i < pCount; i++)
                        {
                            int neg = 0;
                            if ((spread * (i + 1)) > childCount)
                            {
                                neg = childCount - (spread * (i + 1));
                            }
                            int start = (spread * i);
                            int rangeStart = (spread * i) + nodeStart;
                            bool showHigher = nodeStart != 0 ? true : false;

                            List<Node> NodesToProcess = n.Children.GetRange(rangeStart, spread + neg);

                            if (NodesToProcess.Count > 0)
                            {
                                Thread threadJob = new Thread(() => CreateNodeChildViewsParallel(NodesToProcess, start, showHigher, childCount + nodeStart, nodeStart - childCount));
                                threadJob.Name = i.ToString();
                                threadJob.Start();
                                threadList.Add(threadJob);
                            }
                        }
                        foreach (Thread t in threadList)
                        {
                            t.Join();
                        }
                        cNodes.Sort((x, y) => x.Item1.CompareTo(y.Item1));

                        foreach (Tuple<int, string> tup in cNodes)
                        {
                            output += tup.Item2;
                        }
                    }
                }
            }
            else if (n.Children.Count > 0)
            {
                foreach (Node n2 in n.Children)
                {
                    if (GotParent)
                    {
                        if (n2.Number == Parent)
                        {
                            output += CreateNodeView(n2, "node-parent");
                        }
                    }
                    output += CheckChildren(n2, number,pCount,nodeStart,ref found);
                }
            }

            return output;
        }

        /// <summary>
        /// Finds the parent node.
        /// </summary>
        /// <param name="node">Node to search</param>
        /// <param name="number">Node number to find</param>
        private void GetParent(Node node,int number)
        {
            if(!CheckNodeNumber(node,number))
            {
                foreach(Node n in node.Children)
                {
                    if(CheckNodeNumber(n, number))
                    {                        
                        Parent = node.Number;
                        GotParent = true;
                    }
                    else
                    {                       
                        GetParent(n, number);
                    }
                }
            }
        }

        /// <summary>
        /// Generates HTML for the Specific Node (Multi-Threaded Version)
        /// </summary>
        /// <param name="n">Node to parse</param>
        /// <param name="type">Type of node</param>
        /// <param name="leftVal">Margin left of display</param>
        /// <param name="topVal">Margin top of display</param>
        /// <returns>String of calculated HTML</returns>
        private string CreateNodeView(Node n, string type,int leftVal,int topVal)
        {
            string node = "";

            if(type == "node")
            {
                if(GotParent)
                {
                    leftVal = leftVal + 400;
                }
            }               
            if(type == "node-child")
            {
                leftVal = leftVal + 400;
            }
            node += "<div id='" + n.Number + "'class='" + type + " type ui-draggable ui-selectee' style='left:" + leftVal + "px; top:" + topVal + "px;'>";
            node += "<div class='head'><span><button class='nameBtn' onclick='GetNode("+n.Number+")'>" + n.Name + "</button></span></div>";
            if (!string.IsNullOrEmpty(n.Value))
            {
                node += string.Format("<div class='blockR'><p>Value</p></div><div class=comment><span>{0}</span></div>", n.Value);
            }
            if(n.Comments.Count >0)
            {
                node += "<div><p class='text-center'>Comments</p></div>";

                foreach(string com in n.Comments)
                {
                    node += "<div class='comment'>" + com + "</div>";
                }
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
        /// Generates HTML for specific Node (Single Threaded Version)
        /// </summary>
        /// <param name="n">Node to parse</param>
        /// <param name="type">Type of node</param>
        /// <returns>String of calculated HTML</returns>
        private string CreateNodeView(Node n, string type)
        {
            string node = "";
            int leftVal = left;
            if (type == "node")
            {
                if (GotParent)
                {
                    left = left + 400;
                    leftVal = left;
                }
            }
            if (type == "node-child")
            {
                leftVal = leftVal + 400;
            }
            node += "<div id='" + n.Number + "'class='" + type + " type ui-draggable ui-selectee' style='left:" + leftVal + "px; top:" + top + "px;'>";
            node += "<div class='head'><span><button class='nameBtn' onclick='GetNode(" + n.Number + ")'>" + n.Name + "</button></span></div>";
            if (!string.IsNullOrEmpty(n.Value))
            {
                node += string.Format("<div class='blockR'><p>Value</p></div><div class=comment><span>{0}</span></div>", n.Value);
            }
            if (n.Comments.Count > 0)
            {
                node += "<div><p class='text-center'>Comments</p></div>";

                foreach (string com in n.Comments)
                {
                    node += "<div class='comment'>" + com + "</div>";
                }
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

            if (type == "node-child")
            {
                top = top + 130;
            }
            return node;
        }

        /// <summary>
        /// Checks if Node number and inputted number match.
        /// </summary>
        /// <param name="n">Node to search</param>
        /// <param name="number">Number to match with</param>
        /// <returns></returns>
        private bool CheckNodeNumber(Node n, int number)
        {
            return n.Number.Equals(number);
        }
    }
}
