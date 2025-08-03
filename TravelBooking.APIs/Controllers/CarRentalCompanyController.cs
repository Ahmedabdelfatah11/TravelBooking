using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelBooking.APIs.DTOS.HotelCompany;
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
    [Authorize(Roles = "SuperAdmin,CarRentalAdmin")]
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
        [AllowAnonymous]
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
        [AllowAnonymous]
        public async Task<ActionResult<CarRentalWithCarsDto>> GetCarRentalCompanyWithCarsById(int id)
        {
            var spec = new CarRentalCompanySpecifications(id);
            var rental = await _carRentalRepo.GetWithSpecAsync(spec);
            if (rental == null) return NotFound(new ApiResponse(404));
            return Ok(_mapper.Map<CarRentalCompany, CarRentalDto>(rental));

        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<CarRentalDto>> CreateRental([FromBody] SaveCarRentalDto dto)
        {

            // Validate AdminId
            if (string.IsNullOrWhiteSpace(dto.AdminId))
                return BadRequest("AdminId is required.");
            var rental = _mapper.Map<CarRentalCompany>(dto);
            rental.AdminId = dto.AdminId;

            var result = await _carRentalRepo.AddAsync(rental);
           
            return CreatedAtAction(nameof(GetCarRentalCompanyWithCarsById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "SuperAdmin,CarRentalAdmin")]
        public async Task<ActionResult> UpdateRental(int id, CarCreateUpdateDto dto)
        {
            var rental = await _carRentalRepo.GetAsync(id);
            if (rental == null) return NotFound(new ApiResponse(404));

            var userId = User.FindFirst("uid")?.Value;
            var userRoles = User.FindAll("role").Select(r => r.Value);

            if (userRoles.Contains("CarRentalAdmin") && rental.AdminId != userId)
                return Forbid("You are not authorized to update this rental company.");

            _mapper.Map(dto, rental);
            await _carRentalRepo.Update(rental);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin,CarRentalAdmin")]
        public async Task<ActionResult> DeleteRental(int id)
        {
            var rental = await _carRentalRepo.GetAsync(id);
            if (rental == null) return NotFound(new ApiResponse(404));

            var userId = User.FindFirst("uid")?.Value;
            var userRoles = User.FindAll("role").Select(r => r.Value);

            if (userRoles.Contains("CarRentalAdmin") && rental.AdminId != userId)
                return Forbid("You are not authorized to delete this rental company.");

            await _carRentalRepo.Delete(rental);

            return NoContent();
        }

        [HttpGet("my-companies")]
        [Authorize(Roles = "CarRentalAdmin")]
        public async Task<ActionResult<IEnumerable<CarRentalDto>>> GetMyCompanies()
        {
            var userId = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var companies = await _carRentalRepo.GetCarRentalByAdminIdAsync(userId);
            var data = _mapper.Map<IReadOnlyList<CarRentalDto>>(companies);

            return Ok(data);
        }

    }

}
