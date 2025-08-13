using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelBooking.APIs.DTOS.Flight;
using TravelBooking.APIs.DTOS.FlightCompany;
using TravelBooking.APIs.DTOS.TourCompany;
using TravelBooking.Core.DTOS.CarRentalCompanies;
using TravelBooking.Core.Models;
using TravelBooking.Core.Repository.Contract;
using TravelBooking.Core.Specifications.FlightSpecs;
using TravelBooking.Service.Services.Dashboard;
namespace TravelBooking.APIs.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,FlightAdmin")]
    public class FlightCompanyController : ControllerBase
    {
        private readonly IGenericRepository<FlightCompany> _flightComRepository;
        private readonly IMapper _mapper;
        private readonly IFlightAdminDashboardService _dashboardService;

        public FlightCompanyController(IGenericRepository<FlightCompany> flightComRepository,IMapper mapper, IFlightAdminDashboardService dashboardService)
        {
            _flightComRepository = flightComRepository;
            _mapper = mapper;
            _dashboardService = dashboardService;
        }

        // GET: FlightCompanyController
        [HttpGet]
        [AllowAnonymous]
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
        [AllowAnonymous]
        public async Task<ActionResult<FlightCompanyDetailsDTO>> GetFlightCompany(int id)
        {
            var spec = new FlightWithCompanySpecs(id);
            var Company = await _flightComRepository.GetWithSpecAsync(spec);
            var companyDTO = _mapper.Map<FlightCompanyDetailsDTO>(Company);
            return Ok(companyDTO);
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "SuperAdmin,FlightAdmin")]
        public async Task<ActionResult> UpdateFlightCompany(int id, FlightCompanyDTO company)
        {
            if (company.Id != id) return BadRequest("ID mismatch");

            var UpdatedCompany = _mapper.Map<FlightCompany>(company);
            var existingCompany = await _flightComRepository.GetAsync(id);
            if (existingCompany == null) return NotFound();

            var userId = User.FindFirst("uid")?.Value;
            var userRoles = User.FindAll("role").Select(r => r.Value);

            if (userRoles.Contains("FlightAdmin") && existingCompany.AdminId != userId)
                return Forbid("You are not authorized to update this company.");

            await _flightComRepository.Update(UpdatedCompany);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin,FlightAdmin")]
        public async Task<ActionResult> DeleteFlightCompany(int id)
        {
            var existingCompany = await _flightComRepository.GetAsync(id);
            if (existingCompany == null) return NotFound();

            var userId = User.FindFirst("uid")?.Value;
            var userRoles = User.FindAll("role").Select(r => r.Value);

            if (userRoles.Contains("FlightAdmin") && existingCompany.AdminId != userId)
                return Forbid("You are not authorized to delete this company.");

            await _flightComRepository.Delete(existingCompany);
            return NoContent();
        }


        [HttpGet("my-companies")]
        [Authorize(Roles = "SuperAdmin,FlightAdmin")]
        public async Task<ActionResult<IEnumerable<FlightCompanyDetailsDTO>>> GetMyCompanies()
        {
            var userId = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var companies = await _flightComRepository.GetFlighByAdminIdAsync(userId);
            var data = _mapper.Map<IReadOnlyList<FlightCompanyDetailsDTO>>(companies);

            return Ok(data);
        }


        [HttpGet("dashboard")]
        [Authorize(Roles = "FlightAdmin")]
        public async Task<IActionResult> GetDashboard()
        {
            var flightCompanyIdClaim = User.Claims.FirstOrDefault(c => c.Type == "FlightCompanyId")?.Value;

            if (string.IsNullOrEmpty(flightCompanyIdClaim) || !int.TryParse(flightCompanyIdClaim, out int flightCompanyId))
                return Unauthorized("FlightCompanyId not found in token.");

            var data = await _dashboardService.GetStatsForFlightCompany(flightCompanyId);
            return Ok(data);
        }

    }
}
