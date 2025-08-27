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
        /// SuperAdmin: Get all favorites from all users with pagination and filtering
        /// </summary>
        [HttpGet("admin/all")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<IEnumerable<FavoritetDto>>> GetAllUsersFavorites(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? userId = null,
            [FromQuery] string? companyType = null,
            [FromQuery] string? searchTerm = null)
        {
            try
            {
                var query = _context.Favorites
                    .Include(f => f.User)
                    .Include(f => f.HotelCompany)
                    .Include(f => f.TourCompany)
                    .Include(f => f.Tour)
                    .AsNoTracking()
                    .AsQueryable();

                // Filter by specific user if provided
                if (!string.IsNullOrEmpty(userId))
                {
                    query = query.Where(f => f.UserId == userId);
                }

                // Filter by company type if provided
                if (!string.IsNullOrEmpty(companyType) && companyType.ToLower() != "all")
                {
                    query = query.Where(f => f.CompanyType.ToLower() == companyType.ToLower());
                }

                // Search functionality
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    var search = searchTerm.ToLower();
                    query = query.Where(f =>
                        f.User.UserName.ToLower().Contains(search) ||
                        f.User.Email.ToLower().Contains(search) ||
                        (f.HotelCompany != null && f.HotelCompany.Name.ToLower().Contains(search)) ||
                        (f.Tour != null && f.Tour.Name.ToLower().Contains(search))
                    );
                }

                query = query.OrderByDescending(f => f.CreatedAt);

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
                _logger.LogError(ex, "Error getting all users favorites");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// SuperAdmin: Get favorites for a specific user
        /// </summary>
        [HttpGet("admin/user/{userId}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<IEnumerable<FavoritetDto>>> GetUserFavoritesByAdmin(
            string userId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? companyType = null)
        {
            try
            {
                // Verify user exists
                var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
                if (!userExists)
                    return NotFound("User not found");

                var query = _context.Favorites
                    .Include(f => f.User)
                    .Include(f => f.HotelCompany)
                    .Include(f => f.TourCompany)
                    .Include(f => f.Tour)
                    .Where(f => f.UserId == userId)
                    .AsNoTracking();

                // Filter by company type if provided
                if (!string.IsNullOrEmpty(companyType) && companyType.ToLower() != "all")
                {
                    query = query.Where(f => f.CompanyType.ToLower() == companyType.ToLower());
                }

                query = query.OrderByDescending(f => f.CreatedAt);

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
                _logger.LogError(ex, "Error getting user favorites for user {UserId}", userId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// SuperAdmin: Get favorites statistics
        /// </summary>
        [HttpGet("admin/stats")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<object>> GetFavoritesStats()
        {
            try
            {
                var totalFavorites = await _context.Favorites.CountAsync();

                var userStats = await _context.Favorites
                    .GroupBy(f => f.UserId)
                    .Select(g => new { UserId = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Count)
                    .Take(10)
                    .ToListAsync();

                var companyTypeStats = await _context.Favorites
                    .GroupBy(f => f.CompanyType.ToLower())
                    .Select(g => new { CompanyType = g.Key, Count = g.Count() })
                    .ToListAsync();

                var recentActivity = await _context.Favorites
                    .Include(f => f.User)
                    .OrderByDescending(f => f.CreatedAt)
                    .Take(10)
                    .Select(f => new
                    {
                        f.Id,
                        f.UserId,
                        f.User.UserName,
                        f.User.Email,
                        f.CompanyType,
                        f.CreatedAt
                    })
                    .ToListAsync();

                return Ok(new
                {
                    totalFavorites,
                    userStats,
                    companyTypeStats,
                    recentActivity
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting favorites statistics");
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
                    return BadRequest($"Invalid company type: {companyType}. Valid types are: hotel, tour");

                var query = _context.Favorites
                    .Include(f => f.User)
                    .Include(f => f.HotelCompany)
                    .Include(f => f.Tour)
                    .Include(f => f.Tour.TourCompany)
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
        /// Remove a favorite by ID - Enhanced for SuperAdmin
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
                    .FirstOrDefaultAsync(r => r.Id == id &&
                        (User.IsInRole("SuperAdmin") || r.UserId == userId));

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
        /// SuperAdmin: Bulk delete favorites
        /// </summary>
        [HttpDelete("admin/bulk")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> BulkRemoveFavorites([FromBody] List<int> favoriteIds)
        {
            try
            {
                if (favoriteIds == null || !favoriteIds.Any())
                    return BadRequest("No favorite IDs provided");

                var favorites = await _context.Favorites
                    .Where(f => favoriteIds.Contains(f.Id))
                    .ToListAsync();

                if (!favorites.Any())
                    return NotFound("No favorites found with provided IDs");

                _context.Favorites.RemoveRange(favorites);
                var deletedCount = await _context.SaveChangesAsync();

                _logger.LogInformation("SuperAdmin bulk deleted {Count} favorites", deletedCount);

                return Ok(new { deletedCount, message = $"Successfully deleted {deletedCount} favorites" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error bulk removing favorites");
                return StatusCode(500, "Internal server error");
            }
        }

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
            return User.FindFirst("uid")?.Value;
        }

        private static bool IsValidCompanyType(string companyType)
        {
            var validTypes = new[] { "hotel", "tour" };
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
                .Include(f => f.Tour)
                .Include(f => f.Tour.TourCompany)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);
        }
    }
}