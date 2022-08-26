using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace FileUpload.Application.Contracts.AzureStorage.Pizzas
{
    public interface IPizzaAppService : IApplicationService
    {
        Task<SavedPizzaPictureDto> SavePizzaPicture(SavePizzaPictureDto input);
    }
}
