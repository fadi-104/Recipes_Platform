using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Responses
{
    public class RateResponse
    {
        public int Id { get; set; }
        public int RecipecId { get; set; }
        public int UserId { get; set; }
        public float Rate { get; set; }
    }
}
