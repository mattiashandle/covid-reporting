using AutoFixture;
using AutoFixture.Idioms;
using AutoMapper;
using Karolinska.Application.Dtos;
using Karolinska.Application.Features.Queries;
using Karolinska.Application.Wrappers;
using Karolinska.Core.Entities;
using Karolinska.Infrastructure.Contexts;
using Karolinska.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace Tests.Karolinska.Web.Controllers
{
    [TestFixture]
    public class HealthcareProviderControllerTests
    {
        [Test, CustomAutoData]
        public void Constructor_is_guarded(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(HealthcareProviderController).GetConstructors());
        }

        [Test, CustomAutoData]
        public async Task GetHealthcareProviders_should_return_expected_result(IMapper mapper, HealthcareProvider healthcareProvider, IFixture fixture)
        {
            var dbContextOptions = new DbContextOptionsBuilder<KarolinskaContext>()
                  .UseInMemoryDatabase(databaseName: fixture.Create<string>())
                  .Options;

            using var context = new KarolinskaContext(dbContextOptions);

            await context.HealthcareProviders.AddAsync(healthcareProvider);

            await context.SaveChangesAsync();

            var sut = new HealthcareProviderController(NullLogger<HealthcareProviderController>.Instance);

            var actionResult = await sut.GetHealthcareProviders(new GetHealthcareProvidersQueryHandler(context, mapper), CancellationToken.None);

            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());

            var okObjectResult = (OkObjectResult)actionResult;
            
            Assert.That(okObjectResult.Value, Is.TypeOf<PagedResponse<HealthcareProviderDto[]>>());

            var pagedResponse = (PagedResponse<HealthcareProviderDto[]>)okObjectResult.Value;

            Assert.That(pagedResponse.Data.Length, Is.EqualTo(1));
        }
    }
}