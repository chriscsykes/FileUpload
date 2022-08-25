using System;
using FileUpload.Shared;
using Volo.Abp.AutoMapper;
using FileUpload.Applicants;
using AutoMapper;

namespace FileUpload;

public class FileUploadApplicationAutoMapperProfile : Profile
{
    public FileUploadApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<Applicant, ApplicantDto>();
        CreateMap<Applicant, ApplicantExcelDto>();
    }
}