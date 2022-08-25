using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace FileUpload.ApplicantFiles
{
    public class ApplicantFileDto : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string FileName { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}