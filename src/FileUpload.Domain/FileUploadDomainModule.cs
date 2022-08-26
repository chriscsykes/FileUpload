using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using FileUpload.Localization;
using FileUpload.MultiTenancy;
using Volo.Abp.AuditLogging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Emailing;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.LanguageManagement;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.SettingManagement;
using Volo.Abp.TextTemplateManagement;
using Volo.Saas;
using Volo.Abp.BlobStoring.Database;
using Volo.Abp.Commercial.SuiteTemplates;
using Volo.Abp.Gdpr;
using Volo.Abp.OpenIddict;
using Volo.Abp.PermissionManagement.OpenIddict;
using Volo.Abp.BlobStoring.Azure;
using Microsoft.Extensions.Configuration;
using Volo.Abp.BlobStoring;
using FileUploadStorage;
using FileUpload.Domain.AzureStorage;

namespace FileUpload;

[DependsOn(
    typeof(FileUploadDomainSharedModule),
    typeof(AbpAuditLoggingDomainModule),
    typeof(AbpBackgroundJobsDomainModule),
    typeof(AbpFeatureManagementDomainModule),
    typeof(AbpIdentityProDomainModule),
    typeof(AbpPermissionManagementDomainIdentityModule),
    typeof(AbpOpenIddictDomainModule),
    typeof(AbpPermissionManagementDomainOpenIddictModule),
    typeof(AbpSettingManagementDomainModule),
    typeof(SaasDomainModule),
    typeof(TextTemplateManagementDomainModule),
    typeof(LanguageManagementDomainModule),
    typeof(VoloAbpCommercialSuiteTemplatesModule),
    typeof(AbpEmailingModule),
    typeof(AbpGdprDomainModule),
    typeof(BlobStoringDatabaseDomainModule)
    )]
[DependsOn(typeof(AbpBlobStoringAzureModule))]
    public class FileUploadDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpMultiTenancyOptions>(options =>
        {
            options.IsEnabled = MultiTenancyConsts.IsEnabled;
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Languages.Add(new LanguageInfo("ar", "ar", "العربية", "ae"));
            options.Languages.Add(new LanguageInfo("en", "en", "English", "gb"));
            options.Languages.Add(new LanguageInfo("fi", "fi", "Finnish", "fi"));
            options.Languages.Add(new LanguageInfo("fr", "fr", "Français", "fr"));
            options.Languages.Add(new LanguageInfo("hi", "hi", "Hindi", "in"));
            options.Languages.Add(new LanguageInfo("it", "it", "Italiano", "it"));
            options.Languages.Add(new LanguageInfo("sk", "sk", "Slovak", "sk"));
            options.Languages.Add(new LanguageInfo("ru", "ru", "Русский", "ru"));
            options.Languages.Add(new LanguageInfo("tr", "tr", "Türkçe", "tr"));
            options.Languages.Add(new LanguageInfo("sl", "sl", "Slovenščina", "si"));
            options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文", "cn"));
            options.Languages.Add(new LanguageInfo("zh-Hant", "zh-Hant", "繁體中文", "tw"));
            options.Languages.Add(new LanguageInfo("de-DE", "de-DE", "Deutsch", "de"));
            options.Languages.Add(new LanguageInfo("es", "es", "Español", "es"));
            options.Languages.Add(new LanguageInfo("nl", "nl", "Dutch", "nl"));
        });

        // from https://community.abp.io/posts/uploaddownload-files-to-azure-storage-with-the-abp-framework-sr7t3w4p
        var configuration = context.Services.GetConfiguration(); 
        ConfigureAzureStorageAccountOptions(context, configuration); 
        ConfigureAbpBlobStoringOptions(configuration);

#if DEBUG
        context.Services.Replace(ServiceDescriptor.Singleton<IEmailSender, NullEmailSender>());
#endif
    }

    // from https://community.abp.io/posts/uploaddownload-files-to-azure-storage-with-the-abp-framework-sr7t3w4p
    private void ConfigureAzureStorageAccountOptions(ServiceConfigurationContext context, IConfiguration configuration)
    {
        Configure<AzureStorageAccountOptions>(options =>
        {
            var azureStorageConnectionString = configuration["AzureStorageAccountSettings:ConnectionString"];
            var azureStorageAccountUrl = configuration["AzureStorageAccountSettings:AccountUrl"];

            options.ConnectionString = azureStorageConnectionString;
            options.AccountUrl = azureStorageAccountUrl;
        });
    }




    // from https://community.abp.io/posts/uploaddownload-files-to-azure-storage-with-the-abp-framework-sr7t3w4p
    private void ConfigureAbpBlobStoringOptions(IConfiguration configuration)
    {
        Configure<AbpBlobStoringOptions>(options =>
        {
            var azureStorageConnectionString = configuration["AzureStorageAccountSettings:ConnectionString"];
            options.Containers.Configure<PizzaPictureContainer>(container =>
            {
                container.UseAzure(azure =>
                {
                    azure.ConnectionString = azureStorageConnectionString;
                    azure.CreateContainerIfNotExists = true;
                });
            });
        });
    }

}
