using DomainLayer.Requests;
using DomainLayer.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.RatingService
{
    public interface IRatingService
    {
        Task CreateAsync(RatingRequest request);
        Task<RateResponse> GetAsync(int recipecId, int userId);
        Task UpdateAsync(RatingRequest request);
    }
}
