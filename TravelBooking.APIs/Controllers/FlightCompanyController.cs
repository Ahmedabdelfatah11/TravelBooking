using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelBooking.APIs.DTOS.Flight;
using TravelBooking.APIs.DTOS.FlightCompany;
using TravelBooking.APIs.DTOS.TourCompany;
using TravelBooking.Core.Models;
using TravelBooking.Core.Repository.Contract;
using TravelBooking.Core.Specifications.FlightSpecs;
namespace TravelBooking.APIs.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class FlightCompanyController : ControllerBase
    {
        private readonly IGenericRepository<FlightCompany> _flightComRepository;
        private readonly IMapper _mapper;

        public FlightCompanyController(IGenericRepository<FlightCompany> flightComRepository,IMapper mapper)
        {
            _flightComRepository = flightComRepository;
            _mapper = mapper;
        }

        // GET: FlightCompanyController
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<FlightCompanyDTO>>> GetAllFlightsCompanies()
        {
            var spec = new FlightWithCompanySpecs();
            var Companies = await _flightComRepository.GetAllWithSpecAsync(spec);
            if (Companies == null || !Companies.Any())
            {
                return NotFound("No flight companies found.");
            }
            var companyDTOs = _mapper.Map<IReadOnlyList<FlightCompanyDTO>>(Companies);
            return Ok(companyDTOs);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<FlightDetailsDTO>> GetFlightCompany(int id)
        {
            var spec = new FlightWithCompanySpecs(id);
            var Company = await _flightComRepository.GetWithSpecAsync(spec);
            var companyDTO = _mapper.Map<FlightDetailsDTO>(Company);
            return Ok(companyDTO);
        }
        [HttpPost]
        public async Task<ActionResult<FlightCompany>> AddFlightCompany(FlightCompanyDTO flight)
        {
            var entity = _mapper.Map<FlightCompany>(flight);

            var newCompany = await _flightComRepository.AddAsync(entity);
            var resultDto = _mapper.Map<FlightCompanyDTO>(newCompany);
            return CreatedAtAction(nameof(GetFlightCompany), new { id = newCompany.Id }, resultDto);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateFlightCompany(int id, FlightCompany company)
        {
            if (company.Id != id) return BadRequest("ID mismatch");

            await _flightComRepository.Update(company);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFlightCompany(int id)
        {
            var existing = await _flightComRepository.GetAsync(id);
            if (existing == null) return NotFound();

            await _flightComRepository.Delete(existing);
            return NoContent();
        }

    }
}
