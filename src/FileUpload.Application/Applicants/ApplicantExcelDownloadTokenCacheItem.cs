using System;

namespace FileUpload.Applicants;

[Serializable]
public class ApplicantExcelDownloadTokenCacheItem
{
    public string Token { get; set; }
}