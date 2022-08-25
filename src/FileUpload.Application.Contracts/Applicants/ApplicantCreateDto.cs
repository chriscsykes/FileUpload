using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace FileUpload.Applicants
{
    public class ApplicantCreateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}