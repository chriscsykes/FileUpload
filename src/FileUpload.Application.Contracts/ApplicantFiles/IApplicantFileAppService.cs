using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace FileUpload.ApplicantFiles
{
    public interface IApplicantFilesAppService : IApplicationService
    {
        Task<PagedResultDto<ApplicantFileDto>> GetListAsync(GetApplicantFilesInput input);

        Task<ApplicantFileDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<ApplicantFileDto> CreateAsync(ApplicantFileCreateDto input);

        Task<ApplicantFileDto> UpdateAsync(Guid id, ApplicantFileUpdateDto input);
    }
}