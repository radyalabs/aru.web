using Trisatech.MWorkforce.Infrastructure.Models;

namespace Trisatech.MWorkforce.Infrastructure.Interfaces
{
    public interface IAzureStorageService
    {
        Task<string> UploadAsync(string blobName, string filePath, string contentType = "");
        Task<string> UploadAsync(string blobName, Stream stream, string contentType = "");
        Task<MemoryStream> DownloadAsync(string blobName);
        Task DownloadAsync(string blobName, string path);
        Task<List<AzureBlobItem>> ListAsync();
        Task<List<string>> ListFoldersAsync();
        Task DeleteAsync(string blobName);
        void ContainerName(string containerName);
        string ContainerName();
    }
}
