using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TopUpAPI.Services.Authentication;
using TopUpAPI.ViewModel;
using LoginRequest = TopUpAPI.ViewModel.LoginRequest;

namespace TopUpAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPost(Name = "login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var user = await _authService.AuthenticateUser(request);
                if (user == null)
                    return Unauthorized("Invalid username or password.");                

                var tokenString =  _authService.GenerateJwtToken(user.Id);
                return Ok(tokenString);          
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving active beneficiaries.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }
    }
}
