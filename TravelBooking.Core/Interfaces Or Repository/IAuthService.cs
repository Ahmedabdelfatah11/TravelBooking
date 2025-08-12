
using System.IdentityModel.Tokens.Jwt;
using TravelBooking.Models;
using TravelBooking.Models.ResetPassword;

namespace TravelBooking.Core.Services
{
    public interface IAuthService
    { 
        Task<AuthModel> Register(RegisterModel model);
        Task<AuthModel> Login(LoginModel model);
        Task<AuthModel> ConfirmEmail(ConfirmedEmail model);
        Task<AuthModel> ResendConfirmEmail(ResendConfirmationEmail model);
        Task<string> AddRole(AddRole role);

        Task<AuthModel> ForgotPassword(ForgotPasswordModel model);
        Task<AuthModel> ResetPassword(ResetPasswordModel model);
        Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user);
    }
}
