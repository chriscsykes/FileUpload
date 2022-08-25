namespace FileUpload.ApplicantFiles
{
    public static class ApplicantFileConsts
    {
        private const string DefaultSorting = "{0}FileName asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "ApplicantFile." : string.Empty);
        }

    }
}