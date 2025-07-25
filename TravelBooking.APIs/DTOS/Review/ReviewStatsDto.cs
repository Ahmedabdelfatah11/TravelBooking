namespace TravelBooking.APIs.DTOS.Review
{
    public class ReviewStatsDto
    {

            public double AverageRating { get; set; }
            public int TotalReviews { get; set; }
            public Dictionary<int, int> RatingDistribution { get; set; } = new();
            public List<ReviewDto> RecentReviews { get; set; } = new();
        
    }
}
