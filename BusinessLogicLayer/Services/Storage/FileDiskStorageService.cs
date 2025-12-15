using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;


namespace BusinessLogicLayer.Services.Storage
{
    public class FileDiskStorageService : IStorageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FileDiskStorageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> FileSaveAsync(IFormFile file, string dirctory)
        {
            if (file is null)
                return "";

            var fileInfo = new FileInfo(file.FileName);
            var extension = fileInfo.Extension;
            var fileNameWithExtension = $"{Guid.NewGuid()}{extension}";

            var path = Path.Combine(_webHostEnvironment.WebRootPath, dirctory, fileNameWithExtension);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileNameWithExtension;
        }

        public async Task<string> ReplaceFileAsync(IFormFile file, string directory, string oldFile)
        {
            if (file is null)
                return oldFile;

            if (!string.IsNullOrEmpty(oldFile))
                DeleteFile(oldFile);

            return await FileSaveAsync(file, directory);
        }

        public void DeleteFile(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                var oldPath = $"{_webHostEnvironment.WebRootPath}\\{fileName}";
                File.Delete(oldPath);
            }
        }
    }
}
