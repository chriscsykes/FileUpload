using Volo.Abp.Application.Dtos;
using System;

namespace FileUpload.Applicants
{
    public class ApplicantExcelDownloadDto
    {
        public string DownloadToken { get; set; }

        public string FilterText { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public ApplicantExcelDownloadDto()
        {

        }
    }
}