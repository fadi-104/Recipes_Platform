using DomainLayer.Requests;
using DomainLayer.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.ImageService
{
    public interface IImageService
    {
        Task CreateAsync(ImageRequest request);
        Task DeleteAsync(int id);
        Task<List<ImageResponse>> GetAllAsync(int recipecId);
        Task<ImageResponse> GetByIdAsync(int id);
    }
}
