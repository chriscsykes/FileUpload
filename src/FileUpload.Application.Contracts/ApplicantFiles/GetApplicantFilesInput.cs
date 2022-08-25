using Volo.Abp.Application.Dtos;
using System;

namespace FileUpload.ApplicantFiles
{
    public class GetApplicantFilesInput : PagedAndSortedResultRequestDto
    {
        public string FilterText { get; set; }

        public string FileName { get; set; }

        public GetApplicantFilesInput()
        {

        }
    }
}