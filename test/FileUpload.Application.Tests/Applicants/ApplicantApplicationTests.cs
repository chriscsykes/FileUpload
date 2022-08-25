using System;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace FileUpload.Applicants
{
    public class ApplicantsAppServiceTests : FileUploadApplicationTestBase
    {
        private readonly IApplicantsAppService _applicantsAppService;
        private readonly IRepository<Applicant, Guid> _applicantRepository;

        public ApplicantsAppServiceTests()
        {
            _applicantsAppService = GetRequiredService<IApplicantsAppService>();
            _applicantRepository = GetRequiredService<IRepository<Applicant, Guid>>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Act
            var result = await _applicantsAppService.GetListAsync(new GetApplicantsInput());

            // Assert
            result.TotalCount.ShouldBe(2);
            result.Items.Count.ShouldBe(2);
            result.Items.Any(x => x.Id == Guid.Parse("a62d613b-556a-4acf-aba7-c16256898a1a")).ShouldBe(true);
            result.Items.Any(x => x.Id == Guid.Parse("152c2112-fb97-44ce-b80e-696a7c821417")).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _applicantsAppService.GetAsync(Guid.Parse("a62d613b-556a-4acf-aba7-c16256898a1a"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("a62d613b-556a-4acf-aba7-c16256898a1a"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new ApplicantCreateDto
            {
                FirstName = "ee33eb23c",
                LastName = "37248156cb7242b88d83",
                Email = "9c037a63ff6d483f83bdea6972b69107be363aa7fb1e4326a606a412c284810bb0b8b21446144e238b5df227515e8"
            };

            // Act
            var serviceResult = await _applicantsAppService.CreateAsync(input);

            // Assert
            var result = await _applicantRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.FirstName.ShouldBe("ee33eb23c");
            result.LastName.ShouldBe("37248156cb7242b88d83");
            result.Email.ShouldBe("9c037a63ff6d483f83bdea6972b69107be363aa7fb1e4326a606a412c284810bb0b8b21446144e238b5df227515e8");
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new ApplicantUpdateDto()
            {
                FirstName = "2f8cc3a574b04518",
                LastName = "60c2dbded1174f1bbb2f714f938e57d7",
                Email = "ea00411f25e945de808b4e80e1ab93ba2540"
            };

            // Act
            var serviceResult = await _applicantsAppService.UpdateAsync(Guid.Parse("a62d613b-556a-4acf-aba7-c16256898a1a"), input);

            // Assert
            var result = await _applicantRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.FirstName.ShouldBe("2f8cc3a574b04518");
            result.LastName.ShouldBe("60c2dbded1174f1bbb2f714f938e57d7");
            result.Email.ShouldBe("ea00411f25e945de808b4e80e1ab93ba2540");
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _applicantsAppService.DeleteAsync(Guid.Parse("a62d613b-556a-4acf-aba7-c16256898a1a"));

            // Assert
            var result = await _applicantRepository.FindAsync(c => c.Id == Guid.Parse("a62d613b-556a-4acf-aba7-c16256898a1a"));

            result.ShouldBeNull();
        }
    }
}