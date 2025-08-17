using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc; 
using System.Security.Claims;

using TravelBooking.APIs.DTOS.HotelCompany; 
using TravelBooking.Core.Models;
using TravelBooking.Core.Repository.Contract;
using TravelBooking.Core.Specifications.HotelCompanySpecs;
using TravelBooking.Helper;
using TravelBooking.Service.Services.Dashboard;

namespace TravelBooking.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,HotelAdmin")]
    public class HotelCompanyController : ControllerBase
    {
        private readonly IGenericRepository<HotelCompany> _hotelRepo;
        private readonly IMapper _mapper;
        private readonly IHotelAdminDashboardService _dashboardService;


        public HotelCompanyController(IGenericRepository<HotelCompany> hotelRepo, IMapper mapper, IHotelAdminDashboardService dashboardService)
        {
            _hotelRepo = hotelRepo;
            _mapper = mapper;
            _dashboardService = dashboardService;
        }
         
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<Pagination<HotelCompanyReadDTO>>> GetAll([FromQuery] HotelCompanySpecParams specParams)
        {
            var spec = new HotelCompanyWithRoomsSpecification(specParams);
            var countSpec = new HotelCompanyWithFiltersForCountSpecification(specParams);

            var totalItems = await _hotelRepo.GetCountAsync(countSpec);
            var hotels = await _hotelRepo.GetAllWithSpecAsync(spec);
            var data = _mapper.Map<IReadOnlyList<HotelCompanyReadDTO>>(hotels);

            return Ok(new Pagination<HotelCompanyReadDTO>(specParams.PageIndex, specParams.PageSize, totalItems, data));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<HotelCompanyReadDTO>> GetById(int id)
        {
            var spec = new HotelCompanyWithRoomsSpecification(id);
            var hotel = await _hotelRepo.GetWithSpecAsync(spec);
            if (hotel == null) return NotFound();

            return Ok(_mapper.Map<HotelCompanyReadDTO>(hotel));
        }

         
        // Get hotels managed by current HotelAdmin
        [HttpGet("my-hotels")]
        [Authorize(Roles = "HotelAdmin")]
        public async Task<ActionResult<IEnumerable<HotelCompanyReadDTO>>> GetMyHotels()
        {

            var userId = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var hotels = await _hotelRepo.GetHotelsByAdminIdAsync(userId);

            var data = _mapper.Map<IReadOnlyList<HotelCompanyReadDTO>>(hotels);

            return Ok(data);
        }



        [HttpGet("dashboard")]
        [Authorize(Roles = "HotelAdmin")]
        public async Task<IActionResult> GetDashboard()
        {
            var hotelCompanyIdClaim = User.Claims.FirstOrDefault(c => c.Type == "HotelCompanyId")?.Value;

            if (string.IsNullOrEmpty(hotelCompanyIdClaim) || !int.TryParse(hotelCompanyIdClaim, out int hotelId))
                return Unauthorized("HotelCompanyId not found in token.");

            var data = await _dashboardService.GetStatsForHotel(hotelId);
            return Ok(data);
        }

    }
}
