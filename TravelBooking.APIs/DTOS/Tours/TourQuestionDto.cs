namespace TravelBooking.APIs.DTOS.Tours
{
    public class TourQuestionDto
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public string AnswerText { get; set; }
        public int TourId { get; set; }
    }
}
