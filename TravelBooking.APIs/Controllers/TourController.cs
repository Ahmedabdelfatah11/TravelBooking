using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelBooking.APIs.Dtos.TourCompany;
using TravelBooking.Core.Models;
using TravelBooking.Core.Repository.Contract;
using TravelBooking.Core.Specifications.TourCompanySpecs;
using TravelBooking.Core.Specifications;
using TravelBooking.Helper;
using TravelBooking.Repository.TourCompanySpecs;
using TravelBooking.APIs.Dtos.Tours;
using TravelBooking.Core.Specifications.TourSpecs;
using TravelBooking.Errors;

namespace TravelBooking.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourController : ControllerBase
    {
        private readonly IGenericRepository<Tour> _tourRepo;
        private readonly IMapper _mapper;

        public TourController(
            IGenericRepository<Tour> tourRepo,
            IMapper mapper)
        {
            _tourRepo = tourRepo;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<Pagination<TourReadDto>>> GetTourCompanies([FromQuery] TourSpecParams specParams)
        {
            var spec = new ToursSpecification(specParams);

            var companies = await _tourRepo.GetAllWithSpecAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Tour>, IReadOnlyList<TourReadDto>>(companies);

            var countSpec = new ToursWithFilterationForCountSpec(specParams);
            var count = await _tourRepo.GetCountAsync(countSpec);

            return Ok(new Pagination<TourReadDto>(specParams.PageIndex, specParams.PageSize, count, data));
        }

        [ProducesResponseType(typeof(TourCompanyReadDto), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ApiResponse))]
        [HttpGet("{id}")]
        public async Task<ActionResult<TourReadDto>> GetTourById(int id)
        {
            var spec = new ToursSpecification(id);

            var tour = await _tourRepo.GetWithSpecAsync(spec);

            if (tour is null)
                return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<Tour, TourReadDto>(tour));
        }


        [HttpPost]
        public async Task<ActionResult<TourReadDto>> CreateTour([FromBody] TourCreateDto dto)
        {
            var entity = _mapper.Map<Tour>(dto);
            var result = await _tourRepo.AddAsync(entity);
            var resultDto = _mapper.Map<TourReadDto>(result);
            return CreatedAtAction(nameof(GetTourById), new { id = result.Id }, resultDto);
        }

        // PUT: api/TourCompany/5
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTour(int id, [FromBody] TourUpdateDto dto)
        {
            var existing = await _tourRepo.GetAsync(id);
            if (existing == null) return NotFound();

            _mapper.Map(dto, existing);
            await _tourRepo.Update(existing);
            return NoContent();
        }

        // DELETE: api/TourCompany/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTour(int id)
        {
            var existing = await _tourRepo.GetAsync(id);
            if (existing == null) return NotFound();

            await _tourRepo.Delete(existing);
            return NoContent();
        }
    }
}
