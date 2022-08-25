using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using FileUpload.EntityFrameworkCore;

namespace FileUpload.ApplicantFiles
{
    public class EfCoreApplicantFileRepository : EfCoreRepository<FileUploadDbContext, ApplicantFile, Guid>, IApplicantFileRepository
    {
        public EfCoreApplicantFileRepository(IDbContextProvider<FileUploadDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public async Task<List<ApplicantFile>> GetListAsync(
            string filterText = null,
            string fileName = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, fileName);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ApplicantFileConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public async Task<long> GetCountAsync(
            string filterText = null,
            string fileName = null,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, fileName);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<ApplicantFile> ApplyFilter(
            IQueryable<ApplicantFile> query,
            string filterText,
            string fileName = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.FileName.Contains(filterText))
                    .WhereIf(!string.IsNullOrWhiteSpace(fileName), e => e.FileName.Contains(fileName));
        }
    }
}