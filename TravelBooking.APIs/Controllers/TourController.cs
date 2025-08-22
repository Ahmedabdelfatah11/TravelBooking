using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelBooking.Core.Models;

using TravelBooking.Helper;
using TravelBooking.Errors;
using TravelBooking.APIs.DTOS.Booking.TourBooking;
using Microsoft.AspNetCore.Authorization;
using TravelBooking.Core.Repository.Contract;
using AutoMapper;
using TravelBooking.Core.Specifications.TourSpecs;
using TravelBooking.APIs.DTOS.Tours;
using TravelBooking.APIs.DTOS.TourCompany;
using Microsoft.EntityFrameworkCore;
using TravelBooking.Repository.Data;
using TravelBooking.APIs.Extensions;

namespace TravelBooking.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,TourAdmin")]
    public class TourController : ControllerBase
    {
        private readonly IGenericRepository<Tour> _tourRepo;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Booking> _bookingRepo;
        private readonly IGenericRepository<TourCompany> _tourCompany;
        private readonly AppDbContext _context;

        public TourController(
            IGenericRepository<Tour> tourRepo,
            IMapper mapper,
             IGenericRepository<Booking> bookingRepo,
             IGenericRepository<TourCompany> tourCompany, AppDbContext context)
        {
            _tourRepo = tourRepo;
            _mapper = mapper;
            _bookingRepo = bookingRepo;
            _tourCompany = tourCompany;
            _context = context;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<Pagination<TourReadDto>>> GetTour([FromQuery] TourSpecParams specParams)
        {
            var spec = new ToursSpecification(specParams);

            var tours = await _tourRepo.GetAllWithSpecAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Tour>, IReadOnlyList<TourReadDto>>(tours);

            var countSpec = new ToursWithFilterationForCountSpec(specParams);
            var count = await _tourRepo.GetCountAsync(countSpec);
            var baseQuery = _context.Tours.AsQueryable().ApplyFiltering(specParams);
            var (minPrice, maxPrice) = await baseQuery.GetPriceBoundsAsync();

            var priceBounds = new { min = minPrice, max = maxPrice };

            return Ok(new
            {
                pagination = new Pagination<TourReadDto>(specParams.PageIndex, specParams.PageSize, count, data),
                priceBounds = priceBounds
            });
        }

        [ProducesResponseType(typeof(TourCompanyReadDto), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ApiResponse))]
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<TourReadDto>> GetTourById(int id)
        {
            var spec = new ToursSpecification(id);

            var tour = await _tourRepo.GetWithSpecAsync(spec);

            if (tour is null)
                return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<Tour, TourReadDto>(tour));
        }


        [HttpPost]
        [Authorize(Roles = "SuperAdmin,TourAdmin")]
        public async Task<ActionResult<TourReadDto>> CreateTour([FromForm] TourCreateDto dto)
        {
            var entity = _mapper.Map<Tour>(dto);

            if (dto.Image != null)
            {
                entity.ImageUrl = await SaveImageAsync(dto.Image);
            }

            if (dto.GalleryImages != null && dto.GalleryImages.Any())
            {
                var imageUrls = await SaveImagesAsync(dto.GalleryImages);
                foreach (var url in imageUrls)
                {
                    entity.TourImages.Add(new TourImage { ImageUrl = url });
                }
            }

            var result = await _tourRepo.AddAsync(entity);

            if (dto.Tickets != null && dto.Tickets.Any())
            {
                foreach (var ticketDto in dto.Tickets)
                {
                    var ticket = _mapper.Map<TourTicket>(ticketDto);
                    ticket.TourId = result.Id;
                    await _context.TourTickets.AddAsync(ticket);
                }
                await _context.SaveChangesAsync();
            }

            result.TourCompany = await _tourCompany.GetAsync(dto.TourCompanyId.Value);

            var resultDto = _mapper.Map<TourReadDto>(result);
            return CreatedAtAction(nameof(GetTourById), new { id = result.Id }, resultDto);
        }

        //[Authorize]
        [HttpPost("{serviceId}/book")]
        [AllowAnonymous]
        //[Authorize(Roles = "SuperAdmin,TourAdmin,User")]
        public async Task<IActionResult> BookTour(int serviceId, [FromBody] TourBookingDto dto)
        {
            var userId = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token.");

            var tour = await _tourRepo.GetAsync(serviceId);
            if (tour == null) return NotFound(new ApiResponse(404));

            if (tour.StartDate >= tour.EndDate)
                return BadRequest("Invalid tour date range.");

            //var existingBookings = await _bookingRepo.GetAllAsync(b =>
            //    b.TourId == serviceId &&
            //    b.Status != Status.Cancelled &&
            //    b.StartDate < tour.EndDate &&
            //    tour.StartDate < b.EndDate
            //);
            //if (existingBookings.Any())
            //    return BadRequest("Tour already booked in selected time frame.");

            decimal totalPrice = 0;
            var bookingTickets = new List<TourBookingTicket>();

            foreach (var ticketRequest in dto.Tickets)
            {
                var ticket = await _context.TourTickets
                    .FirstOrDefaultAsync(t =>
                        t.TourId == serviceId &&
                        t.Type == ticketRequest.Type &&
                        t.IsActive);

                if (ticket == null)
                    return BadRequest($"Ticket type '{ticketRequest.Type}' not found or sold out.");

                if (ticket.AvailableQuantity < ticketRequest.Quantity)
                    return BadRequest($"Only {ticket.AvailableQuantity} '{ticket.Type}' tickets left.");

                totalPrice += ticket.Price * ticketRequest.Quantity;

                //ticket.AvailableQuantity -= ticketRequest.Quantity;

                //if (ticket.AvailableQuantity == 0)
                //    ticket.IsActive = false;

                //_context.TourTickets.Update(ticket);

                bookingTickets.Add(new TourBookingTicket
                {
                    TicketId = ticket.Id,
                    Quantity = ticketRequest.Quantity
                });
            }
            var booking = new Booking
            {
                UserId = userId,
                TourId = serviceId,
                StartDate = tour.StartDate,
                EndDate = tour.EndDate,
                BookingType = BookingType.Tour,
                Status = Status.Pending,
                BookingTickets = bookingTickets,
            };

            await _bookingRepo.AddAsync(booking);
            await _context.SaveChangesAsync();

            var fullBooking = await _context.Bookings
                .Include(b => b.BookingTickets)
                .ThenInclude(bt => bt.Ticket)
                .Include(b => b.Tour)
                .FirstOrDefaultAsync(b => b.Id == booking.Id);

            var result = _mapper.Map<TourBookingResultDto>(fullBooking);
            result.TotalPrice = fullBooking.TotalPrice;

            return CreatedAtAction("GetBookingById", "Booking", new { id = booking.Id }, result);
        }




        [HttpPut("{id}")]
        [Authorize(Roles = "SuperAdmin,TourAdmin")]
        public async Task<ActionResult> UpdateTour(int id, [FromForm] TourUpdateDto dto)
        {
            var existing = await _tourRepo.GetWithSpecAsync(new ToursSpecification(id));
            if (existing == null) return NotFound();

            _mapper.Map(dto, existing);

            // رفع صورة رئيسية جديدة
            if (dto.Image != null)
            {
                existing.ImageUrl = await SaveImageAsync(dto.Image);
            }

            // رفع صور إضافية جديدة
            if (dto.GalleryImages != null && dto.GalleryImages.Any())
            {
                var newImageUrls = await SaveImagesAsync(dto.GalleryImages);
                foreach (var url in newImageUrls)
                {
                    existing.TourImages.Add(new TourImage { ImageUrl = url });
                }
            }

            await _tourRepo.Update(existing);
            return NoContent();
        }

        private async Task<string> SaveImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Invalid file");

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "tours");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(folderPath, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/images/tours/{fileName}";
        }
        private async Task<List<string>> SaveImagesAsync(List<IFormFile>? files)
        {
            var urls = new List<string>();

            if (files == null || !files.Any()) return urls;

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "tours");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            foreach (var file in files)
            {
                if (file.Length == 0) continue;

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(folderPath, fileName);

                await using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);

                urls.Add($"/images/tours/{fileName}");
            }

            return urls;
        }
        // DELETE: api/TourCompany/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin,TourAdmin,User")]
        public async Task<ActionResult> DeleteTour(int id)
        {
            var existing = await _tourRepo.GetAsync(id);
            if (existing == null) return NotFound();

            await _tourRepo.Delete(existing);
            return NoContent();
        }

    }
}