using DomainLayer.Requests;
using DomainLayer.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.FavouriteService
{
    public interface IFavouriteService
    {
        Task AddFavouriteAsync(FavouriteRequest request);
        Task<List<RecipecResponse>> GetFavouriteAsync(int userId);
        Task RemoveFavouriteAsync(int id);
    }
}
