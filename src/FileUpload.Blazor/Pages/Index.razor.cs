using System;
using System.Linq;
using System.Threading.Tasks;
using FileUpload.Application.Contracts.AzureStorage.Pizzas;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace FileUpload.Blazor.Pages
{
    public partial class Index
    {
        [Inject] protected IPizzaAppService PizzaAppService { get; set; }
        public SavedPizzaPictureDto SavedPizzaPictureDto { get; set; } = new SavedPizzaPictureDto();
        protected string PictureUrl;

        protected async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            var file = e.GetMultipleFiles(1).FirstOrDefault();
            var byteArray = new byte[file.Size];
            await file.OpenReadStream().ReadAsync(byteArray);

            SavedPizzaPictureDto = await PizzaAppService.SavePizzaPicture(new SavePizzaPictureDto { Content = byteArray, FileName = file.Name }); ;

            var format = "image/png";
            var imageFile = (e.GetMultipleFiles(1)).FirstOrDefault();
            var resizedImageFile = await imageFile.RequestImageFileAsync(format, 100, 100);
            var buffer = new byte[resizedImageFile.Size];
            await resizedImageFile.OpenReadStream().ReadAsync(buffer);
            PictureUrl = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
        }
    }
}