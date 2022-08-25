using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace FileUpload.Blazor;

[Dependency(ReplaceServices = true)]
public class FileUploadBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "FileUpload";
}
