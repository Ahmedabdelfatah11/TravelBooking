using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TravelBooking.APIs.DTOS.UserProfile;
using TravelBooking.Core.Models.Services;
using TravelBooking.Core.Services;
using TravelBooking.Models;

namespace TravelBooking.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthService _authService;

        public UserProfileController(UserManager<ApplicationUser> userManager, IAuthService authService)
        {
            _userManager = userManager;
            _authService = authService;
        }

        [Authorize]
    [HttpGet("GetCurrentUser")]
    public async Task<ActionResult<UserProfileToReturnDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            return Ok(new UserProfileToReturnDto()
            {
                FirstName = user?.FirstName ?? string.Empty,
                LastName = user?.LastName ?? string.Empty,
                Email=user?.Email??string.Empty,
                Phone=user?.PhoneNumber??string.Empty,
                UserName=user?.UserName??string.Empty,
                Address = user.Address?? string.Empty,
                DateOfBirth = user.DateOfBirth ,
                Token = new JwtSecurityTokenHandler().WriteToken(await _authService.CreateJwtToken(user))
            });

        }

        [Authorize]
        [HttpPut("UpdateUserProfile")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UpdateUserProfileDto updatedProfile)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return NotFound("User not found.");

            if (!string.IsNullOrWhiteSpace(updatedProfile.FirstName))
                user.FirstName = updatedProfile.FirstName;

            if (!string.IsNullOrWhiteSpace(updatedProfile.LastName))
                user.LastName = updatedProfile.LastName;

            if (!string.IsNullOrWhiteSpace(updatedProfile.Phone))
                user.PhoneNumber = updatedProfile.Phone;

            if (!string.IsNullOrWhiteSpace(updatedProfile.Address))
                user.Address = updatedProfile.Address;

            if (updatedProfile.DateOfBirth.HasValue)
                user.DateOfBirth = updatedProfile.DateOfBirth.Value;

            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded ? Ok("Profile updated successfully.") : BadRequest(result.Errors);
        }
        [Authorize]
        [HttpDelete("DeleteUserProfile")]
        public async Task<IActionResult> DeleteUserProfile()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return NotFound("User not found.");

            var result = await _userManager.DeleteAsync(user);

            return result.Succeeded ? Ok("User deleted successfully.") : BadRequest(result.Errors);
        }

    }
}
