using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelBooking.Core.Models;
using TravelBooking.Core.Repository.Contract;
using TravelBooking.Core.Specs;
namespace TravelBooking.APIs.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class FlightCompanyController : ControllerBase
    {
        private readonly IGenericRepository<FlightCompany> flightComRepository;

        public FlightCompanyController(IGenericRepository<FlightCompany> flightComRepository)
        {
            this.flightComRepository = flightComRepository;
        }

        // GET: FlightCompanyController
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<FlightCompany>>> GetAllCompanies()
        {
            var spec = new FlightWithCompanySpecs();
            var Companies = await flightComRepository.GetAllWithSpecAsync(spec);
            return Ok(Companies);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<FlightCompany>> GetCompany(int id)
        {
            var spec = new FlightWithCompanySpecs(id);
            var Company = await flightComRepository.GetAllWithSpecAsync(spec);
            return Ok(Company);
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
