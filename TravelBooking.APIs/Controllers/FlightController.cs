using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelBooking.APIs.Dtos.Flight;
using TravelBooking.Core.Models;
using TravelBooking.Core.Repository.Contract;
using TravelBooking.Core.Specifications.FlightSpecs;
using TravelBooking.Helper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TravelBooking.APIs.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FlightController : Controller
    {
        private readonly IGenericRepository<Flight> flightRepository;
        private readonly IMapper mapper;

        public FlightController(IGenericRepository<Flight> flightRepository , IMapper mapper)
        {
            this.flightRepository = flightRepository;
            this.mapper = mapper;
        }
        // GET: FlightController
        [HttpGet]
        public async Task<ActionResult<Pagination<FlightDTO>>> GetAllFlights( [FromQuery]FlightSpecParams specParams )
        {
           var spec =new FlightSpecs(specParams);
            var flights = await flightRepository.GetAllWithSpecAsync(spec);
            if (flights == null || !flights.Any())
            {
                return NotFound("No flights found.");
            }
            foreach (var flight in flights)
            {
                Console.WriteLine(flight.FlightCompany?.Name); // null ولا موجود؟
            }
            var flightDTOs = mapper.Map<IReadOnlyList<FlightDTO>>(flights);
            var countSpec = new FlightCountSpec(specParams);
            var totalCount = await flightRepository.GetCountAsync(countSpec);
            return Ok(new Pagination<FlightDTO>(specParams.PageIndex, specParams.PageSize, totalCount, flightDTOs));
        }
        // GET: FlightController/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FlightDetialsDTO>> GetFlight( int id )
        {
            var spec = new FlightSpecs(id);
            var flight = await flightRepository.GetWithSpecAsync(spec);
            if (flight == null)
            {
                return NotFound($"Flight with ID {id} not found.");
            }
            var flightDTO = mapper.Map<FlightDetialsDTO>(flight);
            return Ok(flightDTO);
        }
        [HttpPost]
        public async Task<ActionResult<Flight>> AddFlight(Flight flight)
        {
            var newCompany = await flightRepository.AddAsync(flight);
            return Ok(newCompany);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateFlight(int id, Flight company)
        {
            if (company.Id != id) return BadRequest("ID mismatch");

            await flightRepository.Update(company);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFlight(int id)
        {
            var existing = await flightRepository.GetAsync(id);
            if (existing == null) return NotFound();

            await flightRepository.Delete(existing);
            return NoContent();
        }
    }
}
