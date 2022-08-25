using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace FileUpload.ApplicantFiles
{
    public class ApplicantFileManager : DomainService
    {
        private readonly IApplicantFileRepository _applicantFileRepository;

        public ApplicantFileManager(IApplicantFileRepository applicantFileRepository)
        {
            _applicantFileRepository = applicantFileRepository;
        }

        public async Task<ApplicantFile> CreateAsync(
        string fileName)
        {
            var applicantFile = new ApplicantFile(
             GuidGenerator.Create(),
             fileName
             );

            return await _applicantFileRepository.InsertAsync(applicantFile);
        }

        public async Task<ApplicantFile> UpdateAsync(
            Guid id,
            string fileName, [CanBeNull] string concurrencyStamp = null
        )
        {
            var queryable = await _applicantFileRepository.GetQueryableAsync();
            var query = queryable.Where(x => x.Id == id);

            var applicantFile = await AsyncExecuter.FirstOrDefaultAsync(query);

            applicantFile.FileName = fileName;

            applicantFile.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _applicantFileRepository.UpdateAsync(applicantFile);
        }

    }
}