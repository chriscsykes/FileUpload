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
                    fileName: "51a1531eac274c3ead2165b3fab70e0613ed823581f2495f96012f2ca1bba6736f567724901d4c"
                );

                // Assert
                result.Count.ShouldBe(1);
                result.FirstOrDefault().ShouldNotBe(null);
                result.First().Id.ShouldBe(Guid.Parse("73229655-a50f-43dc-97dd-a68ed41be5a2"));
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
                    fileName: "0c008fc90e0b40819c4e4a689f22c5e26f4f3af3fba94ec2add1d210a00b26249d5b1174c55442f8a"
                );

                // Assert
                result.ShouldBe(1);
            });
        }
    }
}