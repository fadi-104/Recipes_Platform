using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.AiService
{
    public interface IAiService
    {
        Task<string> GetAiResponseAsync(List<string> prompt);
        Task<string> SendToN8N(string prompt);
    }
}
