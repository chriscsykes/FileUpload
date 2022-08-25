using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace FileUpload.Applicants
{
    public interface IApplicantRepository : IRepository<Applicant, Guid>
    {
        Task<List<Applicant>> GetListAsync(
            string filterText = null,
            string firstName = null,
            string lastName = null,
            string email = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<long> GetCountAsync(
            string filterText = null,
            string firstName = null,
            string lastName = null,
            string email = null,
            CancellationToken cancellationToken = default);
    }
}