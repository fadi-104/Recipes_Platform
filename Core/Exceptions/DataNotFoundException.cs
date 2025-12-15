using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions
{
    public class DataNotFoundException : BaseException
    {
        public DataNotFoundException(string message) : base(message)
        {

        }
    }
}
