using AutoFixture;
using AutoFixture.Idioms;
using AutoMapper;
using Karolinska.Application.Dtos;
using Karolinska.Application.Features.Commands;
using Karolinska.Application.Features.Queries;
using Karolinska.Application.Wrappers;
using Karolinska.Core.Entities;
using Karolinska.Infrastructure.Contexts;
using Karolinska.Web.Controllers;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using System;
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
        public async Task GetHealthcareProviders_should_return_pagedResponseOfTypeHealthcareProviderDto(IMapper mapper, HealthcareProvider healthcareProvider, IFixture fixture)
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

        [Test, CustomAutoData]
        public async Task GetHealthcareProvider_should_return_okObjectResult(IMapper mapper, HealthcareProvider healthcareProvider, string dbName)
        {
            var dbContextOptions = new DbContextOptionsBuilder<KarolinskaContext>()
                  .UseInMemoryDatabase(databaseName: dbName)
                  .Options;

            using var context = new KarolinskaContext(dbContextOptions);

            await context.HealthcareProviders.AddAsync(healthcareProvider);

            await context.SaveChangesAsync();

            var sut = new HealthcareProviderController(NullLogger<HealthcareProviderController>.Instance);

            var actionResult = await sut.GetHealthcareProvider(new GetHealthcareProviderByIdQueryHandler(context, mapper), healthcareProvider.Id, CancellationToken.None);

            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());

            var okObjectResult = (OkObjectResult)actionResult;

            Assert.That(okObjectResult.Value, Is.TypeOf<HealthcareProviderDto>());

            var healthcareProviderDto = (HealthcareProviderDto)okObjectResult.Value;

            Assert.That(healthcareProviderDto.Id, Is.EqualTo(healthcareProvider.Id));
        }

        [Test, CustomAutoData]
        public async Task GetHealthcareProvider_should_return_notFoundResult(IMapper mapper, Guid healthcareProviderId, string dbName)
        {
            var dbContextOptions = new DbContextOptionsBuilder<KarolinskaContext>()
                  .UseInMemoryDatabase(databaseName: dbName)
                  .Options;

            using var context = new KarolinskaContext(dbContextOptions);

            var sut = new HealthcareProviderController(NullLogger<HealthcareProviderController>.Instance);

            var actionResult = await sut.GetHealthcareProvider(new GetHealthcareProviderByIdQueryHandler(context, mapper), healthcareProviderId, CancellationToken.None);

            Assert.That(actionResult, Is.TypeOf<NotFoundResult>());
        }

        [Test, CustomAutoData]
        public async Task AddOrderReport_should_return_CreatedAtActionResult(IMapper mapper, HealthcareProvider healthcareProvider, IFixture fixture)
        {
            var dbContextOptions = new DbContextOptionsBuilder<KarolinskaContext>()
                  .UseInMemoryDatabase(databaseName: fixture.Create<string>())
                  .Options;

            using var context = new KarolinskaContext(dbContextOptions);

            await context.AddAsync(healthcareProvider);

            await context.SaveChangesAsync();

            var command = fixture.Build<CreateOrderReportCommand>().With(e => e.HealthcareProviderId, healthcareProvider.Id).Create();

            var sut = new HealthcareProviderController(NullLogger<HealthcareProviderController>.Instance);

            var commandHandler = new CreateOrderReportCommandHandler(context, mapper);

            var actionResult = await sut.AddOrderReport(commandHandler, command.HealthcareProviderId, command, CancellationToken.None);

            Assert.That(actionResult, Is.TypeOf<CreatedAtActionResult>());

            var createdAtActionResult = (CreatedAtActionResult)actionResult;

            Assert.That(createdAtActionResult.Value, Is.TypeOf<OrderReportDto>());
        }

        [Test, CustomAutoData]
        public async Task UpdateCapacityReport_should_return_OkObjectResult(IMapper mapper, CapacityReport capacityReport, int numberOfDoses, IFixture fixture)
        {
            var dbContextOptions = new DbContextOptionsBuilder<KarolinskaContext>()
                  .UseInMemoryDatabase(databaseName: fixture.Create<string>())
                  .Options;

            using var context = new KarolinskaContext(dbContextOptions);

            await context.CapacityReports.AddAsync(capacityReport);

            await context.SaveChangesAsync();

            var sut = new HealthcareProviderController(NullLogger<HealthcareProviderController>.Instance);

            var commandHandler = new UpdateCapacityReportCommandHandler(context, mapper);

            var patchDocument = new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<CapacityReportDto>();

            patchDocument.Operations.Add(new Operation<CapacityReportDto>
            {
                op = nameof(OperationType.Replace),
                path = $"/{nameof(CapacityReportDto.NumberOfDoses)}",
                value = numberOfDoses
            });

            var actionResult = await sut.UpdateCapacityReport(commandHandler, 
                capacityReport.HealthcareProviderId, capacityReport.Id, patchDocument, CancellationToken.None);

            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());

            var okObjectResult = (OkObjectResult)actionResult;

            Assert.That(okObjectResult.Value, Is.TypeOf<CapacityReportDto>());

            var capacityReportDto = (CapacityReportDto)okObjectResult.Value;

            Assert.That(capacityReportDto.NumberOfDoses, Is.EqualTo(numberOfDoses));
        }
    }
}