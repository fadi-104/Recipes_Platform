using AutoMapper;
using BusinessLogicLayer.Services.Storage;
using Core.Exceptions;
using DataAccessLayer.Repository.ImageReository;
using DomainLayer.Entites;
using DomainLayer.Requests;
using DomainLayer.Responses;
using static System.Net.Mime.MediaTypeNames;


namespace BusinessLogicLayer.Services.ImageService
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;
        private readonly IStorageService _storageService;
        public ImageService(IImageRepository imageRepository, IMapper mapper, IStorageService storageService)
        {
            _imageRepository = imageRepository;
            _mapper = mapper;
            _storageService = storageService;
        }

        public async Task<List<ImageResponse>> GetAllAsync(int recipecId)
        {
            var list = await _imageRepository.GetAllByIdAsync(recipecId);
            var response = list.Select(x => _mapper.Map<ImageResponse>(x)).ToList();

            return response;
        }

        public async Task<ImageResponse> GetByIdAsync(int id)
        {
            var image = await _imageRepository.FindNoTrackingAsync(id);
            if (image is null)
                throw new DataNotFoundException("Image not found");

            var response = _mapper.Map<ImageResponse>(image);
            return response;
        }

        public async Task CreateAsync(ImageRequest request)
        {
           using (var transaction = await _imageRepository.BeginTransactionAsync())
           {
                if (request.Id > 0)
                    throw new DataValidationException("Id must not to be set");

                if (request.Image is null || request.Image.Count == 0)
                    throw new DataValidationException("Image file is required");

                var entites = new List<DomainLayer.Entites.Image>();

                foreach (var img in request.Image)
                {
                    entites.Add(new DomainLayer.Entites.Image
                    {
                        Id = 0,
                        RecipecId = request.RecipecId,
                        ImageUrl = await _storageService.FileSaveAsync(img, "/Project_Structure/Project_Structure/wwwroot/Image/Recipce")
                    });

                }

                await _imageRepository.AddRangeAsync(entites);
                await transaction.CommitAsync();
            }
        }
        public async Task DeleteAsync(int id)
        {
            var entity = await _imageRepository.FindNoTrackingAsync(id);
            if (entity is null)
                throw new DataNotFoundException("Image not found");

            _storageService.DeleteFile(entity.ImageUrl);
            await _imageRepository.DeleteAsync(entity);
        }
    }
}
