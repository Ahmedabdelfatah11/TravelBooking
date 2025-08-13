namespace TravelBooking.Core.Models
{
    public class TourQuestion : BaseEntity
    {
        public string QuestionText { get; set; } = string.Empty;
        public string AnswerText { get; set; } = string.Empty;
        public int TourId { get; set; }
        public Tour? Tour { get; set; }
    }
}