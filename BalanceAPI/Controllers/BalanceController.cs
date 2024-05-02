using BalanceAPI.Services;
using BalanceAPI.Utilities;
using BalanceAPI.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace BalanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BalanceController : ControllerBase
    {
        private readonly IBalanceService _balanceService;
        private readonly ILogger<BalanceController> _logger;

        public BalanceController(IBalanceService balanceService, ILogger<BalanceController> logger)
        {
            _balanceService = balanceService;
            _logger = logger;
        }

        [ProducesResponseType(typeof(decimal), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpGet("{userId}", Name = "current-balance")]
        public async Task<ActionResult<decimal>> CurrentBalance(long userId)
        {
            try
            {
                var currentBalance = await _balanceService.GetUserCurrentBalance(userId);
                return Ok(currentBalance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving current balance.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPut(Name = "update-balance")]
        public async Task<ActionResult> UpdateBalance([FromBody] UpdateBalanceRequest request)
        {
            try
            {          
                if (!Request.Headers.TryGetValue("x-api-key", out var apiKey) || apiKey != Const.ApiKey)
                {
                    return Unauthorized("Invalid API key.");
                }

                var isSuccess = await _balanceService.UpdateBalance(request.UserId, request.Amount);
                if (isSuccess)
                    return Ok();
                else
                    return BadRequest("Failed to update balance.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating balance.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPost(Name = "topup-balance")]
        public async Task<ActionResult> TopUpBalance([FromBody] TopUpBalanceRequest request)
        { 
            var isSuccess = await _balanceService.TopUpBalance(request.UserId, request.Amount);
            if (isSuccess)            
                return Ok();            
            else            
                return BadRequest("Failed to update balance.");            
        }
    }
}
