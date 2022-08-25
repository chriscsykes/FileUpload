using System;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace FileUpload.ApplicantFiles
{
    public class ApplicantFilesAppServiceTests : FileUploadApplicationTestBase
    {
        private readonly IApplicantFilesAppService _applicantFilesAppService;
        private readonly IRepository<ApplicantFile, Guid> _applicantFileRepository;

        public ApplicantFilesAppServiceTests()
        {
            _applicantFilesAppService = GetRequiredService<IApplicantFilesAppService>();
            _applicantFileRepository = GetRequiredService<IRepository<ApplicantFile, Guid>>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Act
            var result = await _applicantFilesAppService.GetListAsync(new GetApplicantFilesInput());

            // Assert
            result.TotalCount.ShouldBe(2);
            result.Items.Count.ShouldBe(2);
            result.Items.Any(x => x.Id == Guid.Parse("8dd1cd5e-eb66-4c6a-9763-d6dd028c280c")).ShouldBe(true);
            result.Items.Any(x => x.Id == Guid.Parse("82949062-5718-4717-afc3-66d08c3cfe21")).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _applicantFilesAppService.GetAsync(Guid.Parse("8dd1cd5e-eb66-4c6a-9763-d6dd028c280c"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("8dd1cd5e-eb66-4c6a-9763-d6dd028c280c"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new ApplicantFileCreateDto
            {
                FileName = "52920a280b8448279b5b52c6bf360f86ccce83ccbb144c05921b7ad0651e039902810e3b4aa14faf"
            };

            // Act
            var serviceResult = await _applicantFilesAppService.CreateAsync(input);

            // Assert
            var result = await _applicantFileRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.FileName.ShouldBe("52920a280b8448279b5b52c6bf360f86ccce83ccbb144c05921b7ad0651e039902810e3b4aa14faf");
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new ApplicantFileUpdateDto()
            {
                FileName = "b2db45e493e"
            };

            // Act
            var serviceResult = await _applicantFilesAppService.UpdateAsync(Guid.Parse("8dd1cd5e-eb66-4c6a-9763-d6dd028c280c"), input);

            // Assert
            var result = await _applicantFileRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.FileName.ShouldBe("b2db45e493e");
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _applicantFilesAppService.DeleteAsync(Guid.Parse("8dd1cd5e-eb66-4c6a-9763-d6dd028c280c"));

            // Assert
            var result = await _applicantFileRepository.FindAsync(c => c.Id == Guid.Parse("8dd1cd5e-eb66-4c6a-9763-d6dd028c280c"));

            result.ShouldBeNull();
        }
    }
}