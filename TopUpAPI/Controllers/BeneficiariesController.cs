using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Xml.Linq;
using TopUpAPI.Services.Beneficiaries;
using TopUpAPI.ViewModel;

namespace TopUpAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeneficiariesController : BaseController
    {
        private readonly IBeneficiariesService _beneficiariesService;
        private readonly ILogger<BeneficiariesController> _logger;

        public BeneficiariesController(IBeneficiariesService beneficiariesService, ILogger<BeneficiariesController> logger)
        {
            _beneficiariesService = beneficiariesService;
            _logger = logger;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpGet(Name = "beneficiaries")]
        public async Task<ActionResult<List<BeneficiaryVM>>> GetActiveBeneficiaries()
        {
            try
            {
                var userId = GetUserId();
                var beneficiaries = await _beneficiariesService.GetActiveBeneficiaries(userId);
                return Ok(beneficiaries);
            }
            catch (Exception ex)
            {                
                _logger.LogError(ex, "An error occurred while retrieving active beneficiaries.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPost(Name = "beneficiaries")]
        public async Task<ActionResult> AddBeneficiary([FromBody] AddBeneficiaryModel request)
        { 
            try
            {
                var userId = GetUserId();
                var isValid = await _beneficiariesService.ValidateActiveBeneficiariesCount(userId);
                if (!isValid)
                {
                    return BadRequest("You have reached the maximum allowed beneficiaries.");
                }

                var beneficiary = await _beneficiariesService.AddBeneficiary(request, userId);
                return Ok(beneficiary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a beneficiary.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }
    }
}
