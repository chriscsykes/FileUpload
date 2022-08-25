using FileUpload.Localization;
using Volo.Abp.AspNetCore.Components;

namespace FileUpload.Blazor;

public abstract class FileUploadComponentBase : AbpComponentBase
{
    protected FileUploadComponentBase()
    {
        LocalizationResource = typeof(FileUploadResource);
    }
}
