using Microsoft.AspNetCore.Mvc;
using Psp.Api.Db;

namespace Psp.Api.Controllers
{
    [Route("v1")]
    [ApiController]
    public class BalanceController : ControllerBase
    {
        private readonly BalanceFacade _facade;

        public BalanceController(BalanceFacade facade)
        {
            _facade = facade;
        }

        [HttpGet]
        [Route("balances")]
        public async Task<IActionResult> GetBalanceAsync([FromHeader] string customerId)
        {
            var balance = await _facade.GetManyCacheDecorator(customerId);

            if (balance is null)
            {
                return BadRequest("Wrong customer id");
            }
            
            return Ok(balance);
        }
    }
}
