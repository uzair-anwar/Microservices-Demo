using Microsoft.AspNetCore.Mvc;
using Users.Services;
using Users.Data.Models;

namespace Users.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IOktaService _oktaService;

        public UsersController(IOktaService oktaService)
        {
            _oktaService = oktaService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var oktaToken = await _oktaService.GetTokenAsync(request.Username, request.Password);
                return Ok(oktaToken);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetUserAsync()
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var checkAuthentication = await _oktaService.AuthenticateToken(token);

                if (checkAuthentication)
                {
                    var response = await _oktaService.GetUserAsync(token);
                    return Ok(response);
                }
                else { return Unauthorized(); }
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
