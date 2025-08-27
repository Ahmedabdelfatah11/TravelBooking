﻿using Microsoft.AspNetCore.Authorization;
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
            if (string.IsNullOrEmpty(email))
                return Unauthorized("User not authenticated.");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound("User not found.");

            return Ok(new UserProfileToReturnDto()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email ?? string.Empty,
                Phone = user.PhoneNumber ?? string.Empty,
                UserName = user.UserName ?? string.Empty,
                Address = user.Address ?? string.Empty,
                DateOfBirth = user.DateOfBirth,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Token = new JwtSecurityTokenHandler().WriteToken(await _authService.CreateJwtToken(user))
            });
        }

        [Authorize]
        [HttpPut("UpdateUserProfile")]
        public async Task<IActionResult> UpdateUserProfile([FromForm] UpdateUserProfileDto updatedProfile)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
                return Unauthorized("User not authenticated.");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound("User not found.");

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

            if (updatedProfile.ProfilePicture != null && updatedProfile.ProfilePicture.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var ext = Path.GetExtension(updatedProfile.ProfilePicture.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(ext))
                    return BadRequest("Only .jpg, .jpeg, .png, and .webp files are allowed.");

                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "users");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                if (!string.IsNullOrEmpty(user.ProfilePictureUrl))
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", user.ProfilePictureUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                        System.IO.File.Delete(oldFilePath);
                }

                var fileName = $"{Guid.NewGuid()}{ext}";
                var filePath = Path.Combine(folderPath, fileName);
                var dbPath = $"/images/users/{fileName}";

                await using var stream = new FileStream(filePath, FileMode.Create);
                await updatedProfile.ProfilePicture.CopyToAsync(stream);

                user.ProfilePictureUrl = dbPath;
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new
            {
                message = "Profile updated successfully.",
                user = new
                {
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    user.PhoneNumber,
                    user.Address,
                    user.DateOfBirth,
                    user.ProfilePictureUrl
                }
            });
        }

        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            // Log the incoming request for debugging
            Console.WriteLine($"Received ChangePassword request: CurrentPassword={(dto?.CurrentPassword?.Length ?? 0)} chars, NewPassword={(dto?.NewPassword?.Length ?? 0)} chars");

            // Validate the DTO
            if (dto == null)
            {
                Console.WriteLine("DTO is null");
                return BadRequest("Invalid request data.");
            }

            if (string.IsNullOrEmpty(dto.CurrentPassword))
            {
                Console.WriteLine("CurrentPassword is null or empty");
                return BadRequest("Current password is required.");
            }

            if (string.IsNullOrEmpty(dto.NewPassword))
            {
                Console.WriteLine("NewPassword is null or empty");
                return BadRequest("New password is required.");
            }

            if (dto.NewPassword.Length < 6)
            {
                Console.WriteLine($"NewPassword too short: {dto.NewPassword.Length} characters");
                return BadRequest("New password must be at least 6 characters long.");
            }

            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
            {
                Console.WriteLine("User email claim not found");
                return Unauthorized("User not authenticated.");
            }

            Console.WriteLine($"Found user email: {email}");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                Console.WriteLine($"User not found for email: {email}");
                return NotFound("User not found.");
            }

            Console.WriteLine($"Found user: {user.UserName}");

            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (!result.Succeeded)
            {
                Console.WriteLine($"Password change failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                return BadRequest(new
                {
                    message = "Password change failed.",
                    errors = result.Errors.Select(e => e.Description).ToArray()
                });
            }

            Console.WriteLine("Password changed successfully");
            return Ok(new { message = "Password changed successfully." });
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("DeleteUserProfile")]
        public async Task<IActionResult> DeleteUserProfile()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
                return Unauthorized("User not authenticated.");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound("User not found.");

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded ? Ok("User deleted successfully.") : BadRequest(result.Errors);
        }
    }
}