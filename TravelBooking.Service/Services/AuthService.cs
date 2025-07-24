using AutoMapper;
using TravelBooking.Models.ResetPassword;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TravelBooking.Core.Services;
using TravelBooking.Models;
using TravelBooking.Core.Settings;
using TravelBooking.EmailBuilderbody;
using Microsoft.AspNetCore.Mvc;
using static Org.BouncyCastle.Math.EC.ECCurve;



namespace TravelBooking.Core.Models.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AuthService> _logger;
        private readonly IMapper _mapper;
        private readonly JWT _jwt;
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly IEmailSender _emailSender;


        public AuthService(UserManager<ApplicationUser> userManager, IMapper mapper, 
             IOptions<JWT> jwt, RoleManager<IdentityRole> roleManager, ILogger<AuthService> logger
            , SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpAccessor,
             IEmailSender emailSender)
        {
            _userManager = userManager;
            _mapper = mapper;
            _jwt = jwt.Value;
            _roleManager = roleManager;
            _logger = logger;
            _signInManager = signInManager;
            _httpAccessor = httpAccessor;
            _emailSender = emailSender;
        }

        public async Task<string> AddRole(AddRole role)
        {
            var user = await _userManager.FindByIdAsync(role.UserId);
            if (user is null)
            {
                return "User not found";
            }
            if (!await _roleManager.RoleExistsAsync(role.Role))
            {
                return "Role does not exist";
            }
            var result = await _userManager.AddToRoleAsync(user, role.Role);
            if (!result.Succeeded)
            {
                return string.Join(", ", result.Errors.Select(e => e.Description));
            }
            return "Role added successfully";
        }

        public async Task<AuthModel> ConfirmEmail(ConfirmedEmail model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null || string.IsNullOrEmpty(model.Code))
            {
                return new AuthModel { Message = "Invalid confirmation code or user not found" };
            }
            var emailExists = await _userManager.Users.AnyAsync(u => u.Email == user.Email && u.Id != user.Id);
            if (emailExists)
            {
                return new AuthModel { Message = "Email already exists" };
            }
            var decodedCode = "";
            try
            {
                decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Code));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error confirming email for user {UserId}", user.Id);
                return new AuthModel { Message = "An error occurred while confirming email" };
            }
            var result = await _userManager.ConfirmEmailAsync(user, decodedCode);
            if (result.Succeeded)
            {
                return new AuthModel { Message = "Email confirmed successfully", IsAuthenticated = true };
            }
            else
            {
                return new AuthModel { Message = string.Join(", ", result.Errors.Select(e => e.Description)) };
            }
        }

        [HttpPost("Login")]
        public async Task<AuthModel> Login(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
            {
                return new AuthModel { Message = "Invalid email or password" };
            }

            if (!user.EmailConfirmed)
            {
                return new AuthModel { Message = "Email is not confirmed yet" };
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);
            if (!result.Succeeded)
            {
                return new AuthModel { Message = "Invalid email or password" };
            }

            var token = await CreateJwtToken(user);
            var roles = await _userManager.GetRolesAsync(user);

            return new AuthModel
            {
                Message = "Login successful",
                IsAuthenticated = true,
                Username = user.UserName,
                Email = user.Email,
                Roles = roles.ToList(),
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpireOn = token.ValidTo
            };
        }
        public async Task<AuthModel> Register(RegisterModel model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
            {
                return new AuthModel { Message = "Email already exists" };
            }
            else if (await _userManager.FindByNameAsync(model.UserName) is not null)
            {
                return new AuthModel { Message = "Username already exists" };
            }

            var user = _mapper.Map<ApplicationUser>(model);

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                _logger.LogInformation($"User {user.UserName} registered with email confirmation code: {code}");

                //TODO: Implement email sending logic here
                await SendConfirmationEmail(user, code);

                return new AuthModel
                {
                    Message = "User registered successfully. Please confirm your email.",
                    IsAuthenticated = false
                };
            }


            return new AuthModel
            {
                Message = string.Join(", ", result.Errors.Select(e => e.Description)),
                IsAuthenticated = false
            };
        }

        public async Task<AuthModel> ResendConfirmEmail(ResendConfirmationEmail model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) is not ApplicationUser user)
            {
                return new AuthModel { Message = "User not found" };
            }
            if (user.EmailConfirmed)
            {
                return new AuthModel { Message = "Email is already confirmed" };
            }
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            _logger.LogInformation($"Resending confirmation email for user {user.UserName} with code: {code}");

            //TODO: Implement email sending logic here

            await SendConfirmationEmail(user, code);

            return new AuthModel
            {
                Message = "Confirmation email resent successfully. Please check your inbox.",
                IsAuthenticated = false
            };
        }

        public async Task<AuthModel> ForgotPassword(ForgotPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new AuthModel { Message = "User not found" };
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            _logger.LogInformation($"Password reset token generated for user {user.UserName}: {token}");

            await SendResetPasswordEmail(user, token);  

            return new AuthModel
            {
                Message = "Password reset link has been sent to your email.",
                IsAuthenticated = false
            };
        }


        public async Task<AuthModel> ResetPassword(ResetPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new AuthModel { Message = "User not found" };
            }

            var decodedToken = "";
            try
            {
                decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error decoding password reset token for user {UserId}", user.Id);
                return new AuthModel { Message = "Invalid or corrupted token" };
            }

            var result = await _userManager.ResetPasswordAsync(user, decodedToken, model.NewPassword);
            if (!result.Succeeded)
            {
                return new AuthModel { Message = string.Join(", ", result.Errors.Select(e => e.Description)) };
            }

            return new AuthModel
            {
                Message = "Password has been reset successfully.",
                IsAuthenticated = true
            };
        } 
        private async Task SendConfirmationEmail(ApplicationUser user, string code)
        {
            var request = _httpAccessor.HttpContext?.Request;
            var origin = $"{request?.Scheme}://{request?.Host}";// Get the origin URL from the request  //GET from frontend 

            var emailBody = EmailBodyBuilder.BuildEmailBody("EmailConfirmation",
                new Dictionary<string, string>
                {
                    { "{{name}}", user.FirstName },
                    { "{{action_url}}", $"{origin}/auth/emailConfirmation?userId={user.Id}&code={code}" }
                  
                });

            await _emailSender.SendEmailAsync(user.Email!, "Travel Booking: Email Confirmation", emailBody);
        }
        private async Task SendResetPasswordEmail(ApplicationUser user, string token)
        {
            var request = _httpAccessor.HttpContext?.Request;
            var origin = $"{request?.Scheme}://{request?.Host}"; // Get the origin URL from the request

            var emailBody = EmailBodyBuilder.BuildEmailBody("ResetPassword",
                new Dictionary<string, string>
                {
                    { "{{name}}", user.FirstName },
                    { "{{reset_url}}", $"{origin}/auth/resetPassword?email={user.Email}&token={token}" }
                });

            await _emailSender.SendEmailAsync(user.Email!, "Travel Booking: Reset Password", emailBody);
        }


        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("role", role));
            }
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim("uid", user.Id),
                new Claim(ClaimTypes.NameIdentifier, user.Id), 
                new Claim(ClaimTypes.Email, user.Email),      
            }
            .Union(userClaims)
            .Union(roleClaims);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(_jwt.DurationInDays),
                signingCredentials: creds
            );
            return token;
        }
    }
}
