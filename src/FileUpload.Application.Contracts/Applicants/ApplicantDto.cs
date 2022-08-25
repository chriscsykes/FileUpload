using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace FileUpload.Applicants
{
    public class ApplicantDto : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}