using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace FileUpload.ApplicantFiles
{
    public interface IApplicantFileRepository : IRepository<ApplicantFile, Guid>
    {
        Task<List<ApplicantFile>> GetListAsync(
            string filterText = null,
            string fileName = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<long> GetCountAsync(
            string filterText = null,
            string fileName = null,
            CancellationToken cancellationToken = default);
    }
}