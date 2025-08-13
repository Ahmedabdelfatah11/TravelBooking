using Microsoft.AspNetCore.Mvc;
using TravelBooking.Core.Models;
using TravelBooking.Core.Repository.Contract;
using TravelBooking.APIs.DTOS.Tours;
using AutoMapper;
using TravelBooking.APIs.DTOS.TourTickets;

namespace TravelBooking.APIs.Controllers
{
    [ApiController]
    [Route("api/tourtickets")]
    public class TourTicketsController : ControllerBase
    {
        private readonly IGenericRepository<TourTicket> _ticketRepo;
        private readonly IMapper _mapper;

        public TourTicketsController(IGenericRepository<TourTicket> ticketRepo, IMapper mapper)
        {
            _ticketRepo = ticketRepo;
            _mapper = mapper;
        }

        // GET: api/tourtickets/bytour/5
        [HttpGet("bytour/{tourId}")]
        public async Task<ActionResult<IEnumerable<TourTicketDto>>> GetTicketsByTour(int tourId)
        {
            var tickets = await _ticketRepo.GetAllAsync(t => t.TourId == tourId && t.IsActive);
            var result = _mapper.Map<IEnumerable<TourTicketDto>>(tickets);
            return Ok(result);
        }

        // GET: api/tourtickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TourTicketDto>> GetTicketById(int id)
        {
            var ticket = await _ticketRepo.GetAsync(id);
            if (ticket == null) return NotFound();

            var dto = _mapper.Map<TourTicketDto>(ticket);
            return Ok(dto);
        }

        // POST: api/tourtickets
        [HttpPost]
        public async Task<ActionResult<TourTicketDto>> CreateTicket(TourTicketCreateDto tourTicketCreate)
        {
            var ticket = _mapper.Map<TourTicket>(tourTicketCreate);
            await _ticketRepo.AddAsync(ticket);

            var result = _mapper.Map<TourTicketDto>(ticket);
            return CreatedAtAction(nameof(GetTicketById), new { id = ticket.Id }, result);
        }

        // PUT: api/tourtickets/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTicket(int id, TourTicketCreateDto tourTicketUpdate)
        {
            var existing = await _ticketRepo.GetAsync(id);
            if (existing == null) return NotFound();

            _mapper.Map(tourTicketUpdate, existing);
            await _ticketRepo.Update(existing);
            return NoContent();
        }

        // DELETE: api/tourtickets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var ticket = await _ticketRepo.GetAsync(id);
            if (ticket == null) return NotFound();

            await _ticketRepo.Delete(ticket);
            return NoContent();
        }
    }
}