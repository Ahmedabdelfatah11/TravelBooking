using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalabatAPIs.Errors;
using TravelBooking.APIs.Dtos.TourCompany;
using TravelBooking.Core.Models;
using TravelBooking.Core.Repository.Contract;
using TravelBooking.Core.Specifications;
using TravelBooking.Core.Specifications.TourCompanySpecs;
using TravelBooking.Helper;
using TravelBooking.Repository.TourCompanySpecs;


namespace TravelBooking.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]  // Assuming only authorized companies can access
    public class TourCompanyController : ControllerBase
    {
        private readonly IGenericRepository<TourCompany> _tourCompanyRepo;
        private readonly IMapper _mapper;

        public TourCompanyController(
            IGenericRepository<TourCompany> tourCompanyRepo,
            IMapper mapper)
        {
            _tourCompanyRepo = tourCompanyRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<TourCompanyReadDto>>> GetTourCompanies([FromQuery] TourCompanySpecParams specParams)
        {
            var spec = new TourCompanyWithToursSpecification(specParams);

            var companies = await _tourCompanyRepo.GetAllWithSpecAsync(spec);

            var data = _mapper.Map<IReadOnlyList<TourCompany>, IReadOnlyList<TourCompanyReadDto>>(companies);

            var countSpec = new TourCompanyWithFilterationForCountSpecifications(specParams);
            var count = await _tourCompanyRepo.GetCountAsync(countSpec);

            return Ok(new Pagination<TourCompanyReadDto>(specParams.PageIndex, specParams.PageSize, count, data));
        }

      
        [ProducesResponseType(typeof(TourCompanyReadDto), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ApiResponse))]
        [HttpGet("{id}")]
        public async Task<ActionResult<TourCompanyReadDto>> GetTourCompanyById(int id)
        {
            var spec = new TourCompanyWithToursSpecification(id);

            var company = await _tourCompanyRepo.GetWithSpecAsync(spec);

            if (company is null)
                return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<TourCompany, TourCompanyReadDto>(company));
        }
        [HttpPost]
        public async Task<ActionResult<TourCompanyReadDto>> CreateTourCompany([FromBody] TourCompanyCreateDto dto)
        {
            var entity = _mapper.Map<TourCompany>(dto);
            var result = await _tourCompanyRepo.AddAsync(entity);
            var resultDto = _mapper.Map<TourCompanyReadDto>(result);
            return CreatedAtAction(nameof(GetTourCompanyById), new { id = result.Id }, resultDto);
        }

        // PUT: api/TourCompany/5
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTourCompany(int id, [FromBody] TourCompanyCreateDto dto)
        {
            var existing = await _tourCompanyRepo.GetAsync(id);
            if (existing == null) return NotFound();

            _mapper.Map(dto, existing);
            await _tourCompanyRepo.Update(existing);
            return NoContent();
        }

        // DELETE: api/TourCompany/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTourCompany(int id)
        {
            var existing = await _tourCompanyRepo.GetAsync(id);
            if (existing == null) return NotFound();

            await _tourCompanyRepo.Delete(existing);
            return NoContent();
        }
    }
}