using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelBooking.APIs.DTOs;
using TravelBooking.Core.Models;
using TravelBooking.Core.Repository.Contract;
using TravelBooking.Core.Specs;
namespace TravelBooking.APIs.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [EnableCors("AllowAnyUSer")]
    public class FlightCompanyController : ControllerBase
    {
        private readonly IGenericRepository<FlightCompany> flightComRepository;
        private readonly IMapper mapper;

        public FlightCompanyController(IGenericRepository<FlightCompany> flightComRepository,IMapper mapper)
        {
            this.flightComRepository = flightComRepository;
            this.mapper = mapper;
        }

        // GET: FlightCompanyController
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<FlightCompanyDTO>>> GetAllCompanies()
        {
            var spec = new FlightWithCompanySpecs();
            var Companies = await flightComRepository.GetAllWithSpecAsync(spec);
            if (Companies == null || !Companies.Any())
            {
                return NotFound("No flight companies found.");
            }
            var companyDTOs = mapper.Map<IReadOnlyList<FlightCompanyDTO>>(Companies);
            return Ok(companyDTOs);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<FlightCompanyDetailsDTO>> GetCompany(int id)
        {
            var spec = new FlightWithCompanySpecs(id);
            var Company = await flightComRepository.GetWithSpecAsync(spec);
            var companyDTO = mapper.Map<FlightCompanyDetailsDTO>(Company);
            return Ok(companyDTO);
        }
        [HttpPost]
        public async Task<ActionResult<FlightCompany>> AddCompany(FlightCompany flight)
        {
            var newCompany = await flightComRepository.AddAsync(flight);
            return Ok(newCompany);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, FlightCompany company)
        {
            if (company.Id != id) return BadRequest("ID mismatch");

            await flightComRepository.UpdateAsync(company);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await flightComRepository.DeleteAsync(id);
            return NoContent();
        }

    }
}
