using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace FileUpload.ApplicantFiles
{
    public class ApplicantFile : FullAuditedAggregateRoot<Guid>
    {
        [CanBeNull]
        public virtual string FileName { get; set; }

        public ApplicantFile()
        {

        }

        public ApplicantFile(Guid id, string fileName)
        {

            Id = id;
            FileName = fileName;
        }

    }
}