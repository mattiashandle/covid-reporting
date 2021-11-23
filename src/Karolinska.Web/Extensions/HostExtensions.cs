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

                var astraZeneca = _fixture.Build<Supplier>().With(e => e.Name, "AztraZenaca").Create();

                context.Suppliers.Add(astraZeneca);

                context.SaveChanges();

                var pfizer = _fixture.Build<Supplier>().With(e => e.Name, "Pfizer").Create();

                context.Suppliers.Add(pfizer);

                context.SaveChanges();

                var danderyId = Guid.Parse("04487af1-74bb-4b4a-a9b8-780c013301e8");

                var fooHealthcareProvider = new HealthcareProvider
                {
                    Id = danderyId,
                    Name = "Danderyds Sjukhus",
                    CapacityReports = Enumerable.Range(1, new Random().Next(10))
                        .Select(e => CreateCapacityReport(danderyId, astraZeneca.Id)).ToArray(),
                    OrderReports = Enumerable.Range(1, new Random().Next(10))
                        .Select(e => CreateOrderReport(danderyId)).ToArray(),
                    ReceiptReports = Enumerable.Range(1, new Random().Next(10))
                        .Select(e => CreateReceiptReport(danderyId, astraZeneca.Id)).ToArray(),
                    StockBalanceReports = Enumerable.Range(1, new Random().Next(10))
                        .Select(e => CreateStockBalanceReport(danderyId, astraZeneca.Id)).ToArray(),
                    ExpenditureReports = Enumerable.Range(1, new Random().Next(10))
                        .Select(e => CreateExpenditureReport(danderyId, astraZeneca.Id)).ToArray()
                };

                context.HealthcareProviders.Add(fooHealthcareProvider);

                context.SaveChanges();

                var huddingeId = Guid.Parse("c5d97ec1-d970-4f33-bd88-e66541533af1");

                var barHealthcareProvider = new HealthcareProvider
                {
                    Id = huddingeId,
                    Name = "Huddinge Sjukhus",
                    CapacityReports = Enumerable.Range(1, new Random().Next(10))
                        .Select(e => CreateCapacityReport(huddingeId, pfizer.Id)).ToArray(),
                    OrderReports = Enumerable.Range(1, new Random().Next(10))
                        .Select(e => CreateOrderReport(huddingeId)).ToArray(),
                    ReceiptReports = Enumerable.Range(1, new Random().Next(10))
                        .Select(e => CreateReceiptReport(huddingeId, pfizer.Id)).ToArray(),
                    StockBalanceReports = Enumerable.Range(1, new Random().Next(10))
                        .Select(e => CreateStockBalanceReport(huddingeId, pfizer.Id)).ToArray(),
                    ExpenditureReports = Enumerable.Range(1, new Random().Next(10))
                        .Select(e => CreateExpenditureReport(huddingeId, pfizer.Id)).ToArray()
                };

                context.HealthcareProviders.Add(barHealthcareProvider);

                context.SaveChanges();

            }

            return host;
        }

        private static CapacityReport CreateCapacityReport(Guid providerId, Guid supplierId)
        {
            var insertDate = DateTime.UtcNow.AddDays(-_fixture.Create<int>());

            return new CapacityReport
            {
                Id = _fixture.Create<Guid>(),
                Date = insertDate,
                HealthcareProviderId = providerId,
                NumberOfDoses = _fixture.Create<int>(),
                InsertDate = insertDate,
            };
        }

        private static OrderReport CreateOrderReport(Guid providerId)
        {
            var insertDate = DateTime.UtcNow.AddDays(-_fixture.Create<int>());

            return new OrderReport
            {
                Id = _fixture.Create<Guid>(),
                GLNReceiver = $"GLN{_fixture.Create<string>()}",
                OrderDate = insertDate.Date,
                Quantity = _fixture.Create<int>(),
                RequestedDeliveryDate = insertDate.AddDays(_fixture.Create<int>()).Date,
                HealthcareProviderId = providerId,
                InsertDate = insertDate
            };
        }

        private static ReceiptReport CreateReceiptReport(Guid providerId, Guid supplierId)
        {
            var insertDate = DateTime.UtcNow.AddDays(-_fixture.Create<int>());

            var expectedDeliveryDate = insertDate.AddDays(_fixture.Create<int>()).Date;

            var deliveryDate = expectedDeliveryDate.AddDays(new Random().Next(0, 10));

            return new ReceiptReport
            {
                Id = _fixture.Create<Guid>(),
                GLNReceiver = $"GLN{_fixture.Create<string>()}",
                DeliveryDate = deliveryDate.Date,
                ExpectedDeliveryDate = expectedDeliveryDate,
                NumberOfVials = _fixture.Create<int>(),
                SupplierId = supplierId,
                HealthcareProviderId = providerId,
                InsertDate = insertDate,
            };
        }

        private static StockBalanceReport CreateStockBalanceReport(Guid providerId, Guid supplierId)
        {
            var insertDate = DateTime.UtcNow.AddDays(-_fixture.Create<int>());

            var stockBalanceDate = insertDate.Date;

            return new StockBalanceReport
            {
                Id = _fixture.Create<Guid>(),
                HealthcareProviderId = providerId,
                Date = stockBalanceDate,
                NumberOfDoses = _fixture.Create<int>(),
                NumberOfVials = _fixture.Create<int>(),
                SupplierId = supplierId,
                InsertDate = insertDate,
            };
        }

        private static ExpenditureReport CreateExpenditureReport(Guid providerId, Guid supplierId)
        {
            var insertDate = DateTime.UtcNow.AddDays(-_fixture.Create<int>());

            return new ExpenditureReport
            {
                Id = _fixture.Create<Guid>(),
                HealthcareProviderId = providerId,
                Date = insertDate.Date,
                NumberOfVials = _fixture.Create<int>(),
                SupplierId = supplierId,
                InsertDate = insertDate,
            };
        }
    }
}
