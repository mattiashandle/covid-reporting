using Karolinska.Application.Dtos;
using Karolinska.Application.Features.Queries;
using Karolinska.Application.Wrappers;
using Karolinska.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Karolinska.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SupplierController : ControllerBase
    {
        private readonly ILogger<SupplierController> _logger;

        public SupplierController(ILogger<SupplierController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<SupplierDto[]>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSuppliers(
        [FromServices] IQueryHandler<GetSuppliersQuery, PagedResponse<SupplierDto[]>> queryHandler,
        CancellationToken cancellationToken,
        [Range(1, 1000), FromQuery] int pageNumber = 1,
        [Range(1, int.MaxValue / 1000), FromQuery] int pageSize = 100)
        {
            var query = new GetSuppliersQuery { PageNumber = pageNumber, PageSize = pageSize };

            var suppliers = await queryHandler.HandleAsync(query, cancellationToken);

            return Ok(suppliers);
        }
    }
}
