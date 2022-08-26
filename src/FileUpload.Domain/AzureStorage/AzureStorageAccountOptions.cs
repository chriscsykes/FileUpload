using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// from https://community.abp.io/posts/uploaddownload-files-to-azure-storage-with-the-abp-framework-sr7t3w4p
namespace FileUploadStorage
{
    public class AzureStorageAccountOptions
    {
        public string ConnectionString { get; set; }
        public string AccountUrl { get; set; }
    }
}
