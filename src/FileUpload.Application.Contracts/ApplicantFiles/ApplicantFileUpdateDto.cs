using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace FileUpload.ApplicantFiles
{
    public class ApplicantFileUpdateDto : IHasConcurrencyStamp
    {
        public string FileName { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}