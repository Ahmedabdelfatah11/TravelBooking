namespace TravelBooking.APIs.DTOS.Favoritet
{
    public class FavoriteCheckDto
    {
        public string CompanyType { get; set; } = string.Empty;
        public int? HotelCompanyId { get; set; }
        public int? FlightCompanyId { get; set; }
        public int? CarRentalCompanyId { get; set; }
        public int? TourCompanyId { get; set; }

        public bool IsValid()
        {
            var companyIds = new[] { HotelCompanyId, FlightCompanyId, CarRentalCompanyId, TourCompanyId };
            return companyIds.Count(id => id.HasValue) == 1;
        }
    }
}
