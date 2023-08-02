
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace PaymentGatewayAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]    
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public  TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet("GetAllTransaction")]
        public async Task<ActionResult<ServiceResponse<List<AllTransactionDto>>>> GetAllTransaction()
        {
            var response = await _transactionService.GetAllTransaction();

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("Pay")]
        public async Task<ActionResult<ServiceResponse<TransactionDto>>> Pay(PayTransactionDto Transaction)
        {
            var response = await _transactionService.Pay(Transaction);
            if(!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
