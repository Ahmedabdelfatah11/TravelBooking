using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelBooking.APIs.DTOS.Booking.CarBooking;
using TravelBooking.Core.DTOS.Cars;
using TravelBooking.Core.Models;
using TravelBooking.Core.Repository.Contract;
using TravelBooking.Core.Specifications;
using TravelBooking.Core.Specifications.CarSpecs;
using TravelBooking.Core.Specifications.RoomSpecs;
using TravelBooking.Errors;
using TravelBooking.Helper;
using TravelBooking.Repository;

namespace TravelBooking.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly IGenericRepository<Car> _carRepo;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Booking> _bookingRepo;

        public CarController(IGenericRepository<Car> carRepo, IMapper mapper, IGenericRepository<Booking> bookingRepo)
        {
            _carRepo = carRepo;
            _mapper = mapper;
            _bookingRepo = bookingRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CarDto>>> GetCars([FromQuery] CarSpecParams specParams)
        {
            var spec = new CarSpecifications(specParams);
            var countCarSpec = new CarsWithFilterForCountSpecification(specParams);

            var totalItems = await _carRepo.GetCountAsync(countCarSpec);

            var cars = await _carRepo.GetAllWithSpecAsync(spec);
            var data = _mapper.Map<IReadOnlyList<Car>, IReadOnlyList<CarDto>>(cars);

            //var data = _mapper.Map<IReadOnlyList<CarDto>>(cars);
            return Ok(new Pagination<CarDto>(specParams.PageIndex, specParams.PageSize, totalItems, data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarDto>> GetCar(int id)
        {

            var spec = new CarSpecifications(id);
            var car = await _carRepo.GetWithSpecAsync(spec);

            if (car == null)
                return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<Car, CarDto>(car));
        }

        [Authorize]
        [HttpPost("{serviceId}/book")]
        public async Task<IActionResult> BookCar(int serviceId, [FromBody] CarBookingDto dto)
        {
            var userId = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token.");

            var car = await _carRepo.GetAsync(serviceId);
            if (car == null) return NotFound(new ApiResponse(404));

            if (dto.StartDate >= dto.EndDate)
                return BadRequest("Start date must be before end date.");

            var existingBookings = await _bookingRepo.GetAllAsync(b =>
                b.CarId == serviceId &&
                b.Status != Status.Cancelled &&
                b.StartDate < dto.EndDate &&
                dto.StartDate < b.EndDate
            );
            if (existingBookings.Any())
                return BadRequest("Car is already booked during the selected time period.");

            var booking = _mapper.Map<Booking>(dto);
            booking.CarId = serviceId;
            booking.UserId = userId;
            booking.BookingType = BookingType.Car;
            booking.Status = Status.Pending;

            await _bookingRepo.AddAsync(booking);

            booking.Car = car;

            var resultDto = _mapper.Map<CarBookingResultDto>(booking);

            return CreatedAtAction("GetBookingById", "Booking", new { id = booking.Id }, resultDto);
        }



        //  POST
        [HttpPost]
        public async Task<ActionResult> CreateCar(CarCreateUpdateDto dto)
        {
            var car = _mapper.Map<Car>(dto);
            var result = await _carRepo.AddAsync(car);
            //await _carRepo.SaveChangesAsync(); // or CompleteAsync()
            return CreatedAtAction(nameof(GetCar), new { id = car.Id }, _mapper.Map<CarDto>(car));
        }

        //  PUT
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCar(int id, CarCreateUpdateDto dto)
        {
            var car = await _carRepo.GetAsync(id);
            if (car == null) return NotFound();

            _mapper.Map(dto, car);
            await _carRepo.Update(car);
            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCar(int id)
        {
            var car = await _carRepo.GetAsync(id);
            if (car == null) return NotFound();

            await _carRepo.Delete(car);

            return NoContent();
        }
    }
}
