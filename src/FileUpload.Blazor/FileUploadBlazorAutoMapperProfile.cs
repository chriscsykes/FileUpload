using Volo.Abp.AutoMapper;
using FileUpload.Applicants;
using AutoMapper;

namespace FileUpload.Blazor;

public class FileUploadBlazorAutoMapperProfile : Profile
{
    public FileUploadBlazorAutoMapperProfile()
    {
        //Define your AutoMapper configuration here for the Blazor project.

        CreateMap<ApplicantDto, ApplicantUpdateDto>();
    }
}