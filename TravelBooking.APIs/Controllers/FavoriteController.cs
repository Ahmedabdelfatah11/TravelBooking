using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TravelBooking.APIs.DTOS.Favoritet;
using TravelBooking.Core.Models;
using TravelBooking.Repository.Data;

namespace TravelBooking.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,User")]

    public class FavoriteController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<FavoriteController> _logger;

        public FavoriteController(AppDbContext context, IMapper mapper, ILogger<FavoriteController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get all favorites for the current user
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "SuperAdmin,User")]
        public async Task<ActionResult<IEnumerable<FavoritetDto>>> GetUserFavorites(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var userId = GetUserId();
                if (userId == null) return Unauthorized("User not authenticated");

                var query = _context.Favorites
                    .Include(f => f.User)
                    .Include(f => f.HotelCompany)
                    .Include(f => f.FlightCompany)
                    .Include(f => f.CarRentalCompany)
                    .Include(f => f.TourCompany)
                    .Include(f => f.Tour)
                    .Where(f => f.UserId == userId)
                    .OrderByDescending(f => f.CreatedAt)
                    .AsNoTracking();

                var totalCount = await query.CountAsync();
                var favorites = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var favoriteDtos = _mapper.Map<List<FavoritetDto>>(favorites);

                Response.Headers.Add("X-Total-Count", totalCount.ToString());
                Response.Headers.Add("X-Page", page.ToString());
                Response.Headers.Add("X-Page-Size", pageSize.ToString());

                return Ok(favoriteDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user favorites for user {UserId}", GetUserId());
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get user favorites by company type
        /// </summary>
        [HttpGet("type/{companyType}")]
        [Authorize(Roles = "SuperAdmin,User")]
        public async Task<ActionResult<IEnumerable<FavoritetDto>>> GetUserFavoritesByType(
            string companyType,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var userId = GetUserId();
                if (userId == null) return Unauthorized("User not authenticated");

                if (!IsValidCompanyType(companyType))
                    return BadRequest($"Invalid company type: {companyType}. Valid types are: hotel, flight, carrental, tour");

                var query = _context.Favorites
                    .Include(f => f.User)
                    .Include(f => f.HotelCompany)
                    .Include(f => f.FlightCompany)
                    .Include(f => f.CarRentalCompany)
                    .Include(f => f.TourCompany)
                    .Where(f => f.UserId == userId && f.CompanyType.ToLower() == companyType.ToLower())
                    .OrderByDescending(f => f.CreatedAt)
                    .AsNoTracking();

                var totalCount = await query.CountAsync();
                var favorites = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var favoriteDtos = _mapper.Map<List<FavoritetDto>>(favorites);

                Response.Headers.Add("X-Total-Count", totalCount.ToString());

                return Ok(favoriteDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user favorites by type {CompanyType} for user {UserId}",
                    companyType, GetUserId());
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Add a company to favorites
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "SuperAdmin,User")]
        public async Task<ActionResult<FavoritetDto>> AddFavorite([FromBody] CreateFavoriteDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!createDto.IsValid())
                    return BadRequest("Please provide exactly one company ID");

                if (!IsValidCompanyType(createDto.CompanyType))
                    return BadRequest($"Invalid company type: {createDto.CompanyType}");

                var userId = GetUserId();
                if (userId == null) return Unauthorized("User not authenticated");

                // Check if favorite already exists
                var existingFavorite = await CheckFavoriteExists(userId, createDto);
                if (existingFavorite)
                    return Conflict("Company already in favorites");

                // Verify company exists
                var companyExists = await VerifyCompanyExists(createDto);
                if (!companyExists)
                    return NotFound("Company not found");

                var favorite = _mapper.Map<Favoritet>(createDto);
                favorite.UserId = userId;

                _context.Favorites.Add(favorite);

                await _context.SaveChangesAsync();

                // Retrieve the favorite with includes for response
                var result = await GetFavoriteWithIncludes(favorite.Id);
                if (result == null)
                    return StatusCode(500, "Failed to retrieve created favorite");

                var favoriteDto = _mapper.Map<FavoritetDto>(result);

                _logger.LogInformation("User {UserId} added {CompanyType} company {CompanyId} to favorites",
                    userId, createDto.CompanyType, createDto.GetCompanyId());

                return CreatedAtAction(nameof(GetUserFavorites), new { id = favorite.Id }, favoriteDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding favorite for user {UserId}", GetUserId());
                return StatusCode(500, "Internal server error");
            }
        }



        /// <summary>
        /// Remove a favorite by ID
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin,User")]
        public async Task<IActionResult> RemoveFavorite(int id)
        {
            try
            {
                var userId = GetUserId();
                if (userId == null) return Unauthorized("User not authenticated");

                var favorite = await _context.Favorites
                    .FirstOrDefaultAsync(f => f.Id == id && f.UserId == userId);

                if (favorite == null)
                    return NotFound("Favorite not found or you don't have permission to delete it");

                _context.Favorites.Remove(favorite);
                await _context.SaveChangesAsync();

                _logger.LogInformation("User {UserId} removed favorite {FavoriteId}", userId, id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing favorite {Id} for user {UserId}", id, GetUserId());
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Check if a company is in user's favorites
        /// </summary>
        ///[HttpPost("check")]
        ///[Authorize(Roles = "SuperAdmin,User")]
        ///public async Task<ActionResult<bool>> CheckFavorite([FromBody] FavoriteCheckDto checkDto)
        ///{
        ///    try
        ///    {
        ///        var userId = GetUserId();
        ///        if (userId == null) return Unauthorized("User not authenticated");
         
        ///        if (!checkDto.IsValid())
        ///            return BadRequest("Invalid favorite data - provide exactly one company ID");
         
        ///        var exists = await _context.Favorites
        ///            .AnyAsync(f => f.UserId == userId &&
        ///                f.HotelCompanyId == checkDto.HotelCompanyId &&
        ///                f.FlightCompanyId == checkDto.FlightCompanyId &&
        ///                f.CarRentalCompanyId == checkDto.CarRentalCompanyId &&
        ///                f.TourCompanyId == checkDto.TourCompanyId &&
        ///                f.TourId == checkDto.TourId);
         
         
        ///        return Ok(exists);
        ///    }
        ///    catch (Exception ex)
        ///    {
        ///        _logger.LogError(ex, "Error checking favorite for user {UserId}", GetUserId());
        ///        return StatusCode(500, "Internal server error");
        ///    }
        ///}
        [HttpPost("check")]
        [Authorize(Roles = "SuperAdmin,User")]
        public async Task<ActionResult<bool>> CheckFavorite([FromBody] FavoriteCheckDto checkDto)
        {
            try
            {
                var userId = GetUserId();
                if (userId == null) return Unauthorized("User not authenticated");

                if (!checkDto.IsValid())
                    return BadRequest("Invalid favorite data - provide exactly one company ID");

                var dto = new CreateFavoriteDto
                {
                    HotelCompanyId = checkDto.HotelCompanyId,
                    FlightCompanyId = checkDto.FlightCompanyId,
                    CarRentalCompanyId = checkDto.CarRentalCompanyId,
                    TourCompanyId = checkDto.TourCompanyId,
                    TourId = checkDto.TourId,
                    CompanyType = checkDto.CompanyType
                };

                var exists = await CheckFavoriteExists(userId, dto);
                return Ok(exists);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking favorite for user {UserId}", GetUserId());
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get favorites count by company type for current user
        /// </summary>
        [HttpGet("count")]
        [Authorize(Roles = "SuperAdmin,User")]
        public async Task<ActionResult<Dictionary<string, int>>> GetFavoritesCount()
        {
            try
            {
                var userId = GetUserId();
                if (userId == null) return Unauthorized("User not authenticated");

                var counts = await _context.Favorites
                    .Where(f => f.UserId == userId)
                    .GroupBy(f => f.CompanyType.ToLower())
                    .Select(g => new { CompanyType = g.Key, Count = g.Count() })
                    .ToDictionaryAsync(x => x.CompanyType, x => x.Count);

                return Ok(counts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting favorites count for user {UserId}", GetUserId());
                return StatusCode(500, "Internal server error");
            }
        }

        // Helper Methods
        private string? GetUserId()
        {
            return  User.FindFirst("uid")?.Value;
        }

        private static bool IsValidCompanyType(string companyType)
        {
            var validTypes = new[] { "hotel", "flight", "carrental", "tour" };
            return validTypes.Contains(companyType.ToLower());
        }

        private async Task<bool> CheckFavoriteExists(string userId, CreateFavoriteDto dto)
        {
            var companyType = dto.CompanyType.ToLower();

            return companyType switch
            {
                "hotel" => dto.HotelCompanyId.HasValue &&
                           await _context.Favorites.AnyAsync(f =>
                               f.UserId == userId &&
                               f.CompanyType.ToLower() == "hotel" &&
                               f.HotelCompanyId == dto.HotelCompanyId),

                "flight" => dto.FlightCompanyId.HasValue &&
                            await _context.Favorites.AnyAsync(f =>
                                f.UserId == userId &&
                                f.CompanyType.ToLower() == "flight" &&
                                f.FlightCompanyId == dto.FlightCompanyId),

                "carrental" => dto.CarRentalCompanyId.HasValue &&
                               await _context.Favorites.AnyAsync(f =>
                                   f.UserId == userId &&
                                   f.CompanyType.ToLower() == "carrental" &&
                                   f.CarRentalCompanyId == dto.CarRentalCompanyId),

                "tour" => dto.TourId.HasValue &&
                          await _context.Favorites.AnyAsync(f =>
                              f.UserId == userId &&
                              f.CompanyType.ToLower() == "tour" &&
                              f.TourId == dto.TourId),

                _ => false
            };
        }

        private async Task<bool> VerifyCompanyExists(CreateFavoriteDto dto)
        {
            return dto.CompanyType.ToLower() switch
            {
                "hotel" => dto.HotelCompanyId.HasValue &&
                          await _context.HotelCompanies.AnyAsync(h => h.Id == dto.HotelCompanyId),
                "flight" => dto.FlightCompanyId.HasValue &&
                           await _context.FlightCompanies.AnyAsync(f => f.Id == dto.FlightCompanyId),
                "carrental" => dto.CarRentalCompanyId.HasValue &&
                              await _context.CarRentalCompanies.AnyAsync(c => c.Id == dto.CarRentalCompanyId),
                "tour" => dto.TourId.HasValue &&
                              await _context.Tours.AnyAsync(t => t.Id == dto.TourId),
                _ => false
            };
        }

        private async Task<Favoritet?> GetFavoriteWithIncludes(int id)
        {
            return await _context.Favorites
                .Include(f => f.User)
                .Include(f => f.HotelCompany)
                .Include(f => f.FlightCompany)
                .Include(f => f.CarRentalCompany)
                .Include(f => f.TourCompany)
                .Include(f => f.Tour)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);
        }
    }
}
