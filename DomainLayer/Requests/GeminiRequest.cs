using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Requests
{
    public class GeminiRequest
    {
        public Content[] Content { get; set; }
    }

    public class Content
    {
        public string Role { get; set; } = "user";
        public Part[] Parts { get; set; }
    }

    public class Part
    {
        public string Text { get; set; }
    }
}
