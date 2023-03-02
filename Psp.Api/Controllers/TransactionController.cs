using Microsoft.AspNetCore.Mvc;
using Psp.Api.Db;
using Psp.Api.Validators;
using Psp.Shared.Models.Requests;
using Psp.Shared.Services.Mq;

namespace Psp.Api.Controllers
{
    [Route("v1")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionFacade _facade;

        public TransactionController(RabbitMqConfiguration messageConfiguration, TransactionFacade facade)
        {
            _facade = facade;
        }

        [HttpPost]
        [Route("transactions")]
        public async Task<IActionResult> ProcessTransactionAsync(
            [FromBody] TransactionRequest request)
        {
            var notification = new ValidatorNotification();
            new TransactionRequestValidator(notification).Validate(request);

            if (!notification.IsValid)
            {
                return BadRequest(notification.Errors);
            }

            await _facade.PublishTransaction(request);
            return Accepted();
        }

        [HttpGet]
        [Route("transactions")]
        public async Task<IActionResult> GetTransactionsAsync(
            [FromHeader] string customerId,
            [FromQuery] int page = 1)
        {
            var transacitons = await _facade.GetManyCacheDecorator(customerId, page);
            
            if (transacitons.Count == 0)
            {
                return NoContent();
            }

            return Ok(transacitons);
        }
    }
}
