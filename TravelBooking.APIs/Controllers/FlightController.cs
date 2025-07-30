using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TravelBooking.APIs.DTOS.Booking.FlightBooking;
using TravelBooking.APIs.DTOS.Flight;
using TravelBooking.APIs.DTOS.Tours;
using TravelBooking.Core.Models;
using TravelBooking.Core.Repository.Contract;
using TravelBooking.Core.Specifications.FlightSpecs;
using TravelBooking.Errors;
using TravelBooking.Helper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TravelBooking.APIs.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,User,FlightAdmin")]
    public class FlightController : Controller
    {
        private readonly IGenericRepository<Flight> flightRepository;
        private readonly IMapper mapper;
        private readonly IGenericRepository<Booking> _bookingRepo;

        public FlightController(IGenericRepository<Flight> flightRepository , IMapper mapper, IGenericRepository<Booking> bookingRepo)
        {
            this.flightRepository = flightRepository;
            this.mapper = mapper;
            _bookingRepo = bookingRepo;
        }
        // GET: FlightController
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<Pagination<FlightDTO>>> GetAllFlights( [FromQuery]FlightSpecParams specParams )
        {
           var spec =new FlightSpecs(specParams);
            var flights = await flightRepository.GetAllWithSpecAsync(spec);
            if (flights == null || !flights.Any())
            {
                return NotFound("No flights found.");
            }
            foreach (var flight in flights)
            {
                Console.WriteLine(flight.FlightCompany?.Name); // null ولا موجود؟
            }
            var flightDTOs = mapper.Map<IReadOnlyList<FlightDTO>>(flights);
            var countSpec = new FlightCountSpec(specParams);
            var totalCount = await flightRepository.GetCountAsync(countSpec);
            return Ok(new Pagination<FlightDTO>(specParams.PageIndex, specParams.PageSize, totalCount, flightDTOs));
        }
        // GET: FlightController/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<FlightDetialsDTO>> GetFlight( int id )
        {
            var spec = new FlightSpecs(id);
            var flight = await flightRepository.GetWithSpecAsync(spec);
            if (flight == null)
            {
                return NotFound($"Flight with ID {id} not found.");
            }
            var flightDTO = mapper.Map<FlightDetialsDTO>(flight);
            return Ok(flightDTO);
        }

        [Authorize]
        [HttpPost("{serviceId}/book")]
        [Authorize(Roles = "SuperAdmin,User")]
        public async Task<IActionResult> BookFlight(int serviceId/*, [FromBody] FlightBookingDto dto*/)
        {
            var userId = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token.");

            var flight = await flightRepository.GetAsync(serviceId);
            if (flight == null) return NotFound(new ApiResponse(404));

            var existingBookings = await _bookingRepo.GetAllAsync(b =>
               b.UserId == userId &&
               b.FlightId == serviceId &&
               b.Status != Status.Cancelled &&
               b.StartDate == flight.DepartureTime &&
               b.EndDate == flight.ArrivalTime
               );

            if (existingBookings.Any())
                return BadRequest("Tour already booked in selected time frame.");

            var booking = new Booking
            {
                FlightId = serviceId,
                UserId = userId,
                BookingType = BookingType.Flight,
                Status = Status.Pending,
                StartDate = flight.DepartureTime,
                EndDate = flight.ArrivalTime
            };

            await _bookingRepo.AddAsync(booking);

            booking.Flight = flight; 
            var result = mapper.Map<FlightBookingResultDto>(booking);

            return CreatedAtAction("GetBookingById", "Booking", new { id = booking.Id }, result);
        }





        [HttpPost]
        [Authorize(Roles = "SuperAdmin,FlightAdmin")]
        public async Task<ActionResult<Flight>> AddFlight([FromBody] FlightDTO dto)
        {
            var entity = mapper.Map<Flight>(dto);
            var newCompany = await flightRepository.AddAsync(entity);

            var resultDto = mapper.Map<FlightDTO>(newCompany);
            return CreatedAtAction(nameof(GetFlight), new { id = newCompany.Id }, resultDto);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "SuperAdmin,FlightAdmin")]
        public async Task<ActionResult> UpdateFlight(int id, Flight company)
        {
            if (company.Id != id) return BadRequest("ID mismatch");

            await flightRepository.Update(company);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin,FlightAdmin")]
        public async Task<ActionResult> DeleteFlight(int id)
        {
            var existing = await flightRepository.GetAsync(id);
            if (existing == null) return NotFound();

            await flightRepository.Delete(existing);
            return NoContent();
        }
    }
}
