using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelBooking.APIs.DTOS.Admins;
using TravelBooking.Core.Services;
using TravelBooking.Models;
using TravelBooking.Service.Services.Dashboard;

namespace TravelBooking.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin")]
    public class SuperAdminController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IDashboardService _dashboardService;

        public SuperAdminController(IAuthService authService, IDashboardService dashboardService)
        {
            _authService = authService;
            _dashboardService = dashboardService;
        }

        //1-Get All user 
        ///api/SuperAdmin/users?pageIndex=1&pageSize=10
        [HttpGet("users")]
        public async Task<ActionResult> GetAllUsers([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var users = await _authService.GetAllUsersAsync(pageIndex, pageSize);
            return Ok(users);
        }


        ///api/SuperAdmin/assign-role
        ///Body:
        ///{
        //  "userId": "USER_ID_FROM_PREVIOUS_STEP",
        //  "role": "HotelAdmin" or FlightAdmin or  CarRentalAdmin,  TourAdmin
        //}

        [HttpPost("assign-role")]
        public async Task<ActionResult> AssignRole([FromBody] AssignAdminToCompanyDto dto)
        {
            var result = await _authService.AssignRoleToUserAsync(dto.UserId, dto.CompanyId, dto.CompanyType);

            if (result == "Admin assigned to company successfully")
                return Ok(new { message = result });

            return BadRequest(new { message = result });
        }

        [HttpPost("remove-role")]
        public async Task<ActionResult> RemoveRole([FromBody] AddRole dto)
        {
            var result = await _authService.RemoveRoleFromUserAsync(dto.UserId, dto.Role);

            if (result == "Role removed successfully")
                return Ok(new { message = result });

            return BadRequest(new { message = result });
        }



        [HttpDelete("delete-user/{userId}")]
        public async Task<ActionResult> DeleteUser(string userId)
        {
            var result = await _authService.DeleteUserAsync(userId);

            if (result == "User deleted successfully")
                return Ok(new { message = result });

            return BadRequest(new { message = result });
        }

        // /api/SuperAdmin/add-user?role=HotelAdmin
        //ADD ANY Admin
        [HttpPost("add-user")]
        public async Task<ActionResult> AddUser([FromBody] RegisterModel model, [FromQuery] string role)
        {
            var result = await _authService.CreateUserByAdmin(model, role);

            if (result.Message.Contains("successfully"))
                return Ok(new { message = result.Message });

            return BadRequest(new { message = result.Message });
        }




        [HttpGet("dashboard")]
        public async Task<ActionResult> GetDashboard()
        {
            var stats = await _dashboardService.GetSuperAdminDashboardAsync();
            return Ok(stats);
        }

    }
}
