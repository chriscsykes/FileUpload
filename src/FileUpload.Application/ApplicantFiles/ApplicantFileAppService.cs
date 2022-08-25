using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using FileUpload.Permissions;
using FileUpload.ApplicantFiles;

namespace FileUpload.ApplicantFiles
{

    [Authorize(FileUploadPermissions.ApplicantFiles.Default)]
    public class ApplicantFilesAppService : ApplicationService, IApplicantFilesAppService
    {

        private readonly IApplicantFileRepository _applicantFileRepository;
        private readonly ApplicantFileManager _applicantFileManager;

        public ApplicantFilesAppService(IApplicantFileRepository applicantFileRepository, ApplicantFileManager applicantFileManager)
        {

            _applicantFileRepository = applicantFileRepository;
            _applicantFileManager = applicantFileManager;
        }

        public virtual async Task<PagedResultDto<ApplicantFileDto>> GetListAsync(GetApplicantFilesInput input)
        {
            var totalCount = await _applicantFileRepository.GetCountAsync(input.FilterText, input.FileName);
            var items = await _applicantFileRepository.GetListAsync(input.FilterText, input.FileName, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<ApplicantFileDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<ApplicantFile>, List<ApplicantFileDto>>(items)
            };
        }

        public virtual async Task<ApplicantFileDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<ApplicantFile, ApplicantFileDto>(await _applicantFileRepository.GetAsync(id));
        }

        [Authorize(FileUploadPermissions.ApplicantFiles.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _applicantFileRepository.DeleteAsync(id);
        }

        [Authorize(FileUploadPermissions.ApplicantFiles.Create)]
        public virtual async Task<ApplicantFileDto> CreateAsync(ApplicantFileCreateDto input)
        {

            var applicantFile = await _applicantFileManager.CreateAsync(
            input.FileName
            );

            return ObjectMapper.Map<ApplicantFile, ApplicantFileDto>(applicantFile);
        }

        [Authorize(FileUploadPermissions.ApplicantFiles.Edit)]
        public virtual async Task<ApplicantFileDto> UpdateAsync(Guid id, ApplicantFileUpdateDto input)
        {

            var applicantFile = await _applicantFileManager.UpdateAsync(
            id,
            input.FileName, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<ApplicantFile, ApplicantFileDto>(applicantFile);
        }
    }
}