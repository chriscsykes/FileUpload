using System;
using System.IO;
using System.Threading.Tasks;
using FileUploadStorage;
using Microsoft.Extensions.Options;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Services;

namespace FileUpload.Domain.AzureStorage.Pizzas
{
    public class PizzaPictureContainerManager : DomainService
    {
        private readonly IBlobContainer<PizzaPictureContainer> _pizzaPictureContainer;
        private readonly AzureStorageAccountOptions _azureStorageAccountOptions;

        public PizzaPictureContainerManager(IBlobContainer<PizzaPictureContainer> pizzaPictureContainer, IOptions<AzureStorageAccountOptions> azureStorageAccountOptions)
        {
            _pizzaPictureContainer = pizzaPictureContainer;
            _azureStorageAccountOptions = azureStorageAccountOptions.Value;
        }

        public async Task<string> SaveAsync(string fileName, byte[] byteArray, bool overrideExisting = false)
        {
            var extension = Path.GetExtension(fileName);
            var storageFileName = $"{Path.GetFileNameWithoutExtension(fileName)}_{Guid.NewGuid()}{extension}";
            await _pizzaPictureContainer.SaveAsync(storageFileName, byteArray, overrideExisting);
            return storageFileName;
        }
    }
}