using AutoMapper;
using Core.Exceptions;
using DataAccessLayer.Repository.FavouriteRepository;
using DomainLayer.Entites;
using DomainLayer.Requests;
using DomainLayer.Responses;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.FavouriteService
{
    public class FavouriteService : IFavouriteService
    {
        private readonly IFavouriteRepository _favouriteRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public FavouriteService(IFavouriteRepository favouriteRepository, IMapper mapper, IMemoryCache cache)
        {
            _favouriteRepository = favouriteRepository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<List<RecipecResponse>> GetFavouriteAsync(int userId)
        {
            var cacheKey = $"favourites_{userId}";
           
            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                entry.SlidingExpiration = TimeSpan.FromMinutes(2);
                entry.SetPriority(CacheItemPriority.Normal);
                entry.Size = 1;
                var list = await _favouriteRepository.GetFavouriteAsync(userId);
                return list.Select(x => _mapper.Map<RecipecResponse>(x)).ToList();
                
            });
        }

        public async Task AddFavouriteAsync(FavouriteRequest request)
        {
            if (request.Id > 0)
                throw new DataValidationException("Id must not to be set");

            var entity = _mapper.Map<Favourite>(request);
            await _favouriteRepository.AddAsync(entity);
        }

        public async Task RemoveFavouriteAsync(int id)
        {
            var entity = await _favouriteRepository.FindAsync(id);
            if (entity == null)
                throw new DataValidationException("Favourite not found");

            await _favouriteRepository.DeleteAsync(entity);
        }
    }
}
