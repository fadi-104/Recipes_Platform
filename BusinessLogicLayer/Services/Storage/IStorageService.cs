using Microsoft.AspNetCore.Http;

namespace BusinessLogicLayer.Services.Storage
{
    public interface IStorageService
    {
        void DeleteFile(string fileName);
        Task<string> FileSaveAsync(IFormFile file, string dirctory);
        Task<string> ReplaceFileAsync(IFormFile file, string directory, string oldFile);
    }
}
