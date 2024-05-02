using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Xml.Linq;
using TopUpAPI.Services.TopUp;
using TopUpAPI.Services.Users;
using TopUpAPI.Utilities;
using TopUpAPI.ViewModel;

namespace TopUpAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopUpController : BaseController
    {
        private readonly ITopUpService _topUpService;
        private readonly IUserService _userService;
        private readonly ILogger<TopUpController> _logger;

        public TopUpController(ITopUpService topUpService, IUserService userService, ILogger<TopUpController> logger)
        {
            _topUpService = topUpService;
            _userService = userService;
            _logger = logger;
        }

        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpGet(Name = "options")]
        public ActionResult<List<int>> GetTopUpOptions()
        {
            try
            {
                var topUpOptions = _topUpService.GetTopUpOptions();
                return Ok(topUpOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving top-up options.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPost(Name = "topup")]
        public async Task<ActionResult> TopUpBeneficiary([FromBody] TopUpRequest request)
        {

            try
            {
                var userId = GetUserId();
                var isSuccess = await _topUpService.TopUpBeneficiary(request, userId);

                if (isSuccess)
                    return Ok("Top-up successful");
                else
                    return BadRequest("Failed to top up beneficiary");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while topping up beneficiary.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }

        }

    }
}
