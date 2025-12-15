using AutoMapper;
using BusinessLogicLayer.Services.Storage;
using Core.Exceptions;
using Core.Model;
using DataAccessLayer.Repository.FavouriteRepository;
using DataAccessLayer.Repository.ImageReository;
using DataAccessLayer.Repository.RatingRepository;
using DataAccessLayer.Repository.RecipecRepository;
using DomainLayer.Entites;
using DomainLayer.Requests;
using DomainLayer.Responses;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;


namespace BusinessLogicLayer.Services.RecipecService
{
    public class RecipecService : IRecipecService
    {
        private readonly IRecipecRepository _recipecRepository;
        private readonly IMapper _mapper;
        private readonly IRatingRepository _ratingRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IStorageService _storageService;
        private readonly IFavouriteRepository _favouriteRepository;
        private readonly IMemoryCache _cache;
        public RecipecService(IRecipecRepository recipecRepository, IMapper mapper, IRatingRepository ratingRepository, IImageRepository imageRepository,
           IStorageService storageService, IMemoryCache cache, IFavouriteRepository favouriteRepository)
        {
            _recipecRepository = recipecRepository;
            _mapper = mapper;
            _ratingRepository = ratingRepository;
            _imageRepository = imageRepository;
            _storageService = storageService;
            _cache = cache;
            _favouriteRepository = favouriteRepository;
        }


        public async Task<PagedResponse<List<RecipecResponse>>> GetAllAsync(TableOptions options, string? name, bool? isPublished, int? CategoryId, DateTime? date)
        {
            var totalCount = await _recipecRepository.CountAsync();
            var list = await _recipecRepository.GetAllAsNoTracking(options.Skip, options.PageSize, options.OrderBy, options.OrderByDirection, name, isPublished, CategoryId, date);
            var response = list.Select(x => _mapper.Map<RecipecResponse>(x)).ToList();

            return new PagedResponse<List<RecipecResponse>>
            {
                Data = response,
                TotalCount = totalCount,
            };
        }


        public async Task<RecipecResponse> GetAsync(int id, string key)
        {
            var cachKey = $"{key}_recipec_{id}";
            var entity = await _recipecRepository.GetByIdAsync(id);
            if (entity is null)
                throw new Exception("Recipec not found");


            var ratings = await _ratingRepository.GetAverageAsync(id);
            var images = await _imageRepository.GetAllByIdAsync(id);

            var response = _mapper.Map<RecipecResponse>(entity);
            var imagesResponse = _mapper.Map<List<ImageResponse>>(images);

            response.Ingredient = JsonSerializer.Deserialize<List<string>>(entity.Ingredients);
            response.Images = imagesResponse;
            response.AverageRating = (float)Math.Round(ratings, 1);


            if(_cache.TryGetValue(cachKey, out _)){
                return response;
            }

            await _recipecRepository.AddViewCountAsync(entity);
            _cache.Set(key, true, TimeSpan.FromMinutes(20));

            return response;
        }


        public async Task CreateAsync(RecipecRequest request)
        {
            if (request.Id > 0)
                throw new DataValidationException("Id must not to be set");

            var entity = _mapper.Map<Recipec>(request);
            entity.BaseImage = await _storageService.FileSaveAsync(request.BaseImage, "/Project_Structure/Project_Structure/wwwroot/Image/Recipce");
            entity.Ingredients = JsonSerializer.Serialize(request.Ingredients);


            await _recipecRepository.AddAsync(entity);
        }


        public async Task UpdateAsync(RecipecRequest request)
        {
            if (!request.Id.HasValue)
                throw new DataValidationException("Id must be set");

            var entity = await _recipecRepository.GetByIdAsync(request.Id.Value);
            if (entity is null)
                throw new DataNotFoundException("Recipec not found");

            entity = _mapper.Map<Recipec>(request);
            entity.BaseImage = await _storageService.ReplaceFileAsync(request.BaseImage, "/Project_Structure/Project_Structure/wwwroot/Image/Recipce", entity.BaseImage);
            entity.Ingredients = JsonSerializer.Serialize(request.Ingredients);

            await _recipecRepository.UpdateAsync(entity);
        }


        public async Task DeleteAsync(int id)
        {
            var entity = await _recipecRepository.GetByIdAsync(id);
            if (entity is null)
                throw new DataNotFoundException("Recipec not found");

            _storageService.DeleteFile(entity.BaseImage);
            await _recipecRepository.DeleteAsync(entity);

        }

    }
}
