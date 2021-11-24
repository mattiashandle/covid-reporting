using Karolinska.Application.Dtos;
using Karolinska.Application.Features.Commands;
using Karolinska.Application.Features.Queries;
using Karolinska.Application.Wrappers;
using Karolinska.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Karolinska.Web.Controllers
{
    [ApiController]
    public class HealthcareProviderController : ControllerBase
    {
        private readonly ILogger<HealthcareProviderController> _logger;

        public HealthcareProviderController(ILogger<HealthcareProviderController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("healtcareProvider")]
        [ProducesResponseType(typeof(PagedResponse<HealthcareProviderDto[]>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHealthcareProviders(
            [FromServices] IQueryHandler<GetHealthcareProvidersQuery, PagedResponse<HealthcareProviderDto[]>> queryHandler,
            CancellationToken cancellationToken,
            [Range(1, 1000), FromQuery] int pageNumber = 1,
            [Range(1, int.MaxValue / 1000), FromQuery] int pageSize = 100)
        {
            var query = new GetHealthcareProvidersQuery { PageNumber = pageNumber, PageSize = pageSize };

            var providers = await queryHandler.HandleAsync(query, cancellationToken);

            return Ok(providers);
        }

        [HttpGet("healtcareProvider/{healthcareProviderId}")]
        [ProducesResponseType(typeof(HealthcareProviderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHealthcareProvider(
            [FromServices] IQueryHandler<GetHealthcareProviderByIdQuery, HealthcareProviderDto> queryHandler,
            [FromRoute] Guid healthcareProviderId,
            CancellationToken cancellationToken)
        {
            var provider = await queryHandler.HandleAsync(new GetHealthcareProviderByIdQuery { Id = healthcareProviderId }, cancellationToken);

            if (provider == null)
            {
                return NotFound();
            }

            return Ok(provider);
        }

        [HttpGet("healtcareProvider/{healthcareProviderId}/orderReports")]
        [ProducesResponseType(typeof(PagedResponse<OrderReportDto[]>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrderReports(
            [FromServices] IQueryHandler<GetOrderReportsQuery, PagedResponse<OrderReportDto[]>> queryHandler,
            [FromRoute] Guid healthcareProviderId,
            CancellationToken cancellationToken,
            [Range(1, 1000), FromQuery] int pageNumber = 1,
            [Range(1, int.MaxValue / 1000), FromQuery] int pageSize = 100)
        {
            var query = new GetOrderReportsQuery
            {
                HealthcareProviderId = healthcareProviderId,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var orderReports = await queryHandler.HandleAsync(query, cancellationToken);

            return Ok(orderReports);
        }

        [HttpGet("healtcareProvider/{healthcareProviderId}/orderReports/{id}")]
        [ProducesResponseType(typeof(OrderReportDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrderReport(
           [FromServices] IQueryHandler<GetOrderReportByIdQuery, OrderReportDto> queryHandler,
           [FromRoute] Guid healthcareProviderId, Guid id,
            CancellationToken cancellationToken)
        {
            var orderReport = await queryHandler.HandleAsync(new GetOrderReportByIdQuery { Id = id }, cancellationToken);

            if (orderReport == null)
            {
                return NotFound();
            }

            return Ok(orderReport);
        }

        [HttpPost("healtcareProvider/{healthcareProviderId}/orderReports")]
        [ProducesResponseType(typeof(OrderReportDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddOrderReport(
            [FromServices] ICommandHandler<CreateOrderReportCommand, OrderReportDto> commandHandler,
            [FromRoute] Guid healthcareProviderId,
            [FromBody] CreateOrderReportCommand command,
            CancellationToken cancellationToken)
        {
            var orderReport = await commandHandler.HandleAsync(command);

            if (orderReport == null)
            {
                return NotFound();
            }

            return CreatedAtAction(nameof(GetOrderReport), new { healthcareProviderId, id = orderReport.Id }, orderReport);
        }

        [HttpGet("healtcareProvider/{healthcareProviderId}/stockBalanceReports")]
        [ProducesResponseType(typeof(PagedResponse<StockBalanceReportDto[]>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetStockBalanceReports(
            [FromServices] IQueryHandler<GetStockBalanceReportsQuery, PagedResponse<StockBalanceReportDto[]>> queryHandler,
            [FromRoute] Guid healthcareProviderId,
            CancellationToken cancellationToken,
            [Range(1, 1000), FromQuery] int pageNumber = 1,
            [Range(1, int.MaxValue / 1000), FromQuery] int pageSize = 100)
        {
            var query = new GetStockBalanceReportsQuery
            {
                HealthcareProviderId = healthcareProviderId,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var stockBalanceReports = await queryHandler.HandleAsync(query, cancellationToken);

            return Ok(stockBalanceReports);
        }

        [HttpGet("healtcareProvider/{healthcareProviderId}/stockBalanceReports/{id}")]
        [ProducesResponseType(typeof(StockBalanceReportDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetStockBalanceReport(
         [FromServices] IQueryHandler<GetStockBalanceReportByIdQuery, StockBalanceReportDto?> queryHandler,
         [FromRoute] Guid healthcareProviderId, Guid id,
            CancellationToken cancellationToken)
        {
            var stockBalanceReport = await queryHandler.HandleAsync(new GetStockBalanceReportByIdQuery { Id = id }, cancellationToken);

            if (stockBalanceReport == null)
            {
                return NotFound();
            }

            return Ok(stockBalanceReport);
        }

        [HttpPost("healtcareProvider/{healthcareProviderId}/stockBalanceReports")]
        [ProducesResponseType(typeof(StockBalanceReportDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddStockBalanceReport(
             [FromServices] ICommandHandler<CreateStockBalanceReportCommand, StockBalanceReportDto> commandHandler,
             [FromRoute] Guid healthcareProviderId,
             [FromBody] CreateStockBalanceReportCommand command,
             CancellationToken cancellationToken)
        {
            var stockBalance = await commandHandler.HandleAsync(command);

            if (stockBalance == null)
            {
                return NotFound();
            }

            return CreatedAtAction(nameof(GetStockBalanceReport), new { healthcareProviderId, id = stockBalance.Id }, stockBalance);
        }


        [HttpGet("healtcareProvider/{healthcareProviderId}/receiptReports")]
        [ProducesResponseType(typeof(PagedResponse<ReceiptReportDto[]>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetReceiptReports(
            [FromServices] IQueryHandler<GetReceiptReportsQuery, PagedResponse<ReceiptReportDto[]>> queryHandler,
            [FromRoute] Guid healthcareProviderId,
            CancellationToken cancellationToken,
            [Range(1, 1000), FromQuery] int pageNumber = 1,
            [Range(1, int.MaxValue / 1000), FromQuery] int pageSize = 100)
        {
            var query = new GetReceiptReportsQuery
            {
                HealthcareProviderId = healthcareProviderId,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var recieptReports = await queryHandler.HandleAsync(query, cancellationToken);

            return Ok(recieptReports);
        }

        [HttpGet("healtcareProvider/{healthcareProviderId}/receiptReports/{id}")]
        [ProducesResponseType(typeof(ReceiptReportDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetReceiptReport(
         [FromServices] IQueryHandler<GetReceiptReportByIdQuery, ReceiptReportDto?> queryHandler,
         [FromRoute] Guid healthcareProviderId, Guid id,
            CancellationToken cancellationToken)
        {
            var receiptReport = await queryHandler.HandleAsync(new GetReceiptReportByIdQuery { Id = id }, cancellationToken);

            if (receiptReport == null)
            {
                return NotFound();
            }

            return Ok(receiptReport);
        }

        [HttpPost("healtcareProvider/{healthcareProviderId}/receiptReports")]
        [ProducesResponseType(typeof(ReceiptReportDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddReceiptReport(
            [FromServices] ICommandHandler<CreateReceiptReportCommand, ReceiptReportDto> commandHandler,
            [FromRoute] Guid healthcareProviderId,
            [FromBody] CreateReceiptReportCommand command,
            CancellationToken cancellationToken)
        {
            var receiptReport = await commandHandler.HandleAsync(command);

            if (receiptReport == null)
            {
                return NotFound();
            }

            return CreatedAtAction(nameof(GetReceiptReport), new { healthcareProviderId, id = receiptReport.Id }, receiptReport);
        }

        [HttpGet("healtcareProvider/{healthcareProviderId}/expenditureReports")]
        [ProducesResponseType(typeof(PagedResponse<ExpenditureReportDto[]>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetExpenditureReports(
            [FromServices] IQueryHandler<GetExpenditureReportsQuery, PagedResponse<ExpenditureReportDto[]>> queryHandler,
            [FromRoute] Guid healthcareProviderId,
            CancellationToken cancellationToken,
            [Range(1, 1000), FromQuery] int pageNumber = 1,
            [Range(1, int.MaxValue / 1000), FromQuery] int pageSize = 100)
        {
            var query = new GetExpenditureReportsQuery
            {
                HealthcareProviderId = healthcareProviderId,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var expenditureReports = await queryHandler.HandleAsync(query, cancellationToken);

            return Ok(expenditureReports);
        }

        [HttpGet("healtcareProvider/{healthcareProviderId}/expenditureReports/{id}")]
        [ProducesResponseType(typeof(ExpenditureReportDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetExpenditureReport(
           [FromServices] IQueryHandler<GetExpenditureReportByIdQuery, ExpenditureReportDto?> queryHandler,
           [FromRoute] Guid healthcareProviderId, Guid id,
            CancellationToken cancellationToken)
        {
            var expenditureReport = await queryHandler.HandleAsync(new GetExpenditureReportByIdQuery { Id = id }, cancellationToken);

            if (expenditureReport == null)
            {
                return NotFound();
            }

            return Ok(expenditureReport);
        }

        [HttpPost("healtcareProvider/{healthcareProviderId}/expenditureReports")]
        [ProducesResponseType(typeof(ExpenditureReportDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddExpenditureReport(
            [FromServices] ICommandHandler<CreateExpenditureReportCommand, ExpenditureReportDto> commandHandler,
            [FromRoute] Guid healthcareProviderId,
            [FromBody] CreateExpenditureReportCommand command,
            CancellationToken cancellationToken)
        {
            var expenditureReport = await commandHandler.HandleAsync(command);

            if (expenditureReport == null)
            {
                return NotFound();
            }

            return CreatedAtAction(nameof(GetOrderReport), new { healthcareProviderId, id = expenditureReport.Id }, expenditureReport);
        }

        [HttpGet("healtcareProvider/{healthcareProviderId}/capacityReports")]
        [ProducesResponseType(typeof(PagedResponse<CapacityReportDto[]>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCapacityReports(
            [FromServices] IQueryHandler<GetCapacityReportsQuery, PagedResponse<CapacityReportDto[]>> queryHandler,
            [FromRoute] Guid healthcareProviderId,
            CancellationToken cancellationToken,
            [Range(1, 1000), FromQuery] int pageNumber = 1,
            [Range(1, int.MaxValue / 1000), FromQuery] int pageSize = 100)
        {
            var query = new GetCapacityReportsQuery
            {
                HealthcareProviderId = healthcareProviderId,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var capacityReports = await queryHandler.HandleAsync(query, cancellationToken);

            return Ok(capacityReports);
        }

        [HttpGet("healtcareProvider/{healthcareProviderId}/capacityReports/{id}")]
        [ProducesResponseType(typeof(CapacityReportDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCapacityReport(
           [FromServices] IQueryHandler<GetCapacityReportByIdQuery, CapacityReportDto?> queryHandler,
           [FromRoute] Guid healthcareProviderId, Guid id,
            CancellationToken cancellationToken)
        {
            var capacityReport = await queryHandler.HandleAsync(new GetCapacityReportByIdQuery { Id = id }, cancellationToken);

            if (capacityReport == null)
            {
                return NotFound();
            }

            return Ok(capacityReport);
        }

        [HttpPost("healtcareProvider/{healthcareProviderId}/capacityReports")]
        [ProducesResponseType(typeof(CapacityReportDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddCapacityReport(
            [FromServices] ICommandHandler<CreateCapacityReportCommand, CapacityReportDto> commandHandler,
            [FromRoute] Guid healthcareProviderId,
            [FromBody] CreateCapacityReportCommand command,
            CancellationToken cancellationToken)
        {
            var capacityReport = await commandHandler.HandleAsync(command);

            if (capacityReport == null)
            {
                return NotFound();
            }

            return CreatedAtAction(nameof(GetCapacityReport), new { healthcareProviderId, id = capacityReport.Id }, capacityReport);
        }
    }
}