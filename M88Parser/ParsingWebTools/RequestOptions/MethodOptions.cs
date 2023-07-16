using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsingWebTools.RequestOptions
{
    abstract public class MethodOptions
    {
        public HttpClient? Client { get; set; } = null;

        public bool useProxy { get; set; } = true;

        public Dictionary<string, string>? AdditionalHeaders { get; set; } = null;
    }
}
