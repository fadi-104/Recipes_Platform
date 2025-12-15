using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class MessageOption
    {
        public int Skip { get; set; }
        public int PageSize { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
    }
}
