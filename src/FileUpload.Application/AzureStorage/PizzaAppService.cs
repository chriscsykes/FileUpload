using System.Threading.Tasks;
using FileUpload.Application.Contracts.AzureStorage.Pizzas;
using FileUpload.Domain.AzureStorage.Pizzas;
using Volo.Abp.Application.Services;

namespace FileUpload.Application.AzureStorage.Pizzas
{

    public class PizzaAppService : ApplicationService, IPizzaAppService
    {
        private readonly PizzaPictureContainerManager _pizzaPictureContainerManager;

        public PizzaAppService(PizzaPictureContainerManager pizzaPictureContainerManager)
        {
            _pizzaPictureContainerManager = pizzaPictureContainerManager;
        }

        public async Task<SavedPizzaPictureDto> SavePizzaPicture(SavePizzaPictureDto input)
        {
            var storageFileName = await _pizzaPictureContainerManager.SaveAsync(input.FileName, input.Content, true);
            return new SavedPizzaPictureDto { StorageFileName = storageFileName };
        }
    }
}