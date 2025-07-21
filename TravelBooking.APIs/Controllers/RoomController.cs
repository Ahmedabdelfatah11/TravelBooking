﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelBooking.APIs.Dtos.Rooms;
using TravelBooking.APIs.Helper;
using TravelBooking.Core.Models;
using TravelBooking.Core.Repository.Contract;
using TravelBooking.Core.Specifications;
using TravelBooking.Core.Specifications.RoomSpecs;
using TravelBooking.Helper;

namespace TravelBooking.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IGenericRepository<Room> _roomRepo;
        private readonly IGenericRepository<RoomImage> _roomImageRepo;
        private readonly IGenericRepository<HotelCompany> _hotelRepo;
        private readonly IMapper _mapper;

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
                              IMapper mapper)
        {
            _roomRepo = roomRepo;
            _roomImageRepo = roomImageRepo;
            _hotelRepo = hotelRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all rooms with filtering, pagination, and images
        /// </summary>
        /// <param name="specParams">Room specification parameters for filtering and pagination</param>
        /// <returns>Paginated list of rooms</returns>
        [HttpGet]
        public async Task<ActionResult<Pagination<RoomToReturnDTO>>> GetRooms([FromQuery] RoomSpecParams specParams)
        {
            var spec = new RoomSpecification(specParams);
            var countSpec = new RoomWithFiltersForCountSpecification(specParams);

            var totalItems = await _roomRepo.GetCountAsync(countSpec);
            var rooms = await _roomRepo.GetAllWithSpecAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Room>, IReadOnlyList<RoomToReturnDTO>>(rooms);

            return Ok(new Pagination<RoomToReturnDTO>(specParams.PageIndex, specParams.PageSize, totalItems, data));
        }

        /// <summary>
        /// Get room by ID
        /// </summary>
        /// <param name="id">The ID of the room to retrieve</param>
        /// <returns>Room details with hotel and images</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomToReturnDTO>> GetRoom(int id)
        {
            var spec = new RoomWithHotelAndImagesSpecification(id);
            var room = await _roomRepo.GetWithSpecAsync(spec);

            if (room == null) return NotFound();

            return Ok(_mapper.Map<Room, RoomToReturnDTO>(room));
        }

        /// <summary>
        /// Create a new room
        /// </summary>
        /// <param name="roomDto">Room creation data</param>
        /// <returns>Created room</returns>
        [HttpPost]
        public async Task<ActionResult<RoomToReturnDTO>> CreateRoom([FromBody] RoomCreateDTO roomDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if hotel exists
            var hotelExists = await _hotelRepo.GetAsync(roomDto.HotelCompanyId);
            if (hotelExists == null)
                return BadRequest("Hotel company not found");

            var room = _mapper.Map<RoomCreateDTO, Room>(roomDto);
            room.HotelId = roomDto.HotelCompanyId;

            // Handle room images
            if (roomDto.RoomImages != null && roomDto.RoomImages.Any())
            {
                room.Images = _mapper.Map<List<RoomImage>>(roomDto.RoomImages);
            }

            await _roomRepo.AddAsync(room);

            var result = _mapper.Map<Room, RoomToReturnDTO>(room);
            return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, result);
        }

        /// <summary>
        /// Update existing room
        /// </summary>
        /// <param name="id">Room ID</param>
        /// <param name="roomDto">Room update data</param>
        /// <returns>No content</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateRoom(int id, [FromBody] RoomUpdateDTO roomDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //if (id != roomDto.Id)
            //    return BadRequest("ID mismatch");

            var room = await _roomRepo.GetAsync(id);
            if (room == null) return NotFound();

            // Check if hotel exists
            var hotelExists = await _hotelRepo.GetAsync(roomDto.HotelCompanyId);
            if (hotelExists == null)
                return BadRequest("Hotel company not found");

            _mapper.Map(roomDto, room);
             _roomRepo.Update(room);

            return NoContent();
        }

        /// <summary>
        /// Delete room
        /// </summary>
        /// <param name="id">Room ID</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRoom(int id)
        {
            var room = await _roomRepo.GetAsync(id);
            if (room == null) return NotFound();

             _roomRepo.Delete(room);
            return NoContent();
        }
    }
}
