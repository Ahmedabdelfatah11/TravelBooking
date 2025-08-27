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
            var spec = new ToursSpecification(id);
            var existing = await _tourRepo.GetWithSpecAsync(spec);
            if (existing == null) return NotFound();

            // Validate TourCompanyId
            if (!dto.TourCompanyId.HasValue)
                return BadRequest(new { error = "TourCompanyId is required." });

            var companyExists = await _context.TourCompanies.AnyAsync(c => c.Id == dto.TourCompanyId.Value);
            if (!companyExists)
                return BadRequest(new { error = $"TourCompanyId {dto.TourCompanyId.Value} does not exist." });

            // Update scalar fields
            existing.Name = dto.Name;
            existing.StartDate = dto.StartDate;
            existing.EndDate = dto.EndDate;
            existing.Description = dto.Description;
            existing.Destination = dto.Destination;
            existing.MaxGuests = dto.MaxGuests;
            existing.MinGroupSize = dto.MinGroupSize;
            existing.MaxGroupSize = dto.MaxGroupSize;
            existing.Price = dto.Price;
            existing.Category = dto.Category;
            existing.Languages = dto.Languages;
            existing.TourCompanyId = dto.TourCompanyId;

            // Handle images (unchanged)
            if (dto.Image != null)
            {
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existing.ImageUrl?.TrimStart('/') ?? "");
                if (!string.IsNullOrEmpty(existing.ImageUrl) && System.IO.File.Exists(oldPath))
                    System.IO.File.Delete(oldPath);
                existing.ImageUrl = await SaveImageAsync(dto.Image);
            }

            if (dto.GalleryImages?.Any() == true)
            {
                var newUrls = await SaveImagesAsync(dto.GalleryImages);
                foreach (var url in newUrls)
                    existing.TourImages.Add(new TourImage { ImageUrl = url });
            }

            if (dto.DeletedImageUrls?.Any() == true)
            {
                var toDelete = existing.TourImages.Where(ti => dto.DeletedImageUrls.Contains(ti.ImageUrl)).ToList();
                foreach (var img in toDelete)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", img.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                    _context.TourImages.Remove(img);
                }
            }

            // ✅ Fix: Tickets
            if (dto.Tickets != null)
            {
                var ticketIds = new List<int>();
                foreach (var ticketDto in dto.Tickets)
                {
                    var ticket = existing.TourTickets.FirstOrDefault(t => t.Id == ticketDto.Id)
                               ?? new TourTicket();

                    _mapper.Map(ticketDto, ticket);
                    ticket.TourId = existing.Id;

                    if (ticket.Id == 0)
                        _context.TourTickets.Add(ticket);
                    else
                        _context.TourTickets.Update(ticket);

                    ticketIds.Add(ticket.Id);
                }

                var ticketsToRemove = _context.TourTickets.Where(t => t.TourId == existing.Id && !ticketIds.Contains(t.Id)).ToList();
                _context.TourTickets.RemoveRange(ticketsToRemove);
            }

            // ✅ Fix: Questions
            if (dto.Questions != null)
            {
                var questionIds = new List<int>();
                foreach (var qDto in dto.Questions)
                {
                    var question = existing.Questions.FirstOrDefault(q => q.Id == qDto.Id)
                                 ?? new TourQuestion();

                    _mapper.Map(qDto, question);
                    question.TourId = existing.Id;

                    if (question.Id == 0)
                        _context.TourQuestion.Add(question);
                    else
                        _context.TourQuestion.Update(question);

                    questionIds.Add(question.Id);
                }

                var questionsToRemove = _context.TourQuestion.Where(q => q.TourId == existing.Id && !questionIds.Contains(q.Id)).ToList();
                _context.TourQuestion.RemoveRange(questionsToRemove);
            }

            // Items
            if (dto.IncludedItems != null)
            {
                existing.IncludedItems.Clear();
                existing.IncludedItems.AddRange(dto.IncludedItems);
            }
            if (dto.ExcludedItems != null)
            {
                existing.ExcludedItems.Clear();
                existing.ExcludedItems.AddRange(dto.ExcludedItems);
            }

            // ✅ Final Save
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