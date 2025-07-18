using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelBooking.APIs.Helper;
using TravelBooking.Core.DTOS;
using TravelBooking.Core.Models;
using TravelBooking.Core.Repository.Contract;
using TravelBooking.Core.Specifications;
using TravelBooking.Helper;

namespace TravelBooking.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelCompanyController : ControllerBase
    {
        private readonly IGenericRepository<HotelCompany> _hotelRepo;
        private readonly IMapper _mapper;

        public HotelCompanyController(IGenericRepository<HotelCompany> hotelRepo, IMapper mapper)
        {
            _hotelRepo = hotelRepo;
            _mapper = mapper;
        }

        // Get All with Search + Sort + Paging
        [HttpGet]
        public async Task<ActionResult<Pagination<HotelCompanyReadDTO>>> GetAll([FromQuery] HotelCompanySpecParams specParams)
        {
            var spec = new HotelCompanyWithRoomsSpecification(specParams);
            var countSpec = new HotelCompanyWithFiltersForCountSpecification(specParams);

            var totalItems = await _hotelRepo.GetCountAsync(countSpec);
            var hotels = await _hotelRepo.GetAllWithSpecAsync(spec);
            var data = _mapper.Map<IReadOnlyList<HotelCompanyReadDTO>>(hotels);

            return Ok(new Pagination<HotelCompanyReadDTO>(specParams.PageIndex, specParams.PageSize, totalItems, data));
        }

        //  Get By Id
        [HttpGet("{id}")]
        public async Task<ActionResult<HotelCompanyReadDTO>> GetById(int id)
        {
            var spec = new HotelCompanyWithRoomsSpecification(id);
            var hotel = await _hotelRepo.GetWithSpecAsync(spec);
            if (hotel == null) return NotFound();

            return Ok(_mapper.Map<HotelCompanyReadDTO>(hotel));
        }

        // Create
        [HttpPost]
        public async Task<ActionResult> Create([FromForm] HotelCompanyCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var model = _mapper.Map<HotelCompany>(dto);
            await _hotelRepo.AddAsync(model);
            return Ok(model);
        }

        //Update
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromForm] HotelCompanyUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != dto.Id)
                return BadRequest("ID mismatch");

            var hotel = await _hotelRepo.GetAsync(id);
            if (hotel == null) return NotFound();

            _mapper.Map(dto, hotel);
          _hotelRepo.Update(hotel);
            return Ok(hotel);
        }

        // Delete
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var hotel = await _hotelRepo.GetAsync(id);
            if (hotel == null) return NotFound();

             _hotelRepo.Delete(hotel);
            return Ok();
        }
    }
}


