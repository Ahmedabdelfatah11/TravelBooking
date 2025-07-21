
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelBooking.Core.Services;
using TravelBooking.Models;
using TravelBooking.Models.ResetPassword;

namespace Jwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;
        public AuthController(ILogger<AuthController> logger, IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.Register(model);

            if (string.IsNullOrEmpty(result.Message) && !result.IsAuthenticated)
            {
                return Ok(new
                { 
                    message = result.Message
                });
            }
            return BadRequest(new
            {
                message = result.Message
            });

        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.Login(model);
            if (result.IsAuthenticated)
            {
                return Ok(result);
                //return Ok(new
                //{
                //    token = result.Token,
                //    message = result.Message,
                //    expireOn = result.ExpireOn,
                //    username = result.Username,
                //    email = result.Email,
                //    roles = result.Roles
                //});
            }
            return Unauthorized(result.Message);
        }
        [Authorize (Roles = "Admin")]
        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRole(AddRole role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.AddRole(role);
            if (result == "Role added successfully")
            {
                return Ok(result);
            }
            return BadRequest(result);

        }
        [HttpPost("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmedEmail model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.ConfirmEmail(model);
            if (result.IsAuthenticated)
            {
                return Ok(new
                {
                    message = result.Message,
                    token = result.Token,
                    expireOn = result.ExpireOn
                });
            }
            return BadRequest(new { message = result.Message });
        }

        [HttpPost("ResendConfirmEmail")]
        public async Task<IActionResult> ResendConfirmEmail([FromBody] ResendConfirmationEmail model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.ResendConfirmEmail(model);
            if (result.IsAuthenticated)
            {
                return Ok(new
                {
                    message = result.Message,
                    token = result.Token,
                    expireOn = result.ExpireOn
                });
            }
            return BadRequest(new { message = result.Message });
        }
        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.ForgotPassword(model);
            return Ok(new { message = result.Message });
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.ResetPassword(model);
            if (result.IsAuthenticated)
            {
                return Ok(new
                {
                    message = result.Message
                });
            }
            return BadRequest(new { message = result.Message });
        }

    }
}
