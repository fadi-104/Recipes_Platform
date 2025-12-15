using Core.Model;
using DomainLayer.Requests;
using DomainLayer.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.MessageService
{
    public interface IMessageService
    {
        Task CreateAsync(MessageRequest request);
        Task DeleteAsync(int id);
        Task<PagedResponse<List<MessageResponse>>> GetAllAsync(int skip, int take, int senderId, int recevierId);
        Task<MessageResponse> GetAsync(int id);
        Task UpdateAsync(MessageRequest request);
    }
}
