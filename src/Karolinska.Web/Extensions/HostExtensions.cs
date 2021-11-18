using AutoFixture;
using Karolinska.Core.Entities;
using Karolinska.Infrastructure.Contexts;

namespace Karolinska.Web.Extensions
{
    public static class HostExtensions
    {
        private static IFixture _fixture = new Fixture();

        public static IHost SeedData(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                ArgumentNullException.ThrowIfNull(services);

                using var context = services.GetService<KarolinskaContext>();

                ArgumentNullException.ThrowIfNull(context);

                var astraZeneca = _fixture.Create<Supplier>();

                context.Suppliers.Add(astraZeneca);

                context.SaveChanges();

                var regionUpplandId = Guid.Parse("04487af1-74bb-4b4a-a9b8-780c013301e8");

                var healthcareProvider = new HealthcareProvider
                {
                    Id = regionUpplandId,
                    Name = "Region Uppland",
                    CapacityReports = Enumerable.Range(1, new Random().Next(10))
                        .Select(e => CreateCapacityReport(regionUpplandId, astraZeneca.Id)).ToArray(),
                    OrderReports = Enumerable.Range(1, new Random().Next(10))
                        .Select(e => CreateOrderReport(regionUpplandId)).ToArray(),
                    ReceiptReports = Enumerable.Range(1, new Random().Next(10))
                        .Select(e => CreateReceiptReport(regionUpplandId, astraZeneca.Id)).ToArray(),
                    StockBalanceReports = Enumerable.Range(1, new Random().Next(10))
                        .Select(e => CreateStockBalanceReport(regionUpplandId, astraZeneca.Id)).ToArray(),
                    ExpenditureReports = Enumerable.Range(1, new Random().Next(10))
                        .Select(e => CreateExpenditureReport(regionUpplandId, astraZeneca.Id)).ToArray()
                };

                context.HealthcareProviders.Add(healthcareProvider);

                context.SaveChanges();
            }

            return host;
        }

        private static CapacityReport CreateCapacityReport(Guid providerId, Guid supplierId)
        {
            return new CapacityReport
            {
                Id = _fixture.Create<Guid>(),
                Date = _fixture.Create<DateTime>().Date,
                HealthcareProviderId = providerId,
                NumberOfVials = _fixture.Create<int>(),
                SupplierId = supplierId
            };
        }

        private static OrderReport CreateOrderReport(Guid providerId)
        {
            return new OrderReport
            {
                Id = _fixture.Create<Guid>(),
                GLNReceiver = _fixture.Create<string>(),
                OrderDate = _fixture.Create<DateTime?>().Value.Date,
                Quantity = _fixture.Create<int>(),
                RequestedDeliveryDate = _fixture.Create<DateTime?>().Value.Date,
                HealthcareProviderId = providerId
            };
        }

        private static ReceiptReport CreateReceiptReport(Guid providerId, Guid supplierId)
        {
            return new ReceiptReport
            {
                Id = _fixture.Create<Guid>(),
                GLNReceiver = _fixture.Create<string>(),
                DeliveryDate = _fixture.Create<DateTime?>().Value.Date,
                ExpectedDeliveryDate = _fixture.Create<DateTime>().Date,
                NumberOfVials = _fixture.Create<int>(),
                SupplierId = supplierId,
                HealthcareProviderId = providerId
            };
        }

        private static StockBalanceReport CreateStockBalanceReport(Guid providerId, Guid supplierId)
        {
            return new StockBalanceReport
            {
                Id = _fixture.Create<Guid>(),
                HealthcareProviderId = providerId,
                Date = _fixture.Create<DateTime>().Date,
                NumberOfDosages = _fixture.Create<int>(),
                NumberOfVials = _fixture.Create<int>(),
                SupplierId = supplierId
            };
        }

        private static ExpenditureReport CreateExpenditureReport(Guid providerId, Guid supplierId)
        {
            return new ExpenditureReport
            {
                Id = _fixture.Create<Guid>(),
                HealthcareProviderId = providerId,
                Date = _fixture.Create<DateTime>().Date,
                NumberOfVials = _fixture.Create<int>(),
                SupplierId = supplierId
            };
        }
    }
}
