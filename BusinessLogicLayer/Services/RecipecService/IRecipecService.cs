using Core.Model;
using DomainLayer.Requests;
using DomainLayer.Responses;


namespace BusinessLogicLayer.Services.RecipecService
{
    public interface IRecipecService
    {
        Task CreateAsync(RecipecRequest request);
        Task DeleteAsync(int id);
        Task<PagedResponse<List<RecipecResponse>>> GetAllAsync(TableOptions options, string? name, bool? isPublished, int? CategoryId, DateTime? date);
        Task<RecipecResponse> GetAsync(int id, string key);
        Task UpdateAsync(RecipecRequest request);
    }
}
