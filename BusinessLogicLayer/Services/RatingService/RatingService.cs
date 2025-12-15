using AutoMapper;
using Core.Exceptions;
using DataAccessLayer.Repository.RatingRepository;
using DomainLayer.Entites;
using DomainLayer.Requests;
using DomainLayer.Responses;


namespace BusinessLogicLayer.Services.RatingService
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IMapper _mapper;
        public RatingService(IRatingRepository ratingRepository, IMapper mapper)
        {
            _ratingRepository = ratingRepository;
            _mapper = mapper;
        }

        public async Task<RateResponse> GetAsync(int recipecId, int userId)
        {
            if (recipecId == 0 || userId == 0)
                throw new DataValidationException("Invalid recipecId or userId");

            var entity = await _ratingRepository.GetByIdAsync(recipecId, userId);
            if (entity is null)
                throw new Exception("Rating not found");

            var response = _mapper.Map<RateResponse>(entity);

            return response;
        }

        public async Task CreateAsync(RatingRequest request)
        {
            if (request.Id > 0)
                throw new DataValidationException("id must not to be set");

            var entity = _mapper.Map<Rating>(request);

            await _ratingRepository.AddAsync(entity);

        }

        public async Task UpdateAsync(RatingRequest request)
        {
            if (!request.Id.HasValue)
                throw new DataValidationException("id must be set");

            var entity = await _ratingRepository.FindNoTrackingAsync(request.Id.Value);
            if (entity is null)
                throw new DataNotFoundException("Rating not found");

            entity.Rate = request.Rate;
            await _ratingRepository.UpdateAsync(entity);
        }
    }
}
