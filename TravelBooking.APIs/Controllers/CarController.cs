using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelBooking.Core;
using TravelBooking.Core.DTOS;
using TravelBooking.Core.Models;
using TravelBooking.Core.Params;
using TravelBooking.Core.Repository.Contract;
using TravelBooking.Core.Specifications;
using TravelBooking.Repository;
using TravelBooking.Service.Dto;

namespace TravelBooking.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly IGenericRepository<CarDTO> _carRepo;
        private readonly IMapper _mapper;

        public CarController(IGenericRepository<CarDTO> carRepo, IMapper mapper)
        {
            _carRepo = carRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<CarDTO>>> GetCars([FromQuery] CarSpecParams specParams)
        {
            var spec = new CarsWithFilterSpecification(specParams);
            var cars = await _carRepo.GetAllWithSpecAsync(spec);
            var data = _mapper.Map<IReadOnlyList<CarDto>>(cars);
            var spec_count = new CarCountspec(specParams);
            var count = await _carRepo.GetCountAsync(spec_count);
            return Ok(new Pagination<CarDto>(specParams.PageIndex,specParams.PageSize,count,data));



        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarDto>> GetCar(int id)
        {
            var spec = new CarsWithFilterSpecification(id);
            var car = await _carRepo.GetWithSpecAsync(spec);
            if (car == null) return NotFound();
            return Ok(_mapper.Map<CarDto>(car));
        }

        //  POST
        [HttpPost]
        public async Task<ActionResult> CreateCar(CarCreateUpdateDto dto)
        {
            var car = _mapper.Map<CarDTO>(dto);
            await _carRepo.AddAsync(car);
            await _carRepo.SaveChangesAsync(); // or CompleteAsync()
            return CreatedAtAction(nameof(GetCar), new { id = car.Id }, _mapper.Map<CarDto>(car));
        }

        //  PUT
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCar(int id, CarCreateUpdateDto dto)
        {
            var car = await _carRepo.GetAsync(id);
            if (car == null) return NotFound();

            _mapper.Map(dto, car);
            _carRepo.Update(car);
            // await dbcon.();
            await _carRepo.SaveChangesAsync(); // or CompleteAsync()

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCar(int id)
        {
            var car = await _carRepo.GetAsync(id);
            if (car == null) return NotFound();

            _carRepo.Delete(car);
            await _carRepo.SaveChangesAsync(); // or CompleteAsync()

            return NoContent();
        }

       
    }
}

