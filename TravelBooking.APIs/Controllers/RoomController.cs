
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

using TravelBooking.APIs.DTOS.Booking.RoomBooking;
using TravelBooking.APIs.DTOS.Rooms;
using TravelBooking.APIs.Helper;
using TravelBooking.Core.Models;
using TravelBooking.Core.Repository.Contract;
using TravelBooking.Core.Specifications;
using TravelBooking.Core.Specifications.RoomSpecs;
using TravelBooking.Errors;
using TravelBooking.Helper;
using static TravelBooking.Service.Services.RoomService;

namespace TravelBooking.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "HotelAdmin")]
    public class RoomController : ControllerBase
    {
        private readonly IGenericRepository<Room> _roomRepo;
        private readonly IGenericRepository<RoomImage> _roomImageRepo;
        private readonly IGenericRepository<HotelCompany> _hotelRepo;
        private readonly IGenericRepository<Booking> _bookingRepo;
        private readonly IMapper _mapper;
        private readonly IRoomService _roomService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomController"/> class.
        /// </summary>
        /// <param name="roomRepo">The repository for rooms.</param>
        /// <param name="roomImageRepo">The repository for room images.</param>
        /// <param name="hotelRepo">The repository for hotel companies.</param>
        /// <param name="mapper">The AutoMapper instance for mapping between DTOs and models.</param>
        public RoomController(IGenericRepository<Room> roomRepo,
                              IGenericRepository<RoomImage> roomImageRepo,
                              IGenericRepository<HotelCompany> hotelRepo,
                                IGenericRepository<Booking> bookingRepo,
                              IMapper mapper,
                              IRoomService roomService,
                              IHttpContextAccessor httpContextAccessor)

        {
            _roomRepo = roomRepo;
            _roomImageRepo = roomImageRepo;
            _hotelRepo = hotelRepo;
            _bookingRepo = bookingRepo;
            _mapper = mapper;
            _roomService = roomService;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Get all rooms with filtering, pagination, and images
        /// </summary>
        /// <param name="specParams">Room specification parameters for filtering and pagination</param>
        /// <returns>Paginated list of rooms</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<Pagination<RoomToReturnDTO>>> GetRooms([FromQuery] RoomSpecParams specParams)
        {
            var spec = new RoomSpecification(specParams);
            var countSpec = new RoomWithFiltersForCountSpecification(specParams);

            var totalItems = await _roomRepo.GetCountAsync(countSpec);
            var rooms = await _roomRepo.GetAllWithSpecAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Room>, IReadOnlyList<RoomToReturnDTO>>(rooms);

            return Ok(new Pagination<RoomToReturnDTO>(specParams.PageIndex, specParams.PageSize, totalItems, data));
        }


        [HttpGet("{roomId}/available-dates")]
        [AllowAnonymous]
        public async Task<ActionResult<List<DateRange>>> GetAvailableDates(
            int roomId,
            [FromQuery] DateTime start,
            [FromQuery] DateTime end)
        {
            var ranges = await _roomService.GetAvailableDateRanges(roomId, start.ToLocalTime(), end.ToLocalTime());
            return Ok(ranges);
        }
        /// <summary>
        /// Get room by ID
        /// </summary>
        /// <param name="id">The ID of the room to retrieve</param>
        /// <returns>Room details with hotel and images</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<RoomToReturnDTO>> GetRoom(int id)
        {
            var spec = new RoomWithHotelAndImagesSpecification(id);
            var room = await _roomRepo.GetWithSpecAsync(spec);

            if (room == null) return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<Room, RoomToReturnDTO>(room));
        }


        //[Authorize]
        [HttpPost("{serviceId}/book")]
        //[Authorize(Roles = "SuperAdmin,User,HotelAdmin")]
        [AllowAnonymous]
        public async Task<IActionResult> BookRoom(int serviceId, [FromBody] RoomBookingDto dto)
        {
            var userId = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token.");

            var room = await _roomRepo.GetAsync(serviceId);
            if (room == null) return NotFound();

            if (dto.StartDate >= dto.EndDate)
                return BadRequest("Start date must be before end date.");

            var overlapping = await _bookingRepo.GetAllAsync(b =>
                b.RoomId == serviceId &&
                b.Status != Status.Cancelled &&
                b.StartDate < dto.EndDate &&
                dto.StartDate < b.EndDate
            );
            if (overlapping.Any())
                return BadRequest("Room already booked for selected dates.");

            var booking = _mapper.Map<Booking>(dto);
            booking.RoomId = serviceId;
            booking.UserId = userId;
            booking.BookingType = BookingType.Room;
            booking.Status = Status.Pending;



            await _bookingRepo.AddAsync(booking);


            booking.Room = room;

            var result = _mapper.Map<RoomBookingResultDto>(booking);
            //booking.Payment.Amount = result.TotalPrice;
            return CreatedAtAction("GetBookingById", "Booking", new { id = booking.Id }, result);
        }

        /// <summary>
        /// Create a new room
        /// </summary>
        /// <param name="roomDto">Room creation data</param>
        /// <returns>Created room</returns>
        [HttpPost]
        [Authorize(Roles = "SuperAdmin,HotelAdmin")]
        public async Task<ActionResult<RoomToReturnDTO>> CreateRoom([FromForm] RoomCreateDTO roomDto)
        {
            var hotel = await _hotelRepo.GetAsync(roomDto.HotelCompanyId);
            if (hotel == null)
                return BadRequest("Hotel not found");

            var room = _mapper.Map<Room>(roomDto);
            room.HotelId = roomDto.HotelCompanyId;

            if (roomDto.RoomImages != null && roomDto.RoomImages.Any())
            {
                var imageUrls = await SaveImagesAsync(roomDto.RoomImages);
                foreach (var url in imageUrls)
                {
                    room.Images.Add(new RoomImage { ImageUrl = url });
                }
            }

            await _roomRepo.AddAsync(room);

            var result = _mapper.Map<RoomToReturnDTO>(room);
            return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, result);
        }
        //private async Task<string> SaveRoomImageAsync(IFormFile image)
        //{
        //    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "rooms");
        //    if (!Directory.Exists(folderPath))
        //        Directory.CreateDirectory(folderPath);

        //    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
        //    var filePath = Path.Combine(folderPath, fileName);

        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await image.CopyToAsync(stream);
        //    }

        //    return $"/images/rooms/{fileName}";
        //}
        /// <summary>
        /// Update existing room
        /// </summary>
        /// <param name="id">Room ID</param>
        /// <param name="roomDto">Room update data</param>
        /// <returns>No content</returns>

        // UpdateRoom method in RoomController.cs
        // Replace your existing UpdateRoom method with this:

        [HttpPut("{id}")]
        [Authorize(Roles = "SuperAdmin,HotelAdmin")]
        public async Task<ActionResult> UpdateRoom(int id, [FromForm] RoomUpdateDTO roomDto)
        {
            var room = await _roomRepo.GetAsync(id);
            if (room == null) return NotFound(new ApiResponse(404, "Room not found"));

            // Validate hotel exists
            if (roomDto.HotelCompanyId.HasValue)
            {
                var hotel = await _hotelRepo.GetAsync(roomDto.HotelCompanyId.Value);
                if (hotel == null)
                    return BadRequest(new ApiResponse(400, "Hotel company not found"));
            }

            // Update room properties
            if (!string.IsNullOrEmpty(roomDto.RoomType))
                // Replace this block in UpdateRoom method:
                // if (!string.IsNullOrEmpty(roomDto.RoomType))
                //     room.RoomType = roomDto.RoomType;

                // With this:
                if (!string.IsNullOrEmpty(roomDto.RoomType))
                {
                    if (Enum.TryParse<RoomType>(roomDto.RoomType, true, out var parsedRoomType))
                        room.RoomType = parsedRoomType;
                    else
                        return BadRequest(new ApiResponse(400, $"Invalid RoomType: {roomDto.RoomType}"));
                }
                //room.RoomType = roomDto.RoomType;

            if (roomDto.Price > 0)
                room.Price = roomDto.Price;

            if (roomDto.IsAvailable.HasValue)
                room.IsAvailable = roomDto.IsAvailable.Value;

            if (roomDto.HotelCompanyId.HasValue)
                room.HotelId = roomDto.HotelCompanyId.Value;

            // Handle image updates if new images are provided
            if (roomDto.RoomImages != null && roomDto.RoomImages.Any())
            {
                var newImageUrls = await SaveImagesAsync(roomDto.RoomImages);

                // Clear existing images and add new ones
                room.Images.Clear();
                foreach (var url in newImageUrls)
                {
                    room.Images.Add(new RoomImage { ImageUrl = url, RoomId = room.Id });
                }
            }

            await _roomRepo.Update(room);

            return NoContent();
        }

        /// <summary>
        /// Delete room
        /// </summary>
        /// <param name="id">Room ID</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin,HotelAdmin,User")]
        public async Task<ActionResult> DeleteRoom(int id)
        {
            var room = await _roomRepo.GetAsync(id);
            if (room == null) return NotFound();

            await _roomRepo.Delete(room);
            return NoContent();
        }

        private async Task<List<string>> SaveImagesAsync(List<IFormFile>? files)
        {
            var urls = new List<string>();

            if (files == null || !files.Any())
                return urls;

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "rooms");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            foreach (var file in files)
            {
                if (file.Length == 0) continue;

                var extension = Path.GetExtension(file.FileName).ToLower();
                if (extension != ".jpg" && extension != ".jpeg" && extension != ".png" && extension != ".webp")
                    throw new ArgumentException($"Invalid file type: {file.FileName}. Only jpg, jpeg, png, webp are allowed.");

                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(folderPath, fileName);

                await using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);
                var request = _httpContextAccessor.HttpContext?.Request;
                if (request == null)
                    throw new InvalidOperationException("HttpContext is not available.");
                urls.Add($"{request.Scheme}://{request.Host}/images/rooms/{fileName}");
            }

            return urls;
        }
    }
}




