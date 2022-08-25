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

namespace FileUpload.Applicants
{
    public class EfCoreApplicantRepository : EfCoreRepository<FileUploadDbContext, Applicant, Guid>, IApplicantRepository
    {
        public EfCoreApplicantRepository(IDbContextProvider<FileUploadDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public async Task<List<Applicant>> GetListAsync(
            string filterText = null,
            string firstName = null,
            string lastName = null,
            string email = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, firstName, lastName, email);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ApplicantConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public async Task<long> GetCountAsync(
            string filterText = null,
            string firstName = null,
            string lastName = null,
            string email = null,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, firstName, lastName, email);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<Applicant> ApplyFilter(
            IQueryable<Applicant> query,
            string filterText,
            string firstName = null,
            string lastName = null,
            string email = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.FirstName.Contains(filterText) || e.LastName.Contains(filterText) || e.Email.Contains(filterText))
                    .WhereIf(!string.IsNullOrWhiteSpace(firstName), e => e.FirstName.Contains(firstName))
                    .WhereIf(!string.IsNullOrWhiteSpace(lastName), e => e.LastName.Contains(lastName))
                    .WhereIf(!string.IsNullOrWhiteSpace(email), e => e.Email.Contains(email));
        }
    }
}