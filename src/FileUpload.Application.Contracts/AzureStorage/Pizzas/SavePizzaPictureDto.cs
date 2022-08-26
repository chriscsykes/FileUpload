using System;
using System.Collections.Generic;
using System.Text;

namespace FileUpload.Application.Contracts.AzureStorage.Pizzas
{
    public class SavePizzaPictureDto
    {
        public string FileName { get; set; }
        public byte[] Content { get; set; }
    }
}