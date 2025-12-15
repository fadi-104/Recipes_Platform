using AutoMapper;
using Core.Exceptions;
using DataAccessLayer.Repository.CategoryRepository;
using DomainLayer.Entites;
using DomainLayer.Requests;
using DomainLayer.Responses;
using Microsoft.Extensions.Caching.Memory;


namespace BusinessLogicLayer.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, IMemoryCache cache)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<List<CategoryResponse>> GetAllAsync()
        {
            var cacheKey = "categories_all";

            return await _cache.GetOrCreateAsync(cacheKey, async entry => 
            {

                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                entry.SlidingExpiration = TimeSpan.FromMinutes(2);
                entry.SetPriority(CacheItemPriority.Normal);
                entry.Size = 1;

                var list = await _categoryRepository.GetAllNoTrackingAsync();
                return list.Select(x => _mapper.Map<CategoryResponse>(x)).ToList();

            });
        }

        public async Task<CategoryResponse> GetAsync(int id)
        {
            var entity = await _categoryRepository.FindNoTrackingAsync(id);
            if (entity == null)
                throw new DataNotFoundException("Category not found");

            var response = _mapper.Map<CategoryResponse>(entity);

            return response;
        }

        
        public async Task CreateAsync(CategoryRequest request)
        {
            if (request.Id > 0)
                throw new DataValidationException("id must not to be set");

            var entity = _mapper.Map<Category>(request);

            await _categoryRepository.AddAsync(entity);
           
        }

        public async Task UpdateAsync(CategoryRequest request)
        {
            if (!request.Id.HasValue)
                throw new DataValidationException("id must be set");

            var entity = await _categoryRepository.FindNoTrackingAsync(request.Id.Value); 
            if (entity is null)
                throw new DataNotFoundException("Category not found");

            entity = _mapper.Map<Category>(request);
            await _categoryRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _categoryRepository.FindAsync(id);
            if (entity is null)
                throw new DataNotFoundException("Category not found");

            await _categoryRepository.DeleteAsync(entity);

        }

    }
}
