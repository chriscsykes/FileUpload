using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using FileUpload.ApplicantFiles;

namespace FileUpload.ApplicantFiles
{
    public class ApplicantFilesDataSeedContributor : IDataSeedContributor, ISingletonDependency
    {
        private bool IsSeeded = false;
        private readonly IApplicantFileRepository _applicantFileRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public ApplicantFilesDataSeedContributor(IApplicantFileRepository applicantFileRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _applicantFileRepository = applicantFileRepository;
            _unitOfWorkManager = unitOfWorkManager;

        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (IsSeeded)
            {
                return;
            }

            await _applicantFileRepository.InsertAsync(new ApplicantFile
            (
                id: Guid.Parse("73229655-a50f-43dc-97dd-a68ed41be5a2"),
                fileName: "51a1531eac274c3ead2165b3fab70e0613ed823581f2495f96012f2ca1bba6736f567724901d4c"
            ));

            await _applicantFileRepository.InsertAsync(new ApplicantFile
            (
                id: Guid.Parse("f74b859d-d082-47d4-90e4-768a5fef7a55"),
                fileName: "0c008fc90e0b40819c4e4a689f22c5e26f4f3af3fba94ec2add1d210a00b26249d5b1174c55442f8a"
            ));

            await _unitOfWorkManager.Current.SaveChangesAsync();

            IsSeeded = true;
        }
    }
}