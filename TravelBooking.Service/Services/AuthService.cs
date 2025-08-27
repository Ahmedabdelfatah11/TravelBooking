﻿
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.Design;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TravelBooking.Core.Repository.Contract;
using TravelBooking.Core.Services;
using TravelBooking.Core.Settings;
using TravelBooking.EmailBuilderbody;
using TravelBooking.Models;
using TravelBooking.Models.ResetPassword;
using TravelBooking.Repository.Data.Seeds;
using TravelBooking.Service;
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
        private readonly IConfiguration _config;
        private readonly IGenericRepository<Booking> _bookingRepo;
        private readonly IGenericRepository<Favoritet> _favoritesRepo;
        private readonly IGenericRepository<Review> _reviewRepo;
        private readonly IGenericRepository<HotelCompany> _hotelCompanyRepo;
        private readonly IGenericRepository<FlightCompany> _flightCompanyRepo;
        private readonly IGenericRepository<CarRentalCompany> _carRentalCompanyRepo;
        private readonly IGenericRepository<TourCompany> _tourCompanyRepo;

        public AuthService(UserManager<ApplicationUser> userManager, IMapper mapper,
             IOptions<JWT> jwt, RoleManager<IdentityRole> roleManager, ILogger<AuthService> logger
            , SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpAccessor,
             IEmailSender emailSender,
                IGenericRepository<HotelCompany> hotelCompanyRepo,
                IGenericRepository<FlightCompany> flightCompanyRepo,
                IGenericRepository<CarRentalCompany> carRentalCompanyRepo,
                IGenericRepository<TourCompany> tourCompanyRepo,
                IConfiguration configuration,
                IGenericRepository<Booking> bookingRepo,
                IGenericRepository<Favoritet> favoritesRepo,
                IGenericRepository<Review> reviewRepo

             )
        {
            _userManager = userManager;
            _mapper = mapper;
            _jwt = jwt.Value;
            _roleManager = roleManager;
            _logger = logger;
            _signInManager = signInManager;
            _httpAccessor = httpAccessor;
            _emailSender = emailSender;
            _config = configuration;
            _hotelCompanyRepo = hotelCompanyRepo;
            _flightCompanyRepo = flightCompanyRepo;
            _carRentalCompanyRepo = carRentalCompanyRepo;
            _tourCompanyRepo = tourCompanyRepo;
            _bookingRepo = bookingRepo;
            _favoritesRepo = favoritesRepo;
            _reviewRepo = reviewRepo;
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
                ExpireOn = token.ValidTo.ToLocalTime()
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
                // Assign default role to user
                await _userManager.AddToRoleAsync(user, Roles.User.ToString());

                // Generate email confirmation token
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
            var baseUrl = _config["Frontend:BaseUrl"];

            var request = _httpAccessor.HttpContext?.Request;
            var origin = $"{request?.Scheme}://{request?.Host}";// Get the origin URL from the request  //GET from frontend 
            var confirmUrl = $"{baseUrl}/confirm-email?userId={user.Id}&code={code}";

            var emailBody = EmailBodyBuilder.BuildEmailBody("EmailConfirmation",
                new Dictionary<string, string>
                {
                    //{ "{{name}}", user.FirstName },
                    //{ "{{action_url}}", $"{origin}/auth/emailConfirmation?userId={user.Id}&code={code}" },
                  
                    { "{{name}}", user.UserName! },
                    { "{{action_url}}", confirmUrl }
                });

            await _emailSender.SendEmailAsync(user.Email!, "Travel Booking: Email Confirmation", emailBody);
        }
        private async Task SendResetPasswordEmail(ApplicationUser user, string token)
        {
            var baseUrl = _config["Frontend:BaseUrl"];
            var resetUrl = $"{baseUrl}/reset?email={Uri.EscapeDataString(user.Email!)}&token={Uri.EscapeDataString(token)}";
            var emailBody = EmailBodyBuilder.BuildEmailBody("ResetPassword",
                new Dictionary<string, string>
                {
                    { "{{name}}", user.UserName! },
                    { "{{reset_url}}", resetUrl}
                });

            await _emailSender.SendEmailAsync(user.Email!, "Travel Booking: Reset Password", emailBody);
        }


        public async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {

            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("role", role));
                roleClaims.Add(new Claim(ClaimTypes.Role, role));
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
            .Union(roleClaims)
            .ToList();

            //  Add CompanyId Based on Role
            if (roles.Contains("HotelAdmin"))
            {
                var hotelCompany = await _hotelCompanyRepo.GetAllAsync(h => h.AdminId == user.Id);
                var hotelCompanyId = hotelCompany.FirstOrDefault()?.Id;
                if (hotelCompanyId.HasValue)
                    claims.Add(new Claim("HotelCompanyId", hotelCompanyId.Value.ToString()));
            }
            else if (roles.Contains("FlightAdmin"))
            {
                var flightCompany = await _flightCompanyRepo.GetAllAsync(f => f.AdminId == user.Id);
                var flightCompanyId = flightCompany.FirstOrDefault()?.Id;
                if (flightCompanyId.HasValue)
                    claims.Add(new Claim("FlightCompanyId", flightCompanyId.Value.ToString()));
            }
            else if (roles.Contains("CarRentalAdmin"))
            {
                var carCompany = await _carRentalCompanyRepo.GetAllAsync(c => c.AdminId == user.Id);
                var carCompanyId = carCompany.FirstOrDefault()?.Id;
                if (carCompanyId.HasValue)
                    claims.Add(new Claim("CarRentalCompanyId", carCompanyId.Value.ToString()));
            }
            else if (roles.Contains("TourAdmin"))
            {
                var tourCompany = await _tourCompanyRepo.GetAllAsync(t => t.AdminId == user.Id);
                var tourCompanyId = tourCompany.FirstOrDefault()?.Id;
                if (tourCompanyId.HasValue)
                    claims.Add(new Claim("TourCompanyId", tourCompanyId.Value.ToString()));
            }


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


        // Super Admin 
        public async Task<AuthModel> CreateUserByAdmin(RegisterModel model, string role)
        {
            if (await _userManager.FindByEmailAsync(model.Email) != null)
                return new AuthModel { Message = "Email already exists" };

            if (await _userManager.FindByNameAsync(model.UserName) != null)
                return new AuthModel { Message = "Username already exists" };

            var user = _mapper.Map<ApplicationUser>(model);
            user.EmailConfirmed = true;

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return new AuthModel
                {
                    Message = string.Join(", ", result.Errors.Select(e => e.Description)),
                    IsAuthenticated = false
                };
            }

            if (!await _roleManager.RoleExistsAsync(role))
                return new AuthModel { Message = "Role does not exist" };

            await _userManager.AddToRoleAsync(user, role);

            if (role == "HotelAdmin" && model.CompanyId.HasValue)
            {
                var hotelCompany = await _hotelCompanyRepo.GetAsync(model.CompanyId.Value);
                if (hotelCompany != null)
                {
                    hotelCompany.AdminId = user.Id;
                    await _hotelCompanyRepo.Update(hotelCompany);
                }
            }
            else if (role == "FlightAdmin" && model.CompanyId.HasValue)
            {
                var flightCompany = await _flightCompanyRepo.GetAsync(model.CompanyId.Value);
                if (flightCompany != null)
                {
                    flightCompany.AdminId = user.Id;
                    await _flightCompanyRepo.Update(flightCompany);
                }
            }
            else if (role == "CarRentalAdmin" && model.CompanyId.HasValue)
            {
                var carCompany = await _carRentalCompanyRepo.GetAsync(model.CompanyId.Value);
                if (carCompany != null)
                {
                    carCompany.AdminId = user.Id;
                    await _carRentalCompanyRepo.Update(carCompany);
                }
            }
            else if (role == "TourAdmin" && model.CompanyId.HasValue)
            {
                var tourCompany = await _tourCompanyRepo.GetAsync(model.CompanyId.Value);
                if (tourCompany != null)
                {
                    tourCompany.AdminId = user.Id;
                    await _tourCompanyRepo.Update(tourCompany);
                }
            }


            return new AuthModel
            {
                Message = $"User created successfully with role {role} and linked to company.",
                IsAuthenticated = false
            };
        }
        public async Task<List<UserListDto>> GetAllUsersAsync(int pageIndex, int pageSize)
        {
            // Step 1: Get user IDs (lightweight)
            var userIds = await _userManager.Users
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(u => u.Id)
                .ToListAsync();

            if (!userIds.Any()) return new List<UserListDto>();

            // Step 2: Fetch companies
            var hotels = await _hotelCompanyRepo.GetAllAsync(h => userIds.Contains(h.AdminId));
            var carCompanies = await _carRentalCompanyRepo.GetAllAsync(c => userIds.Contains(c.AdminId));
            var flightCompanies = await _flightCompanyRepo.GetAllAsync(f => userIds.Contains(f.AdminId));
            var tourCompanies = await _tourCompanyRepo.GetAllAsync(t => userIds.Contains(t.AdminId));

            var result = new List<UserListDto>();

            foreach (var userId in userIds)
            {
                // ✅ Critical: Reload user fresh
                var freshUser = await _userManager.FindByIdAsync(userId);
                if (freshUser == null) continue;

                // ✅ Get roles from fresh user
                var roles = await _userManager.GetRolesAsync(freshUser);

                var managedCompanies = new List<UserManagedCompanyDto>();

                // ✅ Hotel
                if (roles.Contains("HotelAdmin"))
                {
                    var hotel = hotels.FirstOrDefault(h => h.AdminId == userId);
                    if (hotel != null)
                    {
                        managedCompanies.Add(new UserManagedCompanyDto
                        {
                            Role = "HotelAdmin",
                            CompanyName = hotel.Name,
                            CompanyId = hotel.Id,
                            CompanyType = "Hotel"
                        });
                    }
                }

                // ✅ Flight
                if (roles.Contains("FlightAdmin"))
                {
                    var flight = flightCompanies.FirstOrDefault(f => f.AdminId == userId);
                    if (flight != null)
                    {
                        managedCompanies.Add(new UserManagedCompanyDto
                        {
                            Role = "FlightAdmin",
                            CompanyName = flight.Name,
                            CompanyId = flight.Id,
                            CompanyType = "Flight"
                        });
                    }
                }

                // ✅ Car Rental
                if (roles.Contains("CarRentalAdmin"))
                {
                    var car = carCompanies.FirstOrDefault(c => c.AdminId == userId);
                    if (car != null)
                    {
                        managedCompanies.Add(new UserManagedCompanyDto
                        {
                            Role = "CarRentalAdmin",
                            CompanyName = car.Name,
                            CompanyId = car.Id,
                            CompanyType = "Car Rental"
                        });
                    }
                }

                // ✅ Tour
                if (roles.Contains("TourAdmin"))
                {
                    var tour = tourCompanies.FirstOrDefault(t => t.AdminId == userId);
                    if (tour != null)
                    {
                        managedCompanies.Add(new UserManagedCompanyDto
                        {
                            Role = "TourAdmin",
                            CompanyName = tour.Name,
                            CompanyId = tour.Id,
                            CompanyType = "Tour"
                        });
                    }
                }

                result.Add(new UserListDto
                {
                    Id = freshUser.Id,
                    UserName = freshUser.UserName,
                    Email = freshUser.Email,
                    FirstName = freshUser.FirstName,
                    LastName = freshUser.LastName,
                    PhoneNumber = freshUser.PhoneNumber,
                    Address = freshUser.Address,
                    EmailConfirmed = freshUser.EmailConfirmed,
                    Roles = roles,
                    ManagedCompanies = managedCompanies
                });
            }

            return result;
        }
        public async Task<string> AssignRoleToUserAsync(string userId, int companyId, string companyType)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return "User not found";

            companyType = companyType.ToLower();

            // Assign Role to User in Identity
            var roleName = companyType switch
            {
                "hotel" => "HotelAdmin",
                "flight" => "FlightAdmin",
                "carrental" => "CarRentalAdmin",
                "tour" => "TourAdmin",
                _ => null
            };

            if (roleName == null) return "Invalid company type";

            if (!await _userManager.IsInRoleAsync(user, roleName))
            {
                var roleResult = await _userManager.AddToRoleAsync(user, roleName);
                if (!roleResult.Succeeded)
                    return "Failed to assign role to user";
            }

            // Link UserId as AdminId to the Company
            switch (companyType)
            {
                case "hotel":
                    var hotelCompany = await _hotelCompanyRepo.GetAsync(companyId);
                    if (hotelCompany == null) return "Hotel company not found";
                    hotelCompany.AdminId = userId;
                    await _hotelCompanyRepo.Update(hotelCompany);
                    break;

                case "flight":
                    var flightCompany = await _flightCompanyRepo.GetAsync(companyId);
                    if (flightCompany == null) return "Flight company not found";
                    flightCompany.AdminId = userId;
                    await _flightCompanyRepo.Update(flightCompany);
                    break;

                case "carrental":
                    var carCompany = await _carRentalCompanyRepo.GetAsync(companyId);
                    if (carCompany == null) return "Car rental company not found";
                    carCompany.AdminId = userId;
                    await _carRentalCompanyRepo.Update(carCompany);
                    break;

                case "tour":
                    var tourCompany = await _tourCompanyRepo.GetAsync(companyId);
                    if (tourCompany == null) return "Tour company not found";
                    tourCompany.AdminId = userId;
                    await _tourCompanyRepo.Update(tourCompany);
                    break;
            }

            return "Admin assigned to company and role successfully";
        }

        public async Task<string> RemoveRoleFromUserAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return "User not found";

            // 🔍 LOG 1: Roles BEFORE
            var rolesBefore = string.Join(", ", await _userManager.GetRolesAsync(user));
            Console.WriteLine($"🔧 BEFORE Remove '{role}': [{rolesBefore}]");

            var result = await _userManager.RemoveFromRoleAsync(user, role);

            // 🔍 LOG 2: Roles AFTER
            var rolesAfter = string.Join(", ", await _userManager.GetRolesAsync(user));
            Console.WriteLine($"🔧 AFTER Remove '{role}': [{rolesAfter}]");

            if (result.Succeeded)
            {
                await _userManager.UpdateAsync(user);
                return "Role removed successfully";
            }

            return string.Join(", ", result.Errors.Select(e => e.Description));
        }


        public async Task<string> DeleteUserAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return "User not found";

                _logger.LogInformation("Starting deletion process for user {UserId} ({Email})", userId, user.Email);

                // Step 1: Get user roles to determine what companies need to be unlinked
                var roles = await _userManager.GetRolesAsync(user);

                // Step 2: Remove AdminId from companies BEFORE deleting user
                foreach (var role in roles)
                {
                    switch (role)
                    {
                        case "HotelAdmin":
                            var hotelCompanies = await _hotelCompanyRepo.GetAllAsync(h => h.AdminId == userId);
                            foreach (var hotel in hotelCompanies)
                            {
                                hotel.AdminId = null; // Unlink the admin
                                await _hotelCompanyRepo.Update(hotel);
                                _logger.LogInformation("Unlinked user {UserId} from hotel company {CompanyId}", userId, hotel.Id);
                            }
                            break;

                        case "FlightAdmin":
                            var flightCompanies = await _flightCompanyRepo.GetAllAsync(f => f.AdminId == userId);
                            foreach (var flight in flightCompanies)
                            {
                                flight.AdminId = null; // Unlink the admin
                                await _flightCompanyRepo.Update(flight);
                                _logger.LogInformation("Unlinked user {UserId} from flight company {CompanyId}", userId, flight.Id);
                            }
                            break;

                        case "CarRentalAdmin":
                            var carCompanies = await _carRentalCompanyRepo.GetAllAsync(c => c.AdminId == userId);
                            foreach (var car in carCompanies)
                            {
                                car.AdminId = null; // Unlink the admin
                                await _carRentalCompanyRepo.Update(car);
                                _logger.LogInformation("Unlinked user {UserId} from car rental company {CompanyId}", userId, car.Id);
                            }
                            break;

                        case "TourAdmin":
                            var tourCompanies = await _tourCompanyRepo.GetAllAsync(t => t.AdminId == userId);
                            foreach (var tour in tourCompanies)
                            {
                                tour.AdminId = null; // Unlink the admin
                                await _tourCompanyRepo.Update(tour);
                                _logger.LogInformation("Unlinked user {UserId} from tour company {CompanyId}", userId, tour.Id);
                            }
                            break;
                    }
                }

                // Step 3: Handle related data cleanup
                // Note: You'll need to inject these repositories into your AuthService constructor

                // Option A: Delete related records (if you want to remove user's data)
                
                // Delete user's bookings
                var userBookings = await _bookingRepo.GetAllAsync(b => b.UserId == userId);
                foreach (var booking in userBookings)
                {
                    await _bookingRepo.Delete(booking);
                }

                // Delete user's favorites
                var userFavorites = await _favoritesRepo.GetAllAsync(f => f.UserId == userId);
                foreach (var favorite in userFavorites)
                {
                    await _favoritesRepo.Delete(favorite);
                }

                // Delete user's reviews
                var userReviews = await _reviewRepo.GetAllAsync(r => r.UserId == userId);
                foreach (var review in userReviews)
                {
                    await _reviewRepo.Delete(review);
                }
                

                // Option B: Set foreign keys to null (if you want to keep the data but unlink it)
                // This requires your database schema to allow NULL values for these foreign keys

                // You would need to execute raw SQL or use a context directly for this approach:
                /*
                await _context.Database.ExecuteSqlRawAsync(
                    "UPDATE Bookings SET UserId = NULL WHERE UserId = {0}", userId);
                await _context.Database.ExecuteSqlRawAsync(
                    "UPDATE Favoritets SET UserId = NULL WHERE UserId = {0}", userId);
                await _context.Database.ExecuteSqlRawAsync(
                    "UPDATE Reviews SET UserId = NULL WHERE UserId = {0}", userId);
                */

                // Step 4: Remove all roles from user
                if (roles.Count > 0)
                {
                    var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, roles);
                    if (!removeRolesResult.Succeeded)
                    {
                        _logger.LogError("Failed to remove roles for user {UserId}: {Errors}",
                            userId, string.Join(", ", removeRolesResult.Errors.Select(e => e.Description)));
                        return "Failed to remove user roles: " + string.Join(", ", removeRolesResult.Errors.Select(e => e.Description));
                    }
                    _logger.LogInformation("Removed all roles from user {UserId}", userId);
                }

                // Step 5: Remove external logins
                var logins = await _userManager.GetLoginsAsync(user);
                foreach (var login in logins)
                {
                    await _userManager.RemoveLoginAsync(user, login.LoginProvider, login.ProviderKey);
                }

                // Step 6: Remove user claims
                var claims = await _userManager.GetClaimsAsync(user);
                if (claims.Count > 0)
                {
                    await _userManager.RemoveClaimsAsync(user, claims);
                }

                // Step 7: Finally delete the user
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User {UserId} deleted successfully", userId);
                    return "User deleted successfully";
                }

                _logger.LogError("Failed to delete user {UserId}: {Errors}",
                    userId, string.Join(", ", result.Errors.Select(e => e.Description)));
                return "Failed to delete user: " + string.Join(", ", result.Errors.Select(e => e.Description));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while deleting user {UserId}: {Message}", userId, ex.Message);

                // Log inner exception details for debugging
                if (ex.InnerException != null)
                {
                    _logger.LogError("Inner exception: {InnerMessage}", ex.InnerException.Message);
                }

                return "An error occurred while deleting the user: " + ex.Message;
            }
        }

    }
}

