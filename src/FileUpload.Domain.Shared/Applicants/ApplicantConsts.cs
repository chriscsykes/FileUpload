namespace FileUpload.Applicants
{
    public static class ApplicantConsts
    {
        private const string DefaultSorting = "{0}FirstName asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "Applicant." : string.Empty);
        }

    }
}