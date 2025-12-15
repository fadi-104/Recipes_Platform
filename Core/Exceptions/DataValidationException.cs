using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions
{
    public class DataValidationException : BaseException
    {
        public DataValidationException(string message) : base(message)
        {
        }

    }
}
