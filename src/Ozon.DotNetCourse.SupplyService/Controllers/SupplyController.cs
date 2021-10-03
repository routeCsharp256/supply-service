using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ozon.DotNetCourse.SupplyService.Domain.Entities;
using Ozon.DotNetCourse.SupplyService.Domain.Interfaces.Services;

namespace Ozon.DotNetCourse.SupplyService.Controllers
{
    [ApiController]
    [Route("api/supply")]
    public class SupplyController: ControllerBase
    {
        private readonly ISupplyService _supplyService;
        
        public SupplyController(ISupplyService supplyService)
        {
            _supplyService = supplyService;
        }

        [HttpGet("active")]
        public async Task<ActionResult> GetActiveSupplies(CancellationToken cancellationToken)
        {
            var supplies = await _supplyService.Get(SupplyState.Created, cancellationToken);
            return Ok(supplies);
        }
    }
}