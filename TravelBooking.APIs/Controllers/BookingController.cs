using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelBooking.APIs.DTOS.Booking;
using TravelBooking.Core.DTOS.CarRentalCompanies;
using TravelBooking.Core.Models;
using TravelBooking.Repository.Data;
using TravelBooking.APIs.Dtos.HotelCompany;
using TravelBooking.Core.DTOS.Cars;
using TravelBooking.APIs.Dtos.Rooms;
using TravelBooking.Core.Repository.Contract;
using AutoMapper;
using TravelBooking.Core.Specifications.CarSpecs;
using TravelBooking.Core.Specifications.RoomSpecs;
using TravelBooking.Core.Specifications.FlightSpecs;
using TravelBooking.Core.Specifications.TourSpecs;
using TravelBooking.APIs.DTOS.Flight;
using TravelBooking.APIs.DTOS.Tours;


namespace TravelBooking.APIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        public async Task<ActionResult<BookingDto>> GetBookingById(int id)
        {
            var booking = await _bookingRepo.GetAsync(id);
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
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetAllBookings()
        {
            var bookings = await _bookingRepo.GetAllAsync();
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
                            var spec = new RoomSpecification(booking.RoomId.Value);
                            var room = await _roomRepo.GetWithSpecAsync(spec);
                            dto.AgencyDetails = _mapper.Map<RoomToReturnDTO>(room);
                        }
                        break;
                    case BookingType.Car:
                        if (booking.CarId.HasValue)
                        {
                            var spec = new CarSpecifications(booking.CarId.Value);
                            var car = await _carRepo.GetWithSpecAsync(spec);
                            dto.AgencyDetails = _mapper.Map<CarDto>(car);
                        }
                        break;
                    case BookingType.Flight:
                        if (booking.FlightId.HasValue)
                        {
                            var spec = new FlightSpecs(booking.FlightId.Value);
                            var flight = await _flightRepo.GetWithSpecAsync(spec);
                            dto.AgencyDetails = _mapper.Map<FlightDTO>(flight);
                        }
                        break;
                    case BookingType.Tour:
                        if (booking.TourId.HasValue)
                        {
                            var spec = new ToursSpecification(booking.TourId.Value);
                            var tour = await _tourRepo.GetWithSpecAsync(spec);
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
        public async Task<IActionResult> DeleteBookingById(int id)
        {
            var booking = await _bookingRepo.GetAsync(id);
            if (booking == null)
                return NotFound(new { message = "Booking not found." });

            await _bookingRepo.Delete(booking);
            return NoContent();
        }
    }
}
