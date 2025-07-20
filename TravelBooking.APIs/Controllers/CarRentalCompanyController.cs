using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelBooking.Core;
using TravelBooking.Core.DTOS;
using TravelBooking.Core.Models;
using TravelBooking.Core.Repository.Contract;
using TravelBooking.Core.Specifications;
using TravelBooking.Helper;
using TravelBooking.Repository;
using TravelBooking.Service.Dto;

namespace TravelBooking.APIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarRentalController : ControllerBase
    {
        private readonly IGenericRepository<CarRentalCompany> _carRentalRepo;
        private readonly IMapper _mapper;

        public CarRentalController(
            IGenericRepository<CarRentalCompany> carRentalRepo,
            IMapper mapper)
        {
            _carRentalRepo = carRentalRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CarRentalDto>>> GetPagedRentals(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10)
        {
            var spec = new SpecificationWithCars(pageIndex, pageSize);
            var rentals = await _carRentalRepo.GetAllWithSpecAsync(spec);
            return Ok(_mapper.Map<IReadOnlyList<CarRentalDto>>(rentals));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarRentalDto>> GetRentalWithCars(int id)
        {
            var spec = new SpecificationWithCars(id);
            var rental = await _carRentalRepo.GetWithSpecAsync(spec);
            if (rental == null) return NotFound();

            return Ok(_mapper.Map<CarRentalDto>(rental));
        }

        [HttpPost]
        public async Task<ActionResult<CarRentalDto>> CreateRental(SaveCarRentalDto dto)
        {
            var rental = _mapper.Map<CarRentalCompany>(dto);
            await _carRentalRepo.AddAsync(rental);
            await _carRentalRepo.SaveChangesAsync(); 

            return Ok(_mapper.Map<CarRentalDto>(rental));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateRental(int id, SaveCarRentalDto dto)
        {
            var rental = await _carRentalRepo.GetAsync(id);
            if (rental == null) return NotFound();

            _mapper.Map(dto, rental);
            _carRentalRepo.Update(rental);
            await _carRentalRepo.SaveChangesAsync(); 

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRental(int id)
        {
            var rental = await _carRentalRepo.GetAsync(id);
            if (rental == null) return NotFound();

            _carRentalRepo.Delete(rental);
            await _carRentalRepo.SaveChangesAsync(); 

            return NoContent();
        }
    }

}
