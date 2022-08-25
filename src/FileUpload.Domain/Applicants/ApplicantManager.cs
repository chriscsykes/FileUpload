using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace FileUpload.Applicants
{
    public class ApplicantManager : DomainService
    {
        private readonly IApplicantRepository _applicantRepository;

        public ApplicantManager(IApplicantRepository applicantRepository)
        {
            _applicantRepository = applicantRepository;
        }

        public async Task<Applicant> CreateAsync(
        string firstName, string lastName, string email)
        {
            var applicant = new Applicant(
             GuidGenerator.Create(),
             firstName, lastName, email
             );

            return await _applicantRepository.InsertAsync(applicant);
        }

        public async Task<Applicant> UpdateAsync(
            Guid id,
            string firstName, string lastName, string email, [CanBeNull] string concurrencyStamp = null
        )
        {
            var queryable = await _applicantRepository.GetQueryableAsync();
            var query = queryable.Where(x => x.Id == id);

            var applicant = await AsyncExecuter.FirstOrDefaultAsync(query);

            applicant.FirstName = firstName;
            applicant.LastName = lastName;
            applicant.Email = email;

            applicant.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _applicantRepository.UpdateAsync(applicant);
        }

    }
}