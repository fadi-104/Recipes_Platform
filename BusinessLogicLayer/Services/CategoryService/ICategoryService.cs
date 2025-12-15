using DomainLayer.Requests;
using DomainLayer.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.CategoryService
{
    public interface ICategoryService
    {
        Task CreateAsync(CategoryRequest request);
        Task DeleteAsync(int id);
        Task<List<CategoryResponse>> GetAllAsync();
        Task<CategoryResponse> GetAsync(int id);
        Task UpdateAsync(CategoryRequest request);
    }
}
