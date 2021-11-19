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

                var pfizer = _fixture.Create<Supplier>();

                context.Suppliers.Add(pfizer);

                context.SaveChanges();

                var fooProviderId = Guid.Parse("04487af1-74bb-4b4a-a9b8-780c013301e8");

                var fooHealthcareProvider = new HealthcareProvider
                {
                    Id = fooProviderId,
                    Name = "Vårdgivare Foo",
                    CapacityReports = Enumerable.Range(1, new Random().Next(10))
                        .Select(e => CreateCapacityReport(fooProviderId, astraZeneca.Id)).ToArray(),
                    OrderReports = Enumerable.Range(1, new Random().Next(10))
                        .Select(e => CreateOrderReport(fooProviderId)).ToArray(),
                    ReceiptReports = Enumerable.Range(1, new Random().Next(10))
                        .Select(e => CreateReceiptReport(fooProviderId, astraZeneca.Id)).ToArray(),
                    StockBalanceReports = Enumerable.Range(1, new Random().Next(10))
                        .Select(e => CreateStockBalanceReport(fooProviderId, astraZeneca.Id)).ToArray(),
                    ExpenditureReports = Enumerable.Range(1, new Random().Next(10))
                        .Select(e => CreateExpenditureReport(fooProviderId, astraZeneca.Id)).ToArray()
                };

                context.HealthcareProviders.Add(fooHealthcareProvider);

                context.SaveChanges();

                var barProviderId = Guid.Parse("c5d97ec1-d970-4f33-bd88-e66541533af1");

                var barHealthcareProvider = new HealthcareProvider
                {
                    Id = barProviderId,
                    Name = "Vårdgivare Bar",
                    CapacityReports = Enumerable.Range(1, new Random().Next(10))
                        .Select(e => CreateCapacityReport(barProviderId, pfizer.Id)).ToArray(),
                    OrderReports = Enumerable.Range(1, new Random().Next(10))
                        .Select(e => CreateOrderReport(barProviderId)).ToArray(),
                    ReceiptReports = Enumerable.Range(1, new Random().Next(10))
                        .Select(e => CreateReceiptReport(barProviderId, pfizer.Id)).ToArray(),
                    StockBalanceReports = Enumerable.Range(1, new Random().Next(10))
                        .Select(e => CreateStockBalanceReport(barProviderId, pfizer.Id)).ToArray(),
                    ExpenditureReports = Enumerable.Range(1, new Random().Next(10))
                        .Select(e => CreateExpenditureReport(barProviderId, pfizer.Id)).ToArray()
                };

                context.HealthcareProviders.Add(barHealthcareProvider);

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
                SupplierId = supplierId,
                InsertDate = _fixture.Create<DateTime>()
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
                HealthcareProviderId = providerId,
                InsertDate = _fixture.Create<DateTime>()
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
                HealthcareProviderId = providerId,
                InsertDate = _fixture.Create<DateTime>()
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
                SupplierId = supplierId,
                InsertDate = _fixture.Create<DateTime>()
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
                SupplierId = supplierId,
                InsertDate = _fixture.Create<DateTime>()
            };
        }
    }
}
