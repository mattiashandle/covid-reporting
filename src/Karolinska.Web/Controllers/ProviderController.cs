using Karolinska.Application.Dtos;
using Karolinska.Application.Features.Queries;
using Karolinska.Application.Wrappers;
using Karolinska.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Karolinska.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProviderController : ControllerBase
    {
        private readonly ILogger<ProviderController> _logger;

        public ProviderController(ILogger<ProviderController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<HealthcareProviderDto[]>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(
            [FromServices] IQueryHandler<GetProvidersQuery, PagedResponse<HealthcareProviderDto[]>> queryHandler,
            [FromQuery] GetProvidersQuery query)
        {
            var suppliers = await queryHandler.HandleAsync(query);

            return Ok(suppliers);
        }

        //[HttpGet("/{id}")]
        //[ProducesResponseType(typeof(SupplierDto), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> Get(
        //    [FromServices] IQueryHandler<GetSupplierByIdQuery, SupplierDto> queryHandler,
        //    [FromRoute] Guid id)
        //{
        //    var patient = await queryHandler.HandleAsync(new GetSupplierByIdQuery { Id = id });
            
        //    if (patient == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(patient);

        //}
    }
}