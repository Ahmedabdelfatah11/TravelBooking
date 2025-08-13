using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
        [HttpPost("Hotels")]
        public async Task<ActionResult> Create([FromForm] HotelCompanyCreateDTO dto)
        {
            var hotel = _mapper.Map<HotelCompany>(dto);

            if (dto.Image == null || dto.Image.Length == 0)
                return BadRequest(new { Message = "Image file is required." });

            hotel.ImageUrl = await SaveImageAsync(dto.Image);

            await _hotelRepo.AddAsync(hotel);
            return Ok(_mapper.Map<HotelCompanyReadDTO>(hotel));
        }

        [HttpPut("Hotels/{id}")]
        public async Task<ActionResult> Update(int id, [FromForm] HotelCompanyUpdateDTO dto)
        {
            if (id != dto.Id)
                return BadRequest("ID mismatch");

            var hotel = await _hotelRepo.GetAsync(id);
            if (hotel == null) return NotFound();

            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole == "HotelAdmin" && !IsHotelAdminAuthorizedForHotel(id))
                return Forbid();

            _mapper.Map(dto, hotel);

            if (dto.Image != null && dto.Image.Length > 0)
            {
                hotel.ImageUrl = await SaveImageAsync(dto.Image);
            }

            await _hotelRepo.Update(hotel);
            return Ok(_mapper.Map<HotelCompanyReadDTO>(hotel));
        }


        [HttpDelete("Hotels/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var hotel = await _hotelRepo.GetAsync(id);
            if (hotel == null) return NotFound();

            if (!IsHotelAdminAuthorizedForHotel(id))
                return Forbid();

            _hotelRepo.Delete(hotel);
            return Ok();
        }

        private async Task<string> SaveImageAsync(IFormFile image)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "hotels");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return $"/images/hotels/{fileName}";
        }
        // Helper Method to check if HotelAdmin is authorized to access HotelCompany
        private bool IsHotelAdminAuthorizedForHotel(int hotelCompanyId)
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole == "HotelAdmin")
            {
                var hotelCompanyIdClaim = User.Claims.FirstOrDefault(c => c.Type == "HotelCompanyId")?.Value;
                if (hotelCompanyIdClaim == null || hotelCompanyIdClaim != hotelCompanyId.ToString())
                    return false;
            }
            return true;
        }

        // ==================== Flights ====================
        [HttpPost("flights")]
        public async Task<ActionResult> CreateFlight([FromForm] FlightCompanyCreateDto dto)
        {
            if (dto.Image == null || dto.Image.Length == 0)
                return BadRequest("Image file is required.");

            var flightCompany = _mapper.Map<FlightCompany>(dto);
            flightCompany.ImageUrl = await SaveFlightImageAsync(dto.Image);

            await _flightRepo.AddAsync(flightCompany);
            return Ok(_mapper.Map<FlightCompanyDTO>(flightCompany)); 
        }

         
        [HttpPut("flights/{id}")]
        public async Task<ActionResult> UpdateFlight(int id, [FromForm] FlightCompanyUpdateDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");

            var company = await _flightRepo.GetAsync(id);
            if (company == null) return NotFound();

            _mapper.Map(dto, company);

            if (dto.Image != null && dto.Image.Length > 0)
                company.ImageUrl = await SaveFlightImageAsync(dto.Image);

            await _flightRepo.Update(company);
            return Ok(_mapper.Map<FlightCompanyDTO>(company));
        }



        [HttpDelete("flights/{id}")]
        public async Task<ActionResult> DeleteFlight(int id)
        {
            var company = await _flightRepo.GetAsync(id);
            if (company == null) return NotFound();

            await _flightRepo.Delete(company);
            return NoContent();
        }

        private async Task<string> SaveFlightImageAsync(IFormFile image)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "flightsCompany");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return $"/images/flights/{fileName}";
        }


        // ==================== Car Rentals ====================
        [HttpPost("car-rentals")]
        public async Task<ActionResult> CreateCarRental([FromForm] SaveCarRentalDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.AdminId))
                return BadRequest("AdminId is required.");

            var entity = _mapper.Map<CarRentalCompany>(dto);

            if (dto.Image == null || dto.Image.Length == 0)
                return BadRequest("Image file is required.");

            entity.ImageUrl = await SaveCarRentalImageAsync(dto.Image);

            await _carRentalRepo.AddAsync(entity);
            return Ok(entity);
        }

        [HttpPut("car-rentals/{id}")]
        public async Task<ActionResult> UpdateCarRental(int id, [FromForm] SaveCarRentalDto dto)
        {
            var company = await _carRentalRepo.GetAsync(id);
            if (company == null) return NotFound();

            _mapper.Map(dto, company);

            if (dto.Image != null && dto.Image.Length > 0)
                company.ImageUrl = await SaveCarRentalImageAsync(dto.Image);

            await _carRentalRepo.Update(company);
            return Ok(company);
        }
        private async Task<string> SaveCarRentalImageAsync(IFormFile image)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "car-rentals");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return $"/images/car-rentals/{fileName}";
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
        public async Task<ActionResult> CreateTour([FromForm] TourCompanyCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.AdminId))
                return BadRequest("AdminId is required.");

            if (dto.Image == null || dto.Image.Length == 0)
                return BadRequest("Image file is required.");

            var entity = _mapper.Map<TourCompany>(dto);
            entity.ImageUrl = await SaveTourImageAsync(dto.Image);

            await _tourCompanyRepo.AddAsync(entity);
            return Ok(entity);
        }


        [HttpPut("tours/{id}")]
        public async Task<ActionResult> UpdateTour(int id, [FromForm] TourCompanyUpdateDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");

            var company = await _tourCompanyRepo.GetAsync(id);
            if (company == null) return NotFound();

            _mapper.Map(dto, company);

            if (dto.Image != null && dto.Image.Length > 0)
                company.ImageUrl = await SaveTourImageAsync(dto.Image);

            await _tourCompanyRepo.Update(company);
            return Ok(company);
        }


        [HttpDelete("tours/{id}")]
        public async Task<ActionResult> DeleteTour(int id)
        {
            var company = await _tourCompanyRepo.GetAsync(id);
            if (company == null) return NotFound();

            await _tourCompanyRepo.Delete(company);
            return NoContent();
        }
        private async Task<string> SaveTourImageAsync(IFormFile image)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "toursCompany");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return $"/images/tours/{fileName}";
        }

        //1-Get All user 
        ///api/SuperAdmin/users?pageIndex=1&pageSize=10
        [HttpGet("users")]
        public async Task<ActionResult> GetAllUsers([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var users = await _authService.GetAllUsersAsync(pageIndex, pageSize);
            return Ok(users);
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
        [HttpGet("dashboard")]
        public async Task<ActionResult> GetDashboard()
        {
            var stats = await _dashboardService.GetSuperAdminDashboardAsync();
            return Ok(stats);
        }
         
    }
}
