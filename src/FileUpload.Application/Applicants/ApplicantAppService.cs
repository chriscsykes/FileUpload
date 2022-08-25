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
using FileUpload.Applicants;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using FileUpload.Shared;

namespace FileUpload.Applicants
{

    [Authorize(FileUploadPermissions.Applicants.Default)]
    public class ApplicantsAppService : ApplicationService, IApplicantsAppService
    {
        private readonly IDistributedCache<ApplicantExcelDownloadTokenCacheItem, string> _excelDownloadTokenCache;
        private readonly IApplicantRepository _applicantRepository;
        private readonly ApplicantManager _applicantManager;

        public ApplicantsAppService(IApplicantRepository applicantRepository, ApplicantManager applicantManager, IDistributedCache<ApplicantExcelDownloadTokenCacheItem, string> excelDownloadTokenCache)
        {
            _excelDownloadTokenCache = excelDownloadTokenCache;
            _applicantRepository = applicantRepository;
            _applicantManager = applicantManager;
        }

        public virtual async Task<PagedResultDto<ApplicantDto>> GetListAsync(GetApplicantsInput input)
        {
            var totalCount = await _applicantRepository.GetCountAsync(input.FilterText, input.FirstName, input.LastName, input.Email);
            var items = await _applicantRepository.GetListAsync(input.FilterText, input.FirstName, input.LastName, input.Email, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<ApplicantDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Applicant>, List<ApplicantDto>>(items)
            };
        }

        public virtual async Task<ApplicantDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Applicant, ApplicantDto>(await _applicantRepository.GetAsync(id));
        }

        [Authorize(FileUploadPermissions.Applicants.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _applicantRepository.DeleteAsync(id);
        }

        [Authorize(FileUploadPermissions.Applicants.Create)]
        public virtual async Task<ApplicantDto> CreateAsync(ApplicantCreateDto input)
        {

            var applicant = await _applicantManager.CreateAsync(
            input.FirstName, input.LastName, input.Email
            );

            return ObjectMapper.Map<Applicant, ApplicantDto>(applicant);
        }

        [Authorize(FileUploadPermissions.Applicants.Edit)]
        public virtual async Task<ApplicantDto> UpdateAsync(Guid id, ApplicantUpdateDto input)
        {

            var applicant = await _applicantManager.UpdateAsync(
            id,
            input.FirstName, input.LastName, input.Email, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<Applicant, ApplicantDto>(applicant);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(ApplicantExcelDownloadDto input)
        {
            var downloadToken = await _excelDownloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var items = await _applicantRepository.GetListAsync(input.FilterText, input.FirstName, input.LastName, input.Email);

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(ObjectMapper.Map<List<Applicant>, List<ApplicantExcelDto>>(items));
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "Applicants.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public async Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _excelDownloadTokenCache.SetAsync(
                token,
                new ApplicantExcelDownloadTokenCacheItem { Token = token },
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                });

            return new DownloadTokenResultDto
            {
                Token = token
            };
        }
    }
}