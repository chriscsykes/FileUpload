using Volo.Abp.Application.Dtos;
using System;

namespace FileUpload.Applicants
{
    public class GetApplicantsInput : PagedAndSortedResultRequestDto
    {
        public string FilterText { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public GetApplicantsInput()
        {

        }
    }
}