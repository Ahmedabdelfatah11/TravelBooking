using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelBooking.APIs.DTOS.Booking;
using TravelBooking.APIs.DTOS.Flight;
using TravelBooking.APIs.DTOS.Rooms;
using TravelBooking.APIs.DTOS.Tours;
using TravelBooking.Core.DTOS.CarRentalCompanies;
using TravelBooking.Core.DTOS.Cars;
using TravelBooking.Core.Models;
using TravelBooking.Core.Repository.Contract;
using TravelBooking.Core.Specifications.CarSpecs;
using TravelBooking.Core.Specifications.FlightSpecs;
using TravelBooking.Core.Specifications.RoomSpecs;
using TravelBooking.Core.Specifications.TourSpecs;
using TravelBooking.Repository.Data;


namespace TravelBooking.APIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "SuperAdmin,User,HotelAdmin,TourAdmin,FlightAdmin,CarAdmin")]
    public class BookingController : ControllerBase
    {
        private readonly IGenericRepository<Booking> _bookingRepo;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly IGenericRepository<Room> _roomRepo;
        private readonly IGenericRepository<Car> _carRepo;
        private readonly IGenericRepository<Flight> _flightRepo;
        private readonly IGenericRepository<Tour> _tourRepo;
        public BookingController(IGenericRepository<Booking> bookingRepo,
            IGenericRepository<Room> roomRepo,
            IGenericRepository<Car> carRepo,
            IGenericRepository<Flight> flightRepo,
            IGenericRepository<Tour> tourRepo,
            IMapper mapper)
        {
            _bookingRepo = bookingRepo;
            _roomRepo = roomRepo;
            _carRepo = carRepo;
            _flightRepo = flightRepo;
            _tourRepo = tourRepo;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "SuperAdmin,User,HotelAdmin,TourAdmin,FlightAdmin,CarAdmin")]
        public async Task<ActionResult<BookingDto>> GetBookingById(int id)
        {
            var spec = new BookingSpecification(id);
            var booking = await _bookingRepo.GetWithSpecAsync(spec);

            if (booking is null) return NotFound();

            var dto = _mapper.Map<BookingDto>(booking);

            // Load agency details manually based on BookingType
            switch (booking.BookingType)
            {
                case BookingType.Room:
                    var Roomspec = new RoomSpecification(booking.RoomId.Value);
                    var room = await _roomRepo.GetWithSpecAsync(Roomspec);
                    dto.AgencyDetails = _mapper.Map<RoomToReturnDTO>(room);
                    break;
                case BookingType.Car:
                    var Carspec = new CarSpecifications(booking.CarId.Value);
                    var car = await _carRepo.GetWithSpecAsync(Carspec);
                    dto.AgencyDetails = _mapper.Map<CarDto>(car);
                    break;
                case BookingType.Flight:
                    var Flightspec = new FlightSpecs(booking.FlightId.Value);
                    var flight = await _flightRepo.GetWithSpecAsync(Flightspec);
                    dto.AgencyDetails = _mapper.Map<FlightDTO>(flight);
                    break;
                case BookingType.Tour:
                    var Tourspec = new ToursSpecification(booking.TourId.Value);
                    var tour = await _tourRepo.GetWithSpecAsync(Tourspec);
                    dto.AgencyDetails = _mapper.Map<TourReadDto>(tour);
                    break;
            }

            return Ok(dto);
        }

        // GET: api/booking
        [HttpGet]
        [Authorize(Roles = "SuperAdmin,User,HotelAdmin,TourAdmin,FlightAdmin,CarAdmin")]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetAllBookings()
        {
            var bookingSpec = new BookingSpecification();
            var bookings = await _bookingRepo.GetAllWithSpecAsync(bookingSpec);

            var dtoList = new List<BookingDto>();

            foreach (var booking in bookings)
            {
                var dto = _mapper.Map<BookingDto>(booking);

                // Attach agency details based on booking type
                switch (booking.BookingType)
                {
                    case BookingType.Room:
                        if (booking.RoomId.HasValue)
                        {
                            var roomSpec = new RoomSpecification(booking.RoomId.Value);
                            var room = await _roomRepo.GetWithSpecAsync(roomSpec);
                            dto.AgencyDetails = _mapper.Map<RoomToReturnDTO>(room);
                        }
                        break;

                    case BookingType.Car:
                        if (booking.CarId.HasValue)
                        {
                            var carSpec = new CarSpecifications(booking.CarId.Value);
                            var car = await _carRepo.GetWithSpecAsync(carSpec);
                            dto.AgencyDetails = _mapper.Map<CarDto>(car);
                        }
                        break;

                    case BookingType.Flight:
                        if (booking.FlightId.HasValue)
                        {
                            var flightSpec = new FlightSpecs(booking.FlightId.Value);
                            var flight = await _flightRepo.GetWithSpecAsync(flightSpec);
                            dto.AgencyDetails = _mapper.Map<FlightDTO>(flight);
                        }
                        break;

                    case BookingType.Tour:
                        if (booking.TourId.HasValue)
                        {
                            var tourSpec = new ToursSpecification(booking.TourId.Value);
                            var tour = await _tourRepo.GetWithSpecAsync(tourSpec);
                            dto.AgencyDetails = _mapper.Map<TourReadDto>(tour);
                        }
                        break;
                }

                dtoList.Add(dto);
            }

            return Ok(dtoList);
        }


        // DELETE: api/booking/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin,User,HotelAdmin,TourAdmin,FlightAdmin,CarAdmin")]
        public async Task<IActionResult> DeleteBookingById(int id)
        {
            var booking = await _bookingRepo.GetAsync(id);
            if (booking == null)
                return NotFound(new { message = "Booking not found." });

            await _bookingRepo.Delete(booking);
            return NoContent();
        }

        [HttpPost("confirm/{bookingId}")]
        public async Task<IActionResult> ConfirmBooking(int bookingId)
        {
            var booking = await _context.Bookings
                .Include(b => b.BookingTickets)
                .ThenInclude(bt => bt.Ticket)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null || booking.Status != Status.Pending)
                return BadRequest("Invalid booking");

            booking.Status = Status.Confirmed;

            foreach (var bt in booking.BookingTickets)
            {
                bt.IsIssued = true;
            }

            await _context.SaveChangesAsync();
            return Ok("Booking confirmed and tickets issued");
        }
        [HttpPost("cancel/{bookingId}")]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            var booking = await _context.Bookings
                .Include(b => b.BookingTickets)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null || booking.Status != Status.Confirmed)
                return BadRequest("Invalid booking");

            booking.Status = Status.Cancelled;

            foreach (var bt in booking.BookingTickets)
            {
                bt.IsIssued = false;
            }

            await _context.SaveChangesAsync();
            return Ok("Booking canceled and tickets revoked");
        }

        [HttpGet("user/{userId}/tickets")]
        public async Task<IActionResult> GetUserTickets(string userId)
        {
            var tickets = await _context.TourBookingTickets
                .Include(bt => bt.Ticket)
                .Include(bt => bt.Booking)
                .Where(bt => bt.Booking.UserId == userId && bt.IsIssued)
                .ToListAsync();

            return Ok(tickets);
        }
    }
}
