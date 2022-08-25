using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using FileUpload.Applicants;
using FileUpload.EntityFrameworkCore;
using Xunit;

namespace FileUpload.Applicants
{
    public class ApplicantRepositoryTests : FileUploadEntityFrameworkCoreTestBase
    {
        private readonly IApplicantRepository _applicantRepository;

        public ApplicantRepositoryTests()
        {
            _applicantRepository = GetRequiredService<IApplicantRepository>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _applicantRepository.GetListAsync(
                    firstName: "0332e8b6f3664f3c9e025c552ccc8622ea50c8bc6d09459580bfb9",
                    lastName: "bed6cd1394",
                    email: "13b5619415df46489cd2ad187e2b6ecdc663aefaa9b14fac8a5b97a9e47ca0fea8728d7f0"
                );

                // Assert
                result.Count.ShouldBe(1);
                result.FirstOrDefault().ShouldNotBe(null);
                result.First().Id.ShouldBe(Guid.Parse("a62d613b-556a-4acf-aba7-c16256898a1a"));
            });
        }

        [Fact]
        public async Task GetCountAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _applicantRepository.GetCountAsync(
                    firstName: "587485578ae0464ba068030a7f8abeac1dc538f354b149c19d72b1074c993c339901d8026aa740da895",
                    lastName: "5747b4931bbb4072a2704a98ba8f4f4e727f422863134a3f969c07b00860ce58f087939373e7465fa67bc9568ffc0ecbd",
                    email: "8c749024073c4507879ec14385b5d52426bc38c3251f4394b017e4c4d05b0ee9eba7"
                );

                // Assert
                result.ShouldBe(1);
            });
        }
    }
}