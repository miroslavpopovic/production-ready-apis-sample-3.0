using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BoardGamesApi.Controllers
{
    // DEMO PURPOSE ONLY!
    public class DemoTokenController : Controller
    {
        private readonly IConfiguration _configuration;

        public DemoTokenController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // NOT FOR PRODUCTION USE!!!
        // you will need a robust auth implementation for production
        // i.e. try IdentityServer
        [AllowAnonymous]
        [Route("/get-token")]
        public IActionResult GenerateToken(string name = "aspnetcore-api-demo", bool admin = false)
        {
            var jwt = JwtTokenGenerator
                .Generate(name, admin, _configuration["Tokens:Issuer"], _configuration["Tokens:Key"]);

            return Ok(jwt);
        }
    }
}
