using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelBooking.APIs.DTOS.Admins;
using TravelBooking.APIs.DTOS.FlightCompany;
using TravelBooking.APIs.DTOS.HotelCompany;
using TravelBooking.APIs.DTOS.TourCompany;
using TravelBooking.Core.DTOS.CarRentalCompanies;
using TravelBooking.Core.Models;
using TravelBooking.Core.Repository.Contract;
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

        private readonly IGenericRepository<HotelCompany> _hotelRepo;
        private readonly IGenericRepository<FlightCompany> _flightRepo;
        private readonly IGenericRepository<CarRentalCompany> _carRentalRepo;
        private readonly IGenericRepository<TourCompany> _tourCompanyRepo;
        private readonly IMapper _mapper;

        public SuperAdminController(IAuthService authService, IDashboardService dashboardService,


            IGenericRepository<HotelCompany> hotelRepo,
            IGenericRepository<FlightCompany> flightRepo,
            IGenericRepository<CarRentalCompany> carRentalRepo,
            IGenericRepository<TourCompany> tourCompanyRepo,
            IMapper mapper


            )
        {
            _authService = authService;
            _dashboardService = dashboardService;

            _hotelRepo = hotelRepo;
            _flightRepo = flightRepo;
            _carRentalRepo = carRentalRepo;
            _tourCompanyRepo = tourCompanyRepo;
            _mapper = mapper;
        }






        // ==================== Hotels ====================
        [HttpPost("hotels")]
        public async Task<ActionResult> CreateHotel([FromBody] HotelCompanyCreateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.AdminId))
                return BadRequest("AdminId is required.");

            var entity = _mapper.Map<HotelCompany>(dto);
            await _hotelRepo.AddAsync(entity);
            return Ok(entity);
        }

        [HttpPut("hotels/{id}")]
        public async Task<ActionResult> UpdateHotel(int id, [FromBody] HotelCompanyUpdateDTO dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");

            var hotel = await _hotelRepo.GetAsync(id);
            if (hotel == null) return NotFound();

            _mapper.Map(dto, hotel);
            await _hotelRepo.Update(hotel);
            return NoContent();
        }

        [HttpDelete("hotels/{id}")]
        public async Task<ActionResult> DeleteHotel(int id)
        {
            var hotel = await _hotelRepo.GetAsync(id);
            if (hotel == null) return NotFound();

            await _hotelRepo.Delete(hotel);
            return NoContent();
        }

        // ==================== Flights ====================
        [HttpPost("flights")]
        public async Task<ActionResult> CreateFlight([FromBody] FlightCompanyDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.AdminId))
                return BadRequest("AdminId is required.");

            var entity = _mapper.Map<FlightCompany>(dto);
            await _flightRepo.AddAsync(entity);
            return Ok(entity);
        }

        [HttpPut("flights/{id}")]
        public async Task<ActionResult> UpdateFlight(int id, [FromBody] FlightCompanyDTO dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");

            var company = await _flightRepo.GetAsync(id);
            if (company == null) return NotFound();

            _mapper.Map(dto, company);
            await _flightRepo.Update(company);
            return NoContent();
        }

        [HttpDelete("flights/{id}")]
        public async Task<ActionResult> DeleteFlight(int id)
        {
            var company = await _flightRepo.GetAsync(id);
            if (company == null) return NotFound();

            await _flightRepo.Delete(company);
            return NoContent();
        }

        // ==================== Car Rentals ====================
        [HttpPost("car-rentals")]
        public async Task<ActionResult> CreateCarRental([FromBody] SaveCarRentalDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.AdminId))
                return BadRequest("AdminId is required.");

            var entity = _mapper.Map<CarRentalCompany>(dto);
            await _carRentalRepo.AddAsync(entity);
            return Ok(entity);
        }

        [HttpPut("car-rentals/{id}")]
        public async Task<ActionResult> UpdateCarRental(int id, [FromBody] SaveCarRentalDto dto)
        {
            var company = await _carRentalRepo.GetAsync(id);
            if (company == null) return NotFound();

            _mapper.Map(dto, company);
            await _carRentalRepo.Update(company);
            return NoContent();
        }

        [HttpDelete("car-rentals/{id}")]
        public async Task<ActionResult> DeleteCarRental(int id)
        {
            var company = await _carRentalRepo.GetAsync(id);
            if (company == null) return NotFound();

            await _carRentalRepo.Delete(company);
            return NoContent();
        }

        // ==================== Tours ====================
        [HttpPost("tours")]
        public async Task<ActionResult> CreateTour([FromBody] TourCompanyCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.AdminId))
                return BadRequest("AdminId is required.");

            var entity = _mapper.Map<TourCompany>(dto);
            await _tourCompanyRepo.AddAsync(entity);
            return Ok(entity);
        }

        [HttpPut("tours/{id}")]
        public async Task<ActionResult> UpdateTour(int id, [FromBody] TourCompanyUpdateDto dto)
        {
            var company = await _tourCompanyRepo.GetAsync(id);
            if (company == null) return NotFound();

            _mapper.Map(dto, company);
            await _tourCompanyRepo.Update(company);
            return NoContent();
        }

        [HttpDelete("tours/{id}")]
        public async Task<ActionResult> DeleteTour(int id)
        {
            var company = await _tourCompanyRepo.GetAsync(id);
            if (company == null) return NotFound();

            await _tourCompanyRepo.Delete(company);
            return NoContent();
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
