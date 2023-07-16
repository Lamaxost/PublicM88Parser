using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsingWebTools.RequestOptions
{
    public class PostOptions : MethodOptions
    {
        public HttpContent? Content { get; set; }
    }
}
