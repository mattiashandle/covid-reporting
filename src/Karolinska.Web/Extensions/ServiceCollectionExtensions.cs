using Karolinska.Application.Dtos;
using Karolinska.Application.Features.Commands;
using Karolinska.Application.Features.Queries;
using Karolinska.Application.Wrappers;
using Karolinska.Core.Interfaces;

namespace Karolinska.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterQueries(this IServiceCollection services)
        {
            services.AddTransient<IQueryHandler<GetSupplierByIdQuery, SupplierDto>, GetSupplierByIdQueryHandler>();

            services.AddTransient<IQueryHandler<GetSuppliersQuery, PagedResponse<SupplierDto[]>>, GetSuppliersQueryHandler>();

            services.AddTransient<IQueryHandler<GetHealthcareProvidersQuery, PagedResponse<HealthcareProviderDto[]>>, GetHealthcareProvidersQueryHandler>();

            services.AddTransient<IQueryHandler<GetHealthcareProviderByIdQuery, HealthcareProviderDto>, GetHealthcareProviderByIdQueryHandler>();

            services.AddTransient<IQueryHandler<GetOrderReportsQuery, PagedResponse<OrderReportDto[]>>, GetOrderReportsQueryHandler>();

            services.AddTransient<IQueryHandler<GetCapacityReportsQuery, PagedResponse<CapacityReportDto[]>>, GetCapacityReportsQueryHandler>();

            services.AddTransient<IQueryHandler<GetStockBalanceReportsQuery, PagedResponse<StockBalanceReportDto[]>>, GetStockBalanceReportsQueryHandler>();

            services.AddTransient<IQueryHandler<GetStockBalanceReportByIdQuery, StockBalanceReportDto?>, GetStockBalanceReportByIdQueryHandler>();

            services.AddTransient<IQueryHandler<GetExpenditureReportsQuery, PagedResponse<ExpenditureReportDto[]>>, GetExpenditureReportsQueryHandler>();

            services.AddTransient<IQueryHandler<GetExpenditureReportByIdQuery, ExpenditureReportDto?>, GetExpenditureReportByIdQueryHandler>();

            services.AddTransient<IQueryHandler<GetReceiptReportsQuery, PagedResponse<ReceiptReportDto[]>>, GetReceiptReportsQueryHandler>();

            services.AddTransient<IQueryHandler<GetOrderReportByIdQuery, OrderReportDto>, GetOrderReportByIdQueryHandler>();

            services.AddTransient<IQueryHandler<GetCapacityReportByIdQuery, CapacityReportDto?>, GetCapacityReportByIdQueryHandler>();



            return services;
        }

        public static IServiceCollection RegisterCommands(this IServiceCollection services)
        {
            services.AddTransient<ICommandHandler<CreateOrderReportCommand, OrderReportDto?>, CreateOrderReportCommandHandler>();

            services.AddTransient<ICommandHandler<CreateCapacityReportCommand, CapacityReportDto?>, CreateCapacityReportCommandHandler>();

            services.AddTransient<ICommandHandler<CreateExpenditureReportCommand, ExpenditureReportDto?>, CreateExpenditureReportCommandHandler>();

            services.AddTransient<ICommandHandler<CreateStockBalanceReportCommand, StockBalanceReportDto?>, CreateStockBalanceReportCommandHandler>();

            services.AddTransient<ICommandHandler<CreateReceiptReportCommand, ReceiptReportDto?>, CreateReceiptReportCommandHandler>();

            services.AddTransient<ICommandHandler<UpdateCapacityReportCommand, CapacityReportDto>, UpdateCapacityReportCommandHandler>();

            return services;

        }
    }
}
