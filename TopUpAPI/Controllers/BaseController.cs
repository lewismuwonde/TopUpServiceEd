using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TopUpAPI.Utilities;

namespace TopUpAPI.Controllers
{
    [Consumes("application/json")]
    [Produces("application/json")]
    public class BaseController : Controller
    {
        public int GetUserId()
        {
            var userClaims = HttpContext.User.Claims.ToList();
            var userId = userClaims.Find(x => x.Type == "userId")?.Value;
            return int.Parse(userId);
        }
    }
}
