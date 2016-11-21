using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Net;
using Web;

namespace WebServiceCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Httpserver s = new Httpserver();
            s.Start();
        }
    }
}
