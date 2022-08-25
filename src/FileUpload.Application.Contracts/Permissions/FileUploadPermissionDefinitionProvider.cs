using FileUpload.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace FileUpload.Permissions;

public class FileUploadPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(FileUploadPermissions.GroupName);

        myGroup.AddPermission(FileUploadPermissions.Dashboard.Host, L("Permission:Dashboard"), MultiTenancySides.Host);
        myGroup.AddPermission(FileUploadPermissions.Dashboard.Tenant, L("Permission:Dashboard"), MultiTenancySides.Tenant);

        //Define your own permissions here. Example:
        //myGroup.AddPermission(FileUploadPermissions.MyPermission1, L("Permission:MyPermission1"));

        var applicantPermission = myGroup.AddPermission(FileUploadPermissions.Applicants.Default, L("Permission:Applicants"));
        applicantPermission.AddChild(FileUploadPermissions.Applicants.Create, L("Permission:Create"));
        applicantPermission.AddChild(FileUploadPermissions.Applicants.Edit, L("Permission:Edit"));
        applicantPermission.AddChild(FileUploadPermissions.Applicants.Delete, L("Permission:Delete"));

        var applicantFilePermission = myGroup.AddPermission(FileUploadPermissions.ApplicantFiles.Default, L("Permission:ApplicantFiles"));
        applicantFilePermission.AddChild(FileUploadPermissions.ApplicantFiles.Create, L("Permission:Create"));
        applicantFilePermission.AddChild(FileUploadPermissions.ApplicantFiles.Edit, L("Permission:Edit"));
        applicantFilePermission.AddChild(FileUploadPermissions.ApplicantFiles.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<FileUploadResource>(name);
    }
}