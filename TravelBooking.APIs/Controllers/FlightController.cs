using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelBooking.APIs.DTOS.Booking.FlightBooking;
using TravelBooking.APIs.DTOS.Flight;
using TravelBooking.Core.Models;
using TravelBooking.Core.Repository.Contract;
using TravelBooking.Core.Specifications.FlightSpecs;
using TravelBooking.Errors;
using TravelBooking.Helper;

namespace TravelBooking.APIs.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FlightController : Controller
    {
        private readonly IGenericRepository<Flight> _flightRepo;
        private readonly IGenericRepository<Booking> _bookingRepo;
        private readonly IMapper _mapper;

        public FlightController(IGenericRepository<Flight> flightRepository, IMapper mapper, IGenericRepository<Booking> bookingRepo)
        {
            _flightRepo = flightRepository;
            _mapper = mapper;
            _bookingRepo = bookingRepo;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<FlightDTO>>> GetAllFlights([FromQuery] FlightSpecParams specParams)
        {
            var spec = new FlightSpecs(specParams);
            var flights = await _flightRepo.GetAllWithSpecAsync(spec);

            if (flights == null || !flights.Any())
                return NotFound("No flights found.");

            var flightDTOs = _mapper.Map<IReadOnlyList<FlightDTO>>(flights);
            var countSpec = new FlightCountSpec(specParams);
            var totalCount = await _flightRepo.GetCountAsync(countSpec);

            return Ok(new Pagination<FlightDTO>(specParams.PageIndex, specParams.PageSize, totalCount, flightDTOs));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FlightDetialsDTO>> GetFlight(int id)
        {
            var spec = new FlightSpecs(id);
            var flight = await _flightRepo.GetWithSpecAsync(spec);
            if (flight == null)
                return NotFound($"Flight with ID {id} not found.");

            var flightDTO = _mapper.Map<FlightDetialsDTO>(flight);
            return Ok(flightDTO);
        }

        [Authorize]
        [HttpPost("{serviceId}/book")]
        public async Task<IActionResult> BookFlight(int serviceId, [FromBody] FlightBookingDto dto)
        {
            var userId = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new ApiResponse(401, "User ID not found in token."));

            var flight = await _flightRepo.GetAsync(serviceId);
            if (flight == null)
                return NotFound(new ApiResponse(404, "Flight not found."));

            var existingBookings = await _bookingRepo.GetAllAsync(b =>
                b.UserId == userId &&
                b.FlightId == serviceId &&
                b.Status != Status.Cancelled &&
                b.StartDate == flight.DepartureTime &&
                b.EndDate == flight.ArrivalTime);

            if (existingBookings.Any())
                return BadRequest(new ApiResponse(400, "You already have a booking for this flight at the same time."));

            decimal price;
            switch (dto.SeatClass)
            {
                case SeatClass.Economy:
                    if (flight.EconomySeats <= 0)
                        return BadRequest(new ApiResponse(400, "No economy seats available."));
                    flight.EconomySeats--;
                    price = flight.EconomyPrice;
                    break;

                case SeatClass.Business:
                    if (flight.BusinessSeats <= 0)
                        return BadRequest(new ApiResponse(400, "No business class seats available."));
                    flight.BusinessSeats--;
                    price = flight.BusinessPrice;
                    break;

                case SeatClass.FirstClass:
                    if (flight.FirstClassSeats <= 0)
                        return BadRequest(new ApiResponse(400, "No first class seats available."));
                    flight.FirstClassSeats--;
                    price = flight.FirstClassPrice;
                    break;

                default:
                    return BadRequest(new ApiResponse(400, "Invalid seat class."));
            }

            var booking = new Booking
            {
                FlightId = serviceId,
                UserId = userId,
                BookingType = BookingType.Flight,
                Status = Status.Pending,
                StartDate = flight.DepartureTime,
                EndDate = flight.ArrivalTime,
                SeatClass = dto.SeatClass
            };

            await _bookingRepo.AddAsync(booking);
            await _flightRepo.Update(flight);

            booking.Flight = flight;

            var result = _mapper.Map<FlightBookingResultDto>(booking);
            result.BookingId = booking.Id;
            result.Price = price;

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Flight>> AddFlight([FromBody] FlightDTO dto)
        {
            var entity = _mapper.Map<Flight>(dto);
            var newFlight = await _flightRepo.AddAsync(entity);

            var resultDto = _mapper.Map<FlightDTO>(newFlight);
            return CreatedAtAction(nameof(GetFlight), new { id = newFlight.Id }, resultDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateFlight(int id, [FromBody] Flight flight)
        {
            if (flight.Id != id)
                return BadRequest("ID mismatch");

            await _flightRepo.Update(flight);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFlight(int id)
        {
            var existing = await _flightRepo.GetAsync(id);
            if (existing == null)
                return NotFound();

            await _flightRepo.Delete(existing);
            return NoContent();
        }
    }
}
