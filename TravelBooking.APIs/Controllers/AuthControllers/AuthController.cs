
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TravelBooking.APIs.DTOS.UserProfile;
using TravelBooking.Core.Models;
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
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;



        public AuthController(
        ILogger<AuthController> logger,
        IAuthService authService,
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration)
        {
            _logger = logger;
            _authService = authService;
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.Register(model);

            if (!string.IsNullOrEmpty(result.Message) && !result.IsAuthenticated)
             
            if (!result.IsAuthenticated)
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
               
            }
            return Unauthorized(new { message = "Invalid email or password" });

        }
        [HttpGet("ExternalLogin")]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Auth", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            _logger.LogInformation("Redirect URI: {RedirectUrl}", redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet("ExternalLoginCallback")]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
                return BadRequest($"Error from external provider: {remoteError}");

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return BadRequest("Error loading external login information.");

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
                return BadRequest("Email not provided by external provider.");
            var firstName = ExtractFirstName(info.Principal);
            var lastName = ExtractLastName(info.Principal);
            var dateOfBirth = ExtractDateOfBirth(info.Principal);
            // Check if user exists
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                // Create new user
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    FirstName = firstName,
                    LastName = lastName,
                    DateOfBirth = dateOfBirth
                };
                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                    return BadRequest(createResult.Errors);

                await _userManager.AddLoginAsync(user, info);
                await _userManager.AddToRoleAsync(user, Roles.User.ToString());
            }

            // Generate JWT using your AuthService pattern
            var jwtToken = await _authService.CreateJwtToken(user);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            var roles = await _userManager.GetRolesAsync(user);

            // Redirect to Angular callback route
            var frontendBaseUrl = _configuration["Frontend:BaseUrl"];

            var redirectUrl = $"{frontendBaseUrl}/google-callback" +
                              $"?token={tokenString}" +
                              $"&username={Uri.EscapeDataString(user.UserName)}" +
                              $"&roles={Uri.EscapeDataString(string.Join(",", roles))}";

            return Redirect(redirectUrl);
        }

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
        [HttpPost("ForgotPassword")]
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
        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("delete-user/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                _logger.LogInformation("DeleteUser called with userId: {UserId}", userId);

                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("DeleteUser called with null or empty userId");
                    return BadRequest("User ID is required");
                }

                // Check if user exists first
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning("User not found for userId: {UserId}", userId);
                    return NotFound(new { message = "User not found" });
                }

                _logger.LogInformation("Found user: {UserEmail}, attempting deletion", user.Email);

                var result = await _authService.DeleteUserAsync(userId);

                _logger.LogInformation("DeleteUserAsync result: {Result}", result);

                if (result == "User deleted successfully")
                {
                    _logger.LogInformation("User {UserId} deleted successfully by SuperAdmin {AdminId}",
                        userId, User.FindFirst("uid")?.Value);
                    return Ok(new { message = result });
                }

                if (result == "User not found")
                {
                    _logger.LogWarning("AuthService returned 'User not found' for userId: {UserId}", userId);
                    return NotFound(new { message = result });
                }

                _logger.LogError("DeleteUser failed with result: {Result}", result);
                return BadRequest(new { message = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception in DeleteUser for userId: {UserId}. Exception details: {ExceptionMessage}",
                    userId, ex.Message);
                return StatusCode(500, new
                {
                    message = "Internal server error",
                    details = ex.Message  // Remove this in production
                });
            }
        }



        // Helper methods to extract user information
        private string ExtractFirstName(ClaimsPrincipal principal)
        {
            // Try different claim types for first name
            var firstName = principal.FindFirstValue(ClaimTypes.GivenName) ??
                           principal.FindFirstValue("given_name") ??
                           principal.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname");

            // If no first name found, try to extract from full name
            if (string.IsNullOrEmpty(firstName))
            {
                var fullName = principal.FindFirstValue(ClaimTypes.Name) ??
                              principal.FindFirstValue("name");
                if (!string.IsNullOrEmpty(fullName))
                {
                    var nameParts = fullName.Split(' ');
                    firstName = nameParts.Length > 0 ? nameParts[0] : "";
                }
            }

            return firstName ?? "User";
        }

        private string ExtractLastName(ClaimsPrincipal principal)
        {
            // Try different claim types for last name
            var lastName = principal.FindFirstValue(ClaimTypes.Surname) ??
                          principal.FindFirstValue("family_name") ??
                          principal.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname");

            // If no last name found, try to extract from full name
            if (string.IsNullOrEmpty(lastName))
            {
                var fullName = principal.FindFirstValue(ClaimTypes.Name) ??
                              principal.FindFirstValue("name");
                if (!string.IsNullOrEmpty(fullName))
                {
                    var nameParts = fullName.Split(' ');
                    lastName = nameParts.Length > 1 ? string.Join(" ", nameParts.Skip(1)) : "";
                }
            }

            return lastName ?? "";
        }

        private DateTime? ExtractDateOfBirth(ClaimsPrincipal principal)
        {
            // Try different claim types for date of birth
            var dobString = principal.FindFirstValue(ClaimTypes.DateOfBirth) ??
                           principal.FindFirstValue("birthdate") ??
                           principal.FindFirstValue("birthday") ??
                           principal.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/dateofbirth");

            if (!string.IsNullOrEmpty(dobString))
            {
                // Try different date formats
                var dateFormats = new string[]
                {
            "yyyy-MM-dd",
            "MM/dd/yyyy",
            "dd/MM/yyyy",
            "yyyy/MM/dd",
            "MM-dd-yyyy",
            "dd-MM-yyyy"
                };

                foreach (var format in dateFormats)
                {
                    if (DateTime.TryParseExact(dobString, format, null, DateTimeStyles.None, out DateTime parsedDate))
                    {
                        return parsedDate;
                    }
                }

                // Try general parsing as fallback
                if (DateTime.TryParse(dobString, out DateTime generalParsedDate))
                {
                    return generalParsedDate;
                }
            }

            return null; // Return null if no date of birth found
        }

    }
}
