using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelBooking.Core.Models;

using TravelBooking.Helper;
using TravelBooking.Errors;
using TravelBooking.APIs.DTOS.Booking.TourBooking;
using Microsoft.AspNetCore.Authorization;
using TravelBooking.Core.Repository.Contract;
using AutoMapper;
using TravelBooking.Core.Specifications.TourSpecs;
using TravelBooking.APIs.DTOS.Tours;
using TravelBooking.APIs.DTOS.TourCompany;
using Microsoft.EntityFrameworkCore;
using TravelBooking.Repository.Data;
using TravelBooking.APIs.Extensions;

namespace TravelBooking.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,TourAdmin")]
    public class TourController : ControllerBase
    {
        private readonly IGenericRepository<Tour> _tourRepo;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Booking> _bookingRepo;
        private readonly IGenericRepository<TourCompany> _tourCompany;
        private readonly AppDbContext _context;

        public TourController(
            IGenericRepository<Tour> tourRepo,
            IMapper mapper,
             IGenericRepository<Booking> bookingRepo,
             IGenericRepository<TourCompany> tourCompany, AppDbContext context)
        {
            _tourRepo = tourRepo;
            _mapper = mapper;
            _bookingRepo = bookingRepo;
            _tourCompany = tourCompany;
            _context = context;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<Pagination<TourReadDto>>> GetTour([FromQuery] TourSpecParams specParams)
        {
            var spec = new ToursSpecification(specParams);

            var tours = await _tourRepo.GetAllWithSpecAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Tour>, IReadOnlyList<TourReadDto>>(tours);

            var countSpec = new ToursWithFilterationForCountSpec(specParams);
            var count = await _tourRepo.GetCountAsync(countSpec);
            var baseQuery = _context.Tours.AsQueryable().ApplyFiltering(specParams);
            var (minPrice, maxPrice) = await baseQuery.GetPriceBoundsAsync();

            var priceBounds = new { min = minPrice, max = maxPrice };

            return Ok(new
            {
                pagination = new Pagination<TourReadDto>(specParams.PageIndex, specParams.PageSize, count, data),
                priceBounds = priceBounds
            });
        }

        [ProducesResponseType(typeof(TourCompanyReadDto), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ApiResponse))]
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<TourReadDto>> GetTourById(int id)
        {
            var spec = new ToursSpecification(id);

            var tour = await _tourRepo.GetWithSpecAsync(spec);

            if (tour is null)
                return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<Tour, TourReadDto>(tour));
        }


        [HttpPost]
        [Authorize(Roles = "SuperAdmin,TourAdmin")]
        public async Task<ActionResult<TourReadDto>> CreateTour([FromBody] TourCreateDto dto)
        {
            var entity = _mapper.Map<Tour>(dto);
            var result = await _tourRepo.AddAsync(entity);
            if (dto.Tickets != null && dto.Tickets.Any())
            {
                foreach (var ticketDto in dto.Tickets)
                {
                    var ticket = _mapper.Map<TourTicket>(ticketDto);
                    ticket.TourId = result.Id;
                    await _context.TourTickets.AddAsync(ticket);
                }
                await _context.SaveChangesAsync();
            }


            // Load TourCompany so AutoMapper can project its Name
            result.TourCompany = await _tourCompany.GetAsync(dto.TourCompanyId.Value);

            var resultDto = _mapper.Map<TourReadDto>(result);
            return CreatedAtAction(nameof(GetTourById), new { id = result.Id }, resultDto);
        }
        [Authorize]
        [HttpPost("{serviceId}/book")]
        [Authorize(Roles = "SuperAdmin,TourAdmin,User")]
        public async Task<IActionResult> BookTour(int serviceId, [FromBody] TourBookingDto dto)
        {
            var userId = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token.");

            var tour = await _tourRepo.GetAsync(serviceId);
            if (tour == null) return NotFound(new ApiResponse(404));

            if (tour.StartDate >= tour.EndDate)
                return BadRequest("Invalid tour date range.");

            var existingBookings = await _bookingRepo.GetAllAsync(b =>
                b.TourId == serviceId &&
                b.Status != Status.Cancelled &&
                b.StartDate < tour.EndDate &&
                tour.StartDate < b.EndDate
            );
            if (existingBookings.Any())
                return BadRequest("Tour already booked in selected time frame.");

            foreach (var ticketRequest in dto.Tickets)
            {
                var ticket = await _context.TourTickets
                    .FirstOrDefaultAsync(t =>
                        t.TourId == serviceId &&
                        t.Type == ticketRequest.Type &&
                        t.IsActive);

                if (ticket == null)
                    return BadRequest($"Ticket type '{ticketRequest.Type}' not found.");

                if (ticket.MaxQuantity < ticketRequest.Quantity)
                    return BadRequest($"Not enough '{ticket.Type}' tickets available.");

                ticket.MaxQuantity -= ticketRequest.Quantity;
                _context.TourTickets.Update(ticket);
            }

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
            await _context.SaveChangesAsync(); // ✅ Save ticket updates + booking

            booking.Tour = tour;
            var result = _mapper.Map<TourBookingResultDto>(booking);
            return CreatedAtAction("GetBookingById", "Booking", new { id = booking.Id }, result);
        }





        // PUT: api/TourCompany/5
        [HttpPut("{id}")]
        [Authorize(Roles = "SuperAdmin,TourAdmin,User")]
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
        [Authorize(Roles = "SuperAdmin,TourAdmin,User")]
        public async Task<ActionResult> DeleteTour(int id)
        {
            var existing = await _tourRepo.GetAsync(id);
            if (existing == null) return NotFound();

            await _tourRepo.Delete(existing);
            return NoContent();
        }
      
    }
}
