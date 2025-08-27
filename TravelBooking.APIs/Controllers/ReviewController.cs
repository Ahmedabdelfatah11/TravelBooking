using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TravelBooking.APIs.DTOS.Review;
using TravelBooking.Core.Models;
using TravelBooking.Repository.Data;

namespace TravelBooking.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ReviewController> _logger;

        public ReviewController(AppDbContext context, IMapper mapper, ILogger<ReviewController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get reviews for a specific company (public endpoint)
        /// </summary>
        [HttpGet("company")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetCompanyReviews(
            [FromQuery] string companyType,
            [FromQuery] int? hotelId = null,
            [FromQuery] int? flightId = null,
            [FromQuery] int? carRentalId = null,
            [FromQuery] int? tourId = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string sortBy = "newest")
        {
            try
            {
                if (!IsValidCompanyType(companyType))
                    return BadRequest($"Invalid company type: {companyType}");

                var query = _context.Reviews
                    .Include(r => r.User)
                    .Include(r => r.HotelCompany)
                    .Include(r => r.FlightCompany)
                    .Include(r => r.CarRentalCompany)
                    .Include(r => r.TourCompany)
                    .Where(r => r.CompanyType.ToLower() == companyType.ToLower() &&
                        r.HotelCompanyId == hotelId &&
                        r.FlightCompanyId == flightId &&
                        r.CarRentalCompanyId == carRentalId &&
                        r.TourCompanyId == tourId)
                    .AsNoTracking();

                // Apply sorting
                query = sortBy.ToLower() switch
                {
                    "oldest" => query.OrderBy(r => r.CreatedAt),
                    "rating_high" => query.OrderByDescending(r => r.Rating).ThenByDescending(r => r.CreatedAt),
                    "rating_low" => query.OrderBy(r => r.Rating).ThenByDescending(r => r.CreatedAt),
                    _ => query.OrderByDescending(r => r.CreatedAt) // newest (default)
                };

                var totalCount = await query.CountAsync();
                var reviews = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var reviewDtos = _mapper.Map<List<ReviewDto>>(reviews);

                Response.Headers.Add("X-Total-Count", totalCount.ToString());
                Response.Headers.Add("X-Page", page.ToString());
                Response.Headers.Add("X-Page-Size", pageSize.ToString());

                return Ok(reviewDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting company reviews for {CompanyType}", companyType);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get all reviews by current user
        /// </summary>
        [HttpGet("user")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetUserReviews(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var userId = GetUserId();
                if (userId == null) return Unauthorized("User not authenticated");

                var query = _context.Reviews
                    .Include(r => r.User)
                    .Include(r => r.HotelCompany)
                    .Include(r => r.FlightCompany)
                    .Include(r => r.CarRentalCompany)
                    .Include(r => r.TourCompany)
                    .Where(r => r.UserId == userId)
                    .OrderByDescending(r => r.CreatedAt)
                    .AsNoTracking();

                var totalCount = await query.CountAsync();
                var reviews = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var reviewDtos = _mapper.Map<List<ReviewDto>>(reviews);

                Response.Headers.Add("X-Total-Count", totalCount.ToString());

                return Ok(reviewDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user reviews for user {UserId}", GetUserId());
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Create a new review
        /// </summary>
        [HttpPost]
        //[Authorize]
        [Authorize(Roles = "SuperAdmin,User,CarRentalCompany")]
        public async Task<ActionResult<ReviewDto>> CreateReview([FromBody] CreateReviewDto createDto)
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

                // Check if user already reviewed this company
                var existingReview = await CheckReviewExists(userId, createDto);
                if (existingReview)
                    return Conflict(new { message = "You already Entered a Review Before" });


                // Verify company exists
                var companyExists = await VerifyCompanyExists(createDto);
                if (!companyExists)
                    return NotFound("Company not found");

                var review = _mapper.Map<Review>(createDto);
                review.UserId = userId;

                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();

                // Retrieve the review with includes for response
                var result = await GetReviewWithIncludes(review.Id);
                if (result == null)
                    return StatusCode(500, "Failed to retrieve created review");

                var reviewDto = _mapper.Map<ReviewDto>(result);

                _logger.LogInformation("User {UserId} created review for {CompanyType} company {CompanyId} with rating {Rating}",
                    userId, createDto.CompanyType, createDto.GetCompanyId(), createDto.Rating);

                return CreatedAtAction(nameof(GetCompanyReviews),
                    new { companyType = createDto.CompanyType }, reviewDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating review for user {UserId}", GetUserId());
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Update an existing review
        /// </summary>
        [HttpPut("{id}")]
        // [Authorize]
        [Authorize(Roles = "SuperAdmin,User,CarRentalCompany")]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] UpdateReviewDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userId = GetUserId();
                if (userId == null) return Unauthorized("User not authenticated");

                var review = await _context.Reviews
                    .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

                if (review == null)
                    return NotFound("Review not found or you don't have permission to update it");

                var oldRating = review.Rating;
                _mapper.Map(updateDto, review);
                await _context.SaveChangesAsync();

                _logger.LogInformation("User {UserId} updated review {ReviewId} from rating {OldRating} to {NewRating}",
                    userId, id, oldRating, updateDto.Rating);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating review {Id} for user {UserId}", id, GetUserId());
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Delete a review
        /// </summary>
        [HttpDelete("{id}")]
        //  [Authorize]
        [Authorize(Roles = "SuperAdmin,User,CarRentalCompany")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            try
            {
                var userId = GetUserId();
                if (userId == null) return Unauthorized("User not authenticated");

                var review = await _context.Reviews
                    .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

                if (review == null)
                    return NotFound("Review not found or you don't have permission to delete it");

                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();

                _logger.LogInformation("User {UserId} deleted review {ReviewId}", userId, id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting review {Id} for user {UserId}", id, GetUserId());
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get average rating for a company
        /// </summary>
        [HttpGet("average")]
        [AllowAnonymous]
        public async Task<ActionResult<double>> GetCompanyAverageRating(
            [FromQuery] string companyType,
            [FromQuery] int? hotelId = null,
            [FromQuery] int? flightId = null,
            [FromQuery] int? carRentalId = null,
            [FromQuery] int? tourId = null)
        {
            try
            {
                if (!IsValidCompanyType(companyType))
                    return BadRequest($"Invalid company type: {companyType}");

                var average = await _context.Reviews
                    .Where(r => r.CompanyType.ToLower() == companyType.ToLower() &&
                        r.HotelCompanyId == hotelId &&
                        r.FlightCompanyId == flightId &&
                        r.CarRentalCompanyId == carRentalId &&
                        r.TourCompanyId == tourId)
                    .AverageAsync(r => (double?)r.Rating) ?? 0;

                return Ok(Math.Round(average, 1));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting company average rating for {CompanyType}", companyType);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get total reviews count for a company
        /// </summary>
        [HttpGet("count")]
        [AllowAnonymous]
        public async Task<ActionResult<int>> GetCompanyReviewsCount(
            [FromQuery] string companyType,
            [FromQuery] int? hotelId = null,
            [FromQuery] int? flightId = null,
            [FromQuery] int? carRentalId = null,
            [FromQuery] int? tourId = null)
        {
            try
            {
                if (!IsValidCompanyType(companyType))
                    return BadRequest($"Invalid company type: {companyType}");

                var count = await _context.Reviews
                    .Where(r => r.CompanyType.ToLower() == companyType.ToLower() &&
                        r.HotelCompanyId == hotelId &&
                        r.FlightCompanyId == flightId &&
                        r.CarRentalCompanyId == carRentalId &&
                        r.TourCompanyId == tourId)
                    .CountAsync();

                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting company reviews count for {CompanyType}", companyType);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get comprehensive review statistics for a company
        /// </summary>
        [HttpGet("stats")]
        [AllowAnonymous]
        public async Task<ActionResult<ReviewStatsDto>> GetCompanyReviewStats(
            [FromQuery] string companyType,
            [FromQuery] int? hotelId = null,
            [FromQuery] int? flightId = null,
            [FromQuery] int? carRentalId = null,
            [FromQuery] int? tourCompanyId = null)
        {
            try
            {
                if (!IsValidCompanyType(companyType))
                    return BadRequest($"Invalid company type: {companyType}");

                var reviews = await _context.Reviews
                    .Include(r => r.User)
                    .Include(r => r.HotelCompany)
                    .Include(r => r.FlightCompany)
                    .Include(r => r.CarRentalCompany)
                    .Include(r => r.TourCompany)
                    .Where(r => r.CompanyType.ToLower() == companyType.ToLower() &&
                        r.HotelCompanyId == hotelId &&
                        r.FlightCompanyId == flightId &&
                        r.CarRentalCompanyId == carRentalId &&
                        r.TourCompanyId == tourCompanyId)
                    .AsNoTracking()
                    .ToListAsync();

                var stats = new ReviewStatsDto
                {
                    TotalReviews = reviews.Count,
                    AverageRating = reviews.Count > 0 ? Math.Round(reviews.Average(r => r.Rating), 1) : 0,
                    RatingDistribution = reviews
                        .GroupBy(r => r.Rating)
                        .ToDictionary(g => g.Key, g => g.Count()),
                    RecentReviews = _mapper.Map<List<ReviewDto>>(
                        reviews.OrderByDescending(r => r.CreatedAt).Take(5).ToList())
                };

                // Ensure all rating levels (1-5) are represented
                for (int i = 1; i <= 5; i++)
                {
                    if (!stats.RatingDistribution.ContainsKey(i))
                        stats.RatingDistribution[i] = 0;
                }

                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting company review stats for {CompanyType}", companyType);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Check if current user has reviewed a specific company
        /// </summary>
        [HttpPost("check")]
        //   [Authorize]
        [Authorize(Roles = "SuperAdmin,User,CarRentalCompany")]
        public async Task<ActionResult<bool>> CheckUserReview([FromBody] CreateReviewDto checkDto)
        {
            try
            {
                var userId = GetUserId();
                if (userId == null) return Unauthorized("User not authenticated");

                if (!checkDto.IsValid())
                    return BadRequest("Invalid review data - provide exactly one company ID");

                var exists = await CheckReviewExists(userId, checkDto);
                return Ok(exists);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking user review for user {UserId}", GetUserId());
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
            var validTypes = new[] { "hotel", "flight", "carrental", "tour" };
            return validTypes.Contains(companyType.ToLower());
        }

        private async Task<bool> CheckReviewExists(string userId, CreateReviewDto dto)
        {
            return await _context.Reviews
                .AnyAsync(r => r.UserId == userId &&
                    r.HotelCompanyId == dto.HotelCompanyId &&
                    r.FlightCompanyId == dto.FlightCompanyId &&
                    r.CarRentalCompanyId == dto.CarRentalCompanyId &&
                    r.TourCompanyId == dto.TourCompanyId);
        }

        private async Task<bool> VerifyCompanyExists(CreateReviewDto dto)
        {
            return dto.CompanyType.ToLower() switch
            {
                "hotel" => dto.HotelCompanyId.HasValue &&
                          await _context.HotelCompanies.AnyAsync(h => h.Id == dto.HotelCompanyId),
                "flight" => dto.FlightCompanyId.HasValue &&
                           await _context.FlightCompanies.AnyAsync(f => f.Id == dto.FlightCompanyId),
                "carrental" => dto.CarRentalCompanyId.HasValue &&
                              await _context.CarRentalCompanies.AnyAsync(c => c.Id == dto.CarRentalCompanyId),
                "tour" => dto.TourCompanyId.HasValue &&
                         await _context.TourCompanies.AnyAsync(t => t.Id == dto.TourCompanyId),
                _ => false
            };
        }

        private async Task<Review?> GetReviewWithIncludes(int id)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.HotelCompany)
                .Include(r => r.FlightCompany)
                .Include(r => r.CarRentalCompany)
                .Include(r => r.TourCompany)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}