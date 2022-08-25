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
            result.Items.Any(x => x.Id == Guid.Parse("73229655-a50f-43dc-97dd-a68ed41be5a2")).ShouldBe(true);
            result.Items.Any(x => x.Id == Guid.Parse("f74b859d-d082-47d4-90e4-768a5fef7a55")).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _applicantFilesAppService.GetAsync(Guid.Parse("73229655-a50f-43dc-97dd-a68ed41be5a2"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("73229655-a50f-43dc-97dd-a68ed41be5a2"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new ApplicantFileCreateDto
            {
                FileName = "3690b04ec5634d5ba40d849446ac74f986035154b089438593556333eae9fd2e5a984be54c674db683025ec32"
            };

            // Act
            var serviceResult = await _applicantFilesAppService.CreateAsync(input);

            // Assert
            var result = await _applicantFileRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.FileName.ShouldBe("3690b04ec5634d5ba40d849446ac74f986035154b089438593556333eae9fd2e5a984be54c674db683025ec32");
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new ApplicantFileUpdateDto()
            {
                FileName = "a6cd4c17036846dbad55afafee01a4bb5d0da960b8684e45b366057b31212ba5b9aeffbff7944966b1c76edfc09edfc8"
            };

            // Act
            var serviceResult = await _applicantFilesAppService.UpdateAsync(Guid.Parse("73229655-a50f-43dc-97dd-a68ed41be5a2"), input);

            // Assert
            var result = await _applicantFileRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.FileName.ShouldBe("a6cd4c17036846dbad55afafee01a4bb5d0da960b8684e45b366057b31212ba5b9aeffbff7944966b1c76edfc09edfc8");
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _applicantFilesAppService.DeleteAsync(Guid.Parse("73229655-a50f-43dc-97dd-a68ed41be5a2"));

            // Assert
            var result = await _applicantFileRepository.FindAsync(c => c.Id == Guid.Parse("73229655-a50f-43dc-97dd-a68ed41be5a2"));

            result.ShouldBeNull();
        }
    }
}