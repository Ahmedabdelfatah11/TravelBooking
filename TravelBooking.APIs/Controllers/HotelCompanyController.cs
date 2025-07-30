using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

using TravelBooking.APIs.DTOS.HotelCompany;
using TravelBooking.APIs.Helper;
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
        private readonly IDashboardService _dashboardService;

        public HotelCompanyController(IGenericRepository<HotelCompany> hotelRepo, IMapper mapper, IDashboardService dashboardService)
        {
            _hotelRepo = hotelRepo;
            _mapper = mapper;
            _dashboardService = dashboardService;
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

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult> Create([FromBody] HotelCompanyCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Validate AdminId
            if (string.IsNullOrWhiteSpace(dto.AdminId))
                return BadRequest("AdminId is required.");

            var model = _mapper.Map<HotelCompany>(dto);
            model.AdminId = dto.AdminId;

            await _hotelRepo.AddAsync(model);

            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
        }


        [HttpPut("{id}")]
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
            await _hotelRepo.Update(hotel);

            return Ok(hotel);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var hotel = await _hotelRepo.GetAsync(id);
            if (hotel == null) return NotFound();

            if (!IsHotelAdminAuthorizedForHotel(id))
                return Forbid();

            _hotelRepo.Delete(hotel);
            return Ok();
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
    }
}
