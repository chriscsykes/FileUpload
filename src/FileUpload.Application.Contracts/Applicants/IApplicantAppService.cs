using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using FileUpload.Shared;

namespace FileUpload.Applicants
{
    public interface IApplicantsAppService : IApplicationService
    {
        Task<PagedResultDto<ApplicantDto>> GetListAsync(GetApplicantsInput input);

        Task<ApplicantDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<ApplicantDto> CreateAsync(ApplicantCreateDto input);

        Task<ApplicantDto> UpdateAsync(Guid id, ApplicantUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(ApplicantExcelDownloadDto input);

        Task<DownloadTokenResultDto> GetDownloadTokenAsync();
    }
}