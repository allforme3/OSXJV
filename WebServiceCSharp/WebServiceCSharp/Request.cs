using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web
{
    public class Request
    {
        public string Value { get; set; }

        public Request(string data)
        {
            Value = data;
        }
        public static Request GetRequest(string request)
        {            
            return new Request(request);
        }
    }
}
