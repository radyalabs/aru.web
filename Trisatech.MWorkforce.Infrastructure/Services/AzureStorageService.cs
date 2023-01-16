using Trisatech.MWorkforce.Infrastructure.Models;
using Trisatech.MWorkforce.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;

namespace Trisatech.MWorkforce.Infrastructure.Services
{
    public class AzureStorageService:IAzureStorageService
    {
        private readonly string _storageAccount;
        private readonly string _storageKey;
        private string containerName;
        public AzureStorageService(string storageAccount, string storageKey, string containerName)
        {
            _storageAccount = storageAccount;
            _storageKey = storageKey;
            this.containerName = containerName;
        }
        
        private async Task<CloudBlobContainer> GetContainerAsync()
        {
            //Account
            CloudStorageAccount storageAccount = new CloudStorageAccount(new StorageCredentials(_storageAccount, _storageKey), false);

            //Client
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            //Container
            CloudBlobContainer blobContainer = blobClient.GetContainerReference(containerName);
            if(await blobContainer.CreateIfNotExistsAsync())
            {
                await blobContainer.SetPermissionsAsync(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });

            }

            return blobContainer;
        }

        private async Task<CloudBlockBlob> GetBlockBlobAsync(string blobName)
        {
            //Container
            CloudBlobContainer blobContainer = await GetContainerAsync();

            //Blob
            CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference(blobName);

            return blockBlob;
        }

        private async Task<List<AzureBlobItem>> GetBlobListAsync(bool useFlatListing = true)
        {
            //Container
            CloudBlobContainer blobContainer = await GetContainerAsync();

            //List
            var list = new List<AzureBlobItem>();
            BlobContinuationToken token = new BlobContinuationToken();

            do
            {
                BlobResultSegment resultSegment =
                    await blobContainer.ListBlobsSegmentedAsync("", useFlatListing, new BlobListingDetails(), null, token, null, null);
                token = resultSegment.ContinuationToken;

                foreach (IListBlobItem item in resultSegment.Results)
                {
                    list.Add(new AzureBlobItem(item));
                }
            } while (token != null);

            return list.OrderBy(i => i.Folder).ThenBy(i => i.Name).ToList();
        }

        public async Task<MemoryStream> DownloadAsync(string blobName)
        {
            //Blob
            CloudBlockBlob blockBlob = await GetBlockBlobAsync(blobName);

            //Download
            using (var stream = new MemoryStream())
            {
                await blockBlob.DownloadToStreamAsync(stream);
                return stream;
            }
        }

        public Task DownloadAsync(string blobName, string path)
        {
            throw new NotImplementedException();
        }

        public Task<List<AzureBlobItem>> ListAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> ListFoldersAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<string> UploadAsync(string blobName, string filePath, string contentType = "")
        {
            //Blob
            CloudBlockBlob blockBlob = await GetBlockBlobAsync(blobName);
            if(!string.IsNullOrEmpty(contentType))
                blockBlob.Properties.ContentType = contentType;
            //Upload
            using (var fileStream = System.IO.File.Open(filePath, FileMode.Open))
            {
                fileStream.Position = 0;
                await blockBlob.UploadFromStreamAsync(fileStream);
            }
            
            return blockBlob.Uri.ToString();
        }

        public async Task<string> UploadAsync(string blobName, Stream stream, string contentType = "")
        {
            //Blob
            CloudBlockBlob blockBlob = await GetBlockBlobAsync(blobName);
            if (!string.IsNullOrEmpty(contentType))
            {
                blockBlob.Properties.ContentType = contentType;
            }

            //Upload
            stream.Position = 0;
            await blockBlob.UploadFromStreamAsync(stream);

            return blockBlob.Uri.ToString();
        }
        public async Task DeleteAsync(string blobName)
        {
            //blob
            CloudBlockBlob blockBlob = await GetBlockBlobAsync(blobName);

            //delete
            await blockBlob.DeleteIfExistsAsync();
        }

        public void ContainerName(string containerName)
        {
            this.containerName = containerName;
        }

        public string ContainerName()
        {
            return containerName;
        }
    }
}
