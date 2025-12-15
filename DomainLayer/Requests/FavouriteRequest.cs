using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Requests
{
    public class FavouriteRequest
    {
        public int? Id { get; set; }
        public int UserId { get; set; }
        public int RecipeId { get; set; }
    }
}
