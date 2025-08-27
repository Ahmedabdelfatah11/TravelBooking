namespace TravelBooking.APIs.DTOS.Tours
{
    public class TourUpdateDto:TourCreateDto
    {
        //public int Id { get; set; }
        public List<string>? DeletedImageUrls { get; set; }
    }
}
