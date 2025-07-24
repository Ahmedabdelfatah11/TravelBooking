using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelBooking.APIs.Dtos.TourCompany;
using TravelBooking.Core.Models;
using TravelBooking.Core.Repository.Contract;
using TravelBooking.Core.Specifications.TourCompanySpecs;
using TravelBooking.Core.Specifications;
using TravelBooking.Helper;
using TravelBooking.Repository.TourCompanySpecs;
using TravelBooking.APIs.Dtos.Tours;
using TravelBooking.Core.Specifications.TourSpecs;
using TravelBooking.Errors;
using TravelBooking.APIs.DTOS.Booking;
using TravelBooking.APIs.DTOS.Booking.TourBooking;
using Microsoft.AspNetCore.Authorization;

namespace TravelBooking.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourController : ControllerBase
    {
        private readonly IGenericRepository<Tour> _tourRepo;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Booking> _bookingRepo;
        private readonly IGenericRepository<TourCompany> _tourCompany;

        public TourController(
            IGenericRepository<Tour> tourRepo,
            IMapper mapper,
             IGenericRepository<Booking> bookingRepo,
             IGenericRepository<TourCompany> tourCompany)
        {
            _tourRepo = tourRepo;
            _mapper = mapper;
            _bookingRepo = bookingRepo;
            _tourCompany = tourCompany;
        }
        [HttpGet]
        public async Task<ActionResult<Pagination<TourReadDto>>> GetTourCompanies([FromQuery] TourSpecParams specParams)
        {
            var spec = new ToursSpecification(specParams);

            var companies = await _tourRepo.GetAllWithSpecAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Tour>, IReadOnlyList<TourReadDto>>(companies);

            var countSpec = new ToursWithFilterationForCountSpec(specParams);
            var count = await _tourRepo.GetCountAsync(countSpec);

            return Ok(new Pagination<TourReadDto>(specParams.PageIndex, specParams.PageSize, count, data));
        }

        [ProducesResponseType(typeof(TourCompanyReadDto), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ApiResponse))]
        [HttpGet("{id}")]
        public async Task<ActionResult<TourReadDto>> GetTourById(int id)
        {
            var spec = new ToursSpecification(id);

            var tour = await _tourRepo.GetWithSpecAsync(spec);

            if (tour is null)
                return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<Tour, TourReadDto>(tour));
        }


        [HttpPost]
        public async Task<ActionResult<TourReadDto>> CreateTour([FromBody] TourCreateDto dto)
        {
            var entity = _mapper.Map<Tour>(dto);
            var result = await _tourRepo.AddAsync(entity);

            // Load TourCompany so AutoMapper can project its Name
            result.TourCompany = await _tourCompany.GetAsync(dto.TourCompanyId.Value);

            var resultDto = _mapper.Map<TourReadDto>(result);
            return CreatedAtAction(nameof(GetTourById), new { id = result.Id }, resultDto);
        }
        [Authorize]
        [HttpPost("{serviceId}/book")]
        public async Task<IActionResult> BookTour(int serviceId/*, [FromBody] TourBookingDto? dto*/)
        {
            var userId = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token.");

            var tour = await _tourRepo.GetAsync(serviceId);
            if (tour == null) return NotFound(new ApiResponse(404));

            // Validate that the tour date is valid (optional but recommended)
            if (tour.StartDate >= tour.EndDate)
                return BadRequest("Invalid tour date range.");

            // Check for overlapping bookings
            var existingBookings = await _bookingRepo.GetAllAsync(b =>
                b.TourId == serviceId &&
                b.Status != Status.Cancelled &&
                b.StartDate < tour.EndDate &&
                tour.StartDate < b.EndDate
            );
            if (existingBookings.Any())
                return BadRequest("Tour already booked in selected time frame.");

            var booking = new Booking
            {
                UserId = userId,
                TourId = serviceId,
                StartDate = tour.StartDate,
                EndDate = tour.EndDate,
                BookingType = BookingType.Tour,
                Status = Status.Pending
            };

            await _bookingRepo.AddAsync(booking);
            booking.Tour = tour;

            var result = _mapper.Map<TourBookingResultDto>(booking);
            return CreatedAtAction("GetBookingById", "Booking", new { id = booking.Id }, result);
        }




        // PUT: api/TourCompany/5
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTour(int id, [FromBody] TourUpdateDto dto)
        {
            var existing = await _tourRepo.GetAsync(id);
            if (existing == null) return NotFound();

            _mapper.Map(dto, existing);
            await _tourRepo.Update(existing);
            return NoContent();
        }

        // DELETE: api/TourCompany/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTour(int id)
        {
            var existing = await _tourRepo.GetAsync(id);
            if (existing == null) return NotFound();

            await _tourRepo.Delete(existing);
            return NoContent();
        }
    }
}
