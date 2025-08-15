namespace TravelBooking.APIs.DTOS.Favoritet
{
    public class FavoriteCheckDto
    {
        public string CompanyType { get; set; } = string.Empty;
        public int? HotelCompanyId { get; set; }
       
        public int? TourCompanyId { get; set; }
        public int? TourId { get; set; }

        public bool IsValid()
        {
            var companyIds = new[] { HotelCompanyId, TourCompanyId,TourId };
            return companyIds.Count(id => id.HasValue) == 1;
        }
    }
}
