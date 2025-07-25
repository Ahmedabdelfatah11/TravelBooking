using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelBooking.Core.DTOS.CarRentalCompanies;
using TravelBooking.Core.DTOS.Cars;
using TravelBooking.Core.Models;
using TravelBooking.Core.Repository.Contract;
using TravelBooking.Core.Specifications.CarRentalCompanySpecs;
using TravelBooking.Errors;
using TravelBooking.Helper;
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
        public async Task<ActionResult<IReadOnlyList<CarRentalWithCarsDto>>> GetCarRentalCompanies([FromQuery]
           CarRentalSpecParams specParams)
        {
            var spec = new CarRentalCompanySpecifications(specParams);
            var rentals = await _carRentalRepo.GetAllWithSpecAsync(spec);

            var data = _mapper.Map<IReadOnlyList<CarRentalCompany>, IReadOnlyList<CarRentalWithCarsDto>>(rentals);

            var countSpec = new CarRentalCompanyWithFilterForCountSpec(specParams);
            var count = await _carRentalRepo.GetCountAsync(countSpec);
            return Ok(new Pagination<CarRentalWithCarsDto>(specParams.PageIndex, specParams.PageSize, count, data));

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarRentalWithCarsDto>> GetCarRentalCompanyWithCarsById(int id)
        {
            var spec = new CarRentalCompanySpecifications(id);
            var rental = await _carRentalRepo.GetWithSpecAsync(spec);
            if (rental == null) return NotFound(new ApiResponse(404));
            return Ok(_mapper.Map<CarRentalCompany, CarRentalDto>(rental));

        }

        [HttpPost]
        public async Task<ActionResult<CarRentalDto>> CreateRental(SaveCarRentalDto dto)
        {
            var rental = _mapper.Map<CarRentalCompany>(dto);
            var result = await _carRentalRepo.AddAsync(rental);
            var resultDto = _mapper.Map<CarRentalDto>(result);

            return CreatedAtAction(nameof(GetCarRentalCompanyWithCarsById), new { id = result.Id }, resultDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateRental(int id, CarCreateUpdateDto dto)
        {
            var rental = await _carRentalRepo.GetAsync(id);
            if (rental == null) return NotFound(new ApiResponse(404));

            _mapper.Map(dto, rental);
            await _carRentalRepo.Update(rental);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRental(int id)
        {
            var rental = await _carRentalRepo.GetAsync(id);
            if (rental == null) return NotFound(new ApiResponse(404));

            await _carRentalRepo.Delete(rental);

            return NoContent();
        }
    }

}