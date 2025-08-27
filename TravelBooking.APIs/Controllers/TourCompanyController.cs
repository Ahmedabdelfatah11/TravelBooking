using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TravelBooking.APIs.DTOS.Booking;
using TravelBooking.APIs.DTOS.TourCompany;
using TravelBooking.APIs.DTOS.Tours;
using TravelBooking.Core.Models;
using TravelBooking.Core.Repository.Contract;
using TravelBooking.Core.Specifications;
using TravelBooking.Core.Specifications.TourCompanySpecs;
using TravelBooking.Core.Specifications.TourSpecs;
using TravelBooking.Core.Specifications.TourSpescs;
using TravelBooking.Errors;
using TravelBooking.Helper;
using TravelBooking.Repository.TourCompanySpecs;
using TravelBooking.Service.Services.Dashboard;


namespace TravelBooking.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,TourAdmin")]
    public class TourCompanyController : ControllerBase
    {
        private readonly IGenericRepository<TourCompany> _tourCompanyRepo;
        private readonly IMapper _mapper;
        private readonly ITourAdminDashboardService _dashboardService;
        private readonly IGenericRepository<Tour> _tourRepo;
        private readonly IGenericRepository<Booking> _bookingRepo;

        public TourCompanyController(
            IGenericRepository<TourCompany> tourCompanyRepo,
            IMapper mapper, ITourAdminDashboardService dashboardService,IGenericRepository<Tour> tourRepo,IGenericRepository<Booking> bookingRepo)
        {
            _tourCompanyRepo = tourCompanyRepo;
            _mapper = mapper;
            _dashboardService = dashboardService;
            _tourRepo = tourRepo;
            _bookingRepo = bookingRepo;
        }


        private bool IsTourAdminAuthorizedForCompany(int companyId)
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole == "TourAdmin")
            {
                var tourCompanyIdClaim = User.Claims.FirstOrDefault(c => c.Type == "TourCompanyId")?.Value;
                if (tourCompanyIdClaim == null || tourCompanyIdClaim != companyId.ToString())
                    return false;
            }
            return true;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<Pagination<TourCompanyReadDto>>> GetTourCompanies([FromQuery] TourCompanySpecParams specParams)
        {
            var spec = new TourCompanyWithToursSpecification(specParams);

            var companies = await _tourCompanyRepo.GetAllWithSpecAsync(spec);

            var data = _mapper.Map<IReadOnlyList<TourCompany>, IReadOnlyList<TourCompanyReadDto>>(companies);

            var countSpec = new TourCompanyWithFilterationForCountSpecifications(specParams);
            var count = await _tourCompanyRepo.GetCountAsync(countSpec);

            return Ok(new Pagination<TourCompanyReadDto>(specParams.PageIndex, specParams.PageSize, count, data));
        }

      
        [ProducesResponseType(typeof(TourCompanyReadDto), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ApiResponse))]
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<TourCompanyReadDto>> GetTourCompanyById(int id)
        {
            var spec = new TourCompanyWithToursSpecification(id);

            var company = await _tourCompanyRepo.GetWithSpecAsync(spec);

            if (company is null)
                return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<TourCompany, TourCompanyReadDto>(company));
        }

        // PUT: api/TourCompany/5
        [HttpPut("{id}")]
        [Authorize(Roles = "SuperAdmin,TourAdmin")]
        public async Task<ActionResult> UpdateTourCompany(int id, [FromBody] TourCompanyCreateDto dto)
        {
            var existing = await _tourCompanyRepo.GetAsync(id);
            if (existing == null) return NotFound(new ApiResponse(404));

            if (!IsTourAdminAuthorizedForCompany(id))
                return Forbid();

            _mapper.Map(dto, existing);
            await _tourCompanyRepo.Update(existing);
            return NoContent();
        }

        // DELETE: api/TourCompany/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin,TourAdmin")]
        public async Task<ActionResult> DeleteTourCompany(int id)
        {
            var existing = await _tourCompanyRepo.GetAsync(id);
            if (existing == null) return NotFound(new ApiResponse(404));

            if (!IsTourAdminAuthorizedForCompany(id))
                return Forbid();

            await _tourCompanyRepo.Delete(existing);
            return NoContent();
        }


        // Get Tours managed by current TourAdmin
        [HttpGet("my-tours")]
        [Authorize(Roles = "TourAdmin")]
        public async Task<ActionResult<IEnumerable<TourReadDto>>> GetMyTours()
        {
            var userId = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var spec = new ToursWithTourCompanyAdminSpec(userId);
            var tours = await _tourRepo.GetAllWithSpecAsync(spec);

            var data = _mapper.Map<IReadOnlyList<TourReadDto>>(tours);
            return Ok(data);
        }

        [HttpGet("dashboard")]
        [Authorize(Roles = "TourAdmin")]
        public async Task<IActionResult> GetDashboard()
        {
            var tourCompanyIdClaim = User.Claims.FirstOrDefault(c => c.Type == "TourCompanyId")?.Value;

            if (string.IsNullOrEmpty(tourCompanyIdClaim) || !int.TryParse(tourCompanyIdClaim, out int tourCompanyId))
                return Unauthorized("TourCompanyId not found in token.");

            var data = await _dashboardService.GetStatsForTourCompany(tourCompanyId);
            return Ok(data);
        }
        [HttpGet("my-companies")]
        [Authorize(Roles = "TourAdmin")]
        public async Task<ActionResult<IEnumerable<TourCompanyReadDto>>> GetMyTourCompanies()
        {
            var userId = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            // Get all TourCompanies where AdminId == current user ID
            var spec = new TourCompanyWithAdminSpec(userId); // ← You need to create this spec
            var companies = await _tourCompanyRepo.GetAllWithSpecAsync(spec);

            if (!companies.Any())
                return NotFound(new { message = "No tour companies found for this admin." });

            var data = _mapper.Map<IEnumerable<TourCompanyReadDto>>(companies);
            return Ok(data);
        }


        // GET: api/TourCompany/5/bookings
        [HttpGet("{id}/bookings")]
        [Authorize(Roles = "TourAdmin")]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookingsByTourCompany(int id)
        {
            Console.WriteLine($"🎯 GetBookingsByTourCompany called with company ID: {id}");

            if (!IsTourAdminAuthorizedForCompany(id))
                return Forbid();

            // ✅ Fix: Use the companyId overload
            var tourSpec = new ToursSpecification(companyId: id);
            var tours = await _tourRepo.GetAllWithSpecAsync(tourSpec);

            var tourIds = tours.Select(t => t.Id).ToList();
            if (!tourIds.Any())
            {
                Console.WriteLine("❌ No tours found for this company");
                return Ok(new List<BookingDto>());
            }

            Console.WriteLine($"✅ Found tours: {string.Join(", ", tourIds)}");

            var bookingSpec = new BookingSpecification();
            var allBookings = await _bookingRepo.GetAllWithSpecAsync(bookingSpec);

            var companyBookings = allBookings
                .Where(b => b.BookingType == BookingType.Tour &&
                            b.TourId.HasValue &&
                            tourIds.Contains(b.TourId.Value))
                .ToList();

            var dtoList = new List<BookingDto>();
            foreach (var booking in companyBookings)
            {
                var dto = _mapper.Map<BookingDto>(booking);
                var tour = tours.FirstOrDefault(t => t.Id == booking.TourId);
                if (tour != null)
                {
                    dto.AgencyDetails = _mapper.Map<TourReadDto>(tour);
                }
                dtoList.Add(dto);
            }

            Console.WriteLine($"✅ Returning {dtoList.Count} bookings");
            return Ok(dtoList);
        }
    }
}