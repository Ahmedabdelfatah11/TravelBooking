
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

        
        //Super Admin
        Task<AuthModel> CreateUserByAdmin(RegisterModel model, string role);
        Task<List<object>> GetAllUsersAsync(int pageIndex, int pageSize);
        Task<string> AssignRoleToUserAsync(string userId, int companyId, string companyType);
        Task<string> RemoveRoleFromUserAsync(string userId, string role);
        Task<string> DeleteUserAsync(string userId);


        //  Task<object> GetDashboardStatsAsync();
    }
}
