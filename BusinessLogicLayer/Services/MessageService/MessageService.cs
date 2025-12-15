using AutoMapper;
using Core.Exceptions;
using Core.Model;
using DataAccessLayer.Repository.MessageRepository;
using DomainLayer.Entites;
using DomainLayer.Requests;
using DomainLayer.Responses;


namespace BusinessLogicLayer.Services.MessageService
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;
        public MessageService(IMessageRepository messageRepository, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<List<MessageResponse>>> GetAllAsync(int skip, int take, int senderId, int recevierId)
        {
            var totalCount = await _messageRepository.CountAsync();
            var list = await _messageRepository.GetAllMessageAsync(skip, take, senderId, recevierId);
            var response = list.Select(x => _mapper.Map<MessageResponse>(x)).ToList();

            return new PagedResponse<List<MessageResponse>>
            {
                Data = response,
                TotalCount = totalCount,
            };
        }


        public async Task<MessageResponse> GetAsync(int id)
        {
            var message = await _messageRepository.GetByIdAsync(id);
            if (message is null)
                throw new DataNotFoundException("Message not found");

            var response = _mapper.Map<MessageResponse>(message);
            return response;
        }

        public async Task CreateAsync(MessageRequest request)
        {
            if (request.Id > 0)
                throw new DataValidationException("Id must not to be set");

            var message = _mapper.Map<Message>(request);
            await _messageRepository.AddAsync(message);
        }

        public async Task UpdateAsync(MessageRequest request)
        {
            if (!request.Id.HasValue)
                throw new DataValidationException("Id must be set");

            var message = await _messageRepository.FindNoTrackingAsync(request.Id.Value);
            if (message is null)
                throw new DataNotFoundException("Message not found");

            message = _mapper.Map<Message>(request);

            await _messageRepository.UpdateAsync(message);
        }

        public async Task DeleteAsync(int id)
        {
            var message = await _messageRepository.FindAsync(id);
            if (message is null)
                throw new DataNotFoundException("Message not found");

            await _messageRepository.DeleteAsync(message);
        }
    }
}
