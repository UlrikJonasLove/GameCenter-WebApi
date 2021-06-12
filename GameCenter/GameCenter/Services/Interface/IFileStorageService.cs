using System.Threading.Tasks;

namespace GameCenter.Services.Interface
{
    public interface IFileStorageService 
    {
        Task<string> SaveFile(byte[] content, string extension, string containerName, string contentType);
        Task DeleteFile(string fileRoute, string containerName);
        Task<string> EditFile(byte[] content, string extension, string containerName, string fileRoute, string contentType);
    }
}