using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using FileUpload.ApplicantFiles;
using FileUpload.EntityFrameworkCore;
using Xunit;

namespace FileUpload.ApplicantFiles
{
    public class ApplicantFileRepositoryTests : FileUploadEntityFrameworkCoreTestBase
    {
        private readonly IApplicantFileRepository _applicantFileRepository;

        public ApplicantFileRepositoryTests()
        {
            _applicantFileRepository = GetRequiredService<IApplicantFileRepository>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _applicantFileRepository.GetListAsync(
                    fileName: "b3af4efcdb1c497095101a83f2256fbd80bbde7745754a759b8a2fb671a2bf07f0bbeddf409446e69a78927d9"
                );

                // Assert
                result.Count.ShouldBe(1);
                result.FirstOrDefault().ShouldNotBe(null);
                result.First().Id.ShouldBe(Guid.Parse("8dd1cd5e-eb66-4c6a-9763-d6dd028c280c"));
            });
        }

        [Fact]
        public async Task GetCountAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _applicantFileRepository.GetCountAsync(
                    fileName: "e20ccb9c865b48eaa"
                );

                // Assert
                result.ShouldBe(1);
            });
        }
    }
}