using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public CarController(IGenericRepository<Car> carRepo, IMapper mapper)
        {
            _carRepo = carRepo;
            _mapper = mapper;
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

