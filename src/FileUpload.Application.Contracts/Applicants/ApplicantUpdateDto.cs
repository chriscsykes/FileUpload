using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace FileUpload.Applicants
{
    public class ApplicantUpdateDto : IHasConcurrencyStamp
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}