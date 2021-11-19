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
    [Route("[controller]")]
    public class HealthcareProviderController : ControllerBase
    {
        private readonly ILogger<HealthcareProviderController> _logger;

        public HealthcareProviderController(ILogger<HealthcareProviderController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<HealthcareProviderDto[]>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHealthcareProviders(
            [FromServices] IQueryHandler<GetHealthcareProvidersQuery, PagedResponse<HealthcareProviderDto[]>> queryHandler,
            [Range(1, 1000), FromQuery] int pageNumber = 1,
            [Range(1, int.MaxValue / 1000), FromQuery] int pageSize = 100)
        {
            var query = new GetHealthcareProvidersQuery { PageNumber = pageNumber, PageSize = pageSize };

            var providers = await queryHandler.HandleAsync(query);

            return Ok(providers);
        }

        [HttpGet("/{healthcareProviderId}")]
        [ProducesResponseType(typeof(HealthcareProviderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHealthcareProvider(
            [FromServices] IQueryHandler<GetProviderByIdQuery, HealthcareProviderDto> queryHandler,
            [FromRoute] Guid healthcareProviderId)
        {
            var provider = await queryHandler.HandleAsync(new GetProviderByIdQuery { Id = healthcareProviderId });

            if (provider == null)
            {
                return NotFound();
            }

            return Ok(provider);
        }

        [HttpGet("/{healthcareProviderId}/orderReports")]
        [ProducesResponseType(typeof(PagedResponse<OrderReportDto[]>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrderReports(
            [FromServices] IQueryHandler<GetOrderReportsQuery, PagedResponse<OrderReportDto[]>> queryHandler,
            [FromRoute] Guid healthcareProviderId,
            [Range(1, 1000), FromQuery] int pageNumber = 1,
            [Range(1, int.MaxValue / 1000), FromQuery] int pageSize = 100)
        {
            var query = new GetOrderReportsQuery
            {
                HealthcareProviderId = healthcareProviderId,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var orderReports = await queryHandler.HandleAsync(query);

            return Ok(orderReports);
        }

        [HttpGet("/{healthcareProviderId}/orderReports/{id}")]
        [ProducesResponseType(typeof(OrderReportDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrderReport(
           [FromServices] IQueryHandler<GetOrderReportByIdQuery, OrderReportDto> queryHandler,
           [FromRoute] Guid healthcareProviderId, Guid id)
        {
            var orderReport = await queryHandler.HandleAsync(new GetOrderReportByIdQuery { Id = id });

            if (orderReport == null)
            {
                return NotFound();
            }

            return Ok(orderReport);
        }

        [HttpPost("/{healthcareProviderId}/orderReports")]
        [ProducesResponseType(typeof(OrderReportDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddOrderReport(
            [FromServices] ICommandHandler<CreateOrderReportCommand, OrderReportDto> commandHandler,
            [FromRoute] Guid healthcareProviderId,
            [FromBody] CreateOrderReportCommand command)
        {
            var orderReport = await commandHandler.HandleAsync(command);

            if (orderReport == null)
            {
                return NotFound();
            }

            return CreatedAtAction(nameof(GetOrderReport), new { healthcareProviderId, id = orderReport.Id } ,orderReport);
        }

        [HttpGet("/{healthcareProviderId}/stockBalanceReports")]
        [ProducesResponseType(typeof(PagedResponse<StockBalanceReportDto[]>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetStockBalanceReports(
            [FromServices] IQueryHandler<GetStockBalanceReportsQuery, PagedResponse<StockBalanceReportDto[]>> queryHandler,
            [FromRoute] Guid healthcareProviderId,
            [Range(1, 1000), FromQuery] int pageNumber = 1,
            [Range(1, int.MaxValue / 1000), FromQuery] int pageSize = 100)
        {
            var query = new GetStockBalanceReportsQuery
            {
                HealthcareProviderId = healthcareProviderId,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var stockBalanceReports = await queryHandler.HandleAsync(query);

            return Ok(stockBalanceReports);
        }

        [HttpGet("/{healthcareProviderId}/stockBalanceReports/{id}")]
        [ProducesResponseType(typeof(StockBalanceReportDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetStockBalanceReport(
         [FromServices] IQueryHandler<GetStockBalanceReportByIdQuery, StockBalanceReportDto?> queryHandler,
         [FromRoute] Guid healthcareProviderId, Guid id)
        {
            var stockBalanceReport = await queryHandler.HandleAsync(new GetStockBalanceReportByIdQuery { Id = id });

            if (stockBalanceReport == null)
            {
                return NotFound();
            }

            return Ok(stockBalanceReport);
        }

        [HttpPost("/{healthcareProviderId}/stockBalanceReports")]
        [ProducesResponseType(typeof(StockBalanceReportDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddStockBalanceReport(
             [FromServices] ICommandHandler<CreateStockBalanceReportCommand, StockBalanceReportDto> commandHandler,
             [FromRoute] Guid healthcareProviderId,
             [FromBody] CreateStockBalanceReportCommand command)
        {
            var stockBalance = await commandHandler.HandleAsync(command);

            if (stockBalance == null)
            {
                return NotFound();
            }

            return CreatedAtAction(nameof(GetStockBalanceReport), new { healthcareProviderId, id = stockBalance.Id }, stockBalance);
        }


        [HttpGet("/{healthcareProviderId}/receiptReports")]
        [ProducesResponseType(typeof(PagedResponse<ReceiptReportDto[]>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetReceiptReports(
            [FromServices] IQueryHandler<GetReceiptReportsQuery, PagedResponse<ReceiptReportDto[]>> queryHandler,
            [FromRoute] Guid healthcareProviderId,
            [Range(1, 1000), FromQuery] int pageNumber = 1,
            [Range(1, int.MaxValue / 1000), FromQuery] int pageSize = 100)
        {
            var query = new GetReceiptReportsQuery
            {
                HealthcareProviderId = healthcareProviderId,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var recieptReports = await queryHandler.HandleAsync(query);

            return Ok(recieptReports);
        }

        [HttpGet("/{healthcareProviderId}/receiptReports/{id}")]
        [ProducesResponseType(typeof(ReceiptReportDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetReceiptReport(
         [FromServices] IQueryHandler<GetReceiptReportByIdQuery, ReceiptReportDto?> queryHandler,
         [FromRoute] Guid healthcareProviderId, Guid id)
        {
            var receiptReport = await queryHandler.HandleAsync(new GetReceiptReportByIdQuery { Id = id });

            if (receiptReport == null)
            {
                return NotFound();
            }

            return Ok(receiptReport);
        }

        [HttpPost("/{healthcareProviderId}/receiptReports")]
        [ProducesResponseType(typeof(ReceiptReportDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddReceiptReport(
            [FromServices] ICommandHandler<CreateReceiptReportCommand, ReceiptReportDto> commandHandler,
            [FromRoute] Guid healthcareProviderId,
            [FromBody] CreateReceiptReportCommand command)
        {
            var receiptReport = await commandHandler.HandleAsync(command);

            if (receiptReport == null)
            {
                return NotFound();
            }

            return CreatedAtAction(nameof(GetReceiptReport), new { healthcareProviderId, id = receiptReport.Id }, receiptReport);
        }

        [HttpGet("/{healthcareProviderId}/expenditureReports")]
        [ProducesResponseType(typeof(PagedResponse<ExpenditureReportDto[]>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetExpenditureReports(
            [FromServices] IQueryHandler<GetExpenditureReportsQuery, PagedResponse<ExpenditureReportDto[]>> queryHandler,
            [FromRoute] Guid healthcareProviderId,
            [Range(1, 1000), FromQuery] int pageNumber = 1,
            [Range(1, int.MaxValue / 1000), FromQuery] int pageSize = 100)
        {
            var query = new GetExpenditureReportsQuery
            {
                HealthcareProviderId = healthcareProviderId,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var expenditureReports = await queryHandler.HandleAsync(query);

            return Ok(expenditureReports);
        }

        [HttpGet("/{healthcareProviderId}/expenditureReports/{id}")]
        [ProducesResponseType(typeof(ExpenditureReportDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCapacityReport(
           [FromServices] IQueryHandler<GetExpenditureReportByIdQuery, ExpenditureReportDto?> queryHandler,
           [FromRoute] Guid healthcareProviderId, Guid id)
        {
            var expenditureReport = await queryHandler.HandleAsync(new GetExpenditureReportByIdQuery { Id = id });

            if (expenditureReport == null)
            {
                return NotFound();
            }

            return Ok(expenditureReport);
        }

        [HttpPost("/{healthcareProviderId}/expenditureReports")]
        [ProducesResponseType(typeof(PagedResponse<CapacityReportDto[]>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddExpenditureReport(
            [FromServices] ICommandHandler<CreateExpenditureReportCommand, ExpenditureReportDto> commandHandler,
            [FromRoute] Guid healthcareProviderId,
            [FromBody] CreateExpenditureReportCommand command)
        {
            var expenditureReport = await commandHandler.HandleAsync(command);

            if (expenditureReport == null)
            {
                return NotFound();
            }

            return CreatedAtAction(nameof(GetOrderReport), new { healthcareProviderId, id = expenditureReport.Id }, expenditureReport);
        }

        [HttpGet("/{healthcareProviderId}/capacityReports")]
        [ProducesResponseType(typeof(PagedResponse<CapacityReportDto[]>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCapacityReports(
            [FromServices] IQueryHandler<GetCapacityReportsQuery, PagedResponse<CapacityReportDto[]>> queryHandler,
            [FromRoute] Guid healthcareProviderId,
            [Range(1, 1000), FromQuery] int pageNumber = 1,
            [Range(1, int.MaxValue / 1000), FromQuery] int pageSize = 100)
        {
            var query = new GetCapacityReportsQuery
            {
                HealthcareProviderId = healthcareProviderId,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var capacityReports = await queryHandler.HandleAsync(query);

            return Ok(capacityReports);
        }

        [HttpGet("/{healthcareProviderId}/capacityReports/{id}")]
        [ProducesResponseType(typeof(CapacityReportDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCapacityReport(
           [FromServices] IQueryHandler<GetCapacityReportByIdQuery, CapacityReportDto?> queryHandler,
           [FromRoute] Guid healthcareProviderId, Guid id)
        {
            var capacityReport = await queryHandler.HandleAsync(new GetCapacityReportByIdQuery { Id = id });

            if (capacityReport == null)
            {
                return NotFound();
            }

            return Ok(capacityReport);
        }

        [HttpPost("/{healthcareProviderId}/capacityReports")]
        [ProducesResponseType(typeof(PagedResponse<CapacityReportDto[]>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddCapacityReport(
            [FromServices] ICommandHandler<CreateCapacityReportCommand, CapacityReportDto> commandHandler,
            [FromRoute] Guid healthcareProviderId,
            [FromBody] CreateCapacityReportCommand command)
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