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
                id: Guid.Parse("8dd1cd5e-eb66-4c6a-9763-d6dd028c280c"),
                fileName: "b3af4efcdb1c497095101a83f2256fbd80bbde7745754a759b8a2fb671a2bf07f0bbeddf409446e69a78927d9"
            ));

            await _applicantFileRepository.InsertAsync(new ApplicantFile
            (
                id: Guid.Parse("82949062-5718-4717-afc3-66d08c3cfe21"),
                fileName: "e20ccb9c865b48eaa"
            ));

            await _unitOfWorkManager.Current.SaveChangesAsync();

            IsSeeded = true;
        }
    }
}