namespace TravelBooking.APIs.DTOS.Favoritet
{
    public class CreateFavoriteDto
    {
        public int? HotelCompanyId { get; set; }
        public int? FlightCompanyId { get; set; }
        public int? CarRentalCompanyId { get; set; }
        public int? TourCompanyId { get; set; }
        public int? TourId { get; set; }

        [Required(ErrorMessage = "Company type is required")]
        [StringLength(50)]
        public string CompanyType { get; set; }

        // <summary>
        /// Validates that exactly one company ID is provided
        /// </summary>
        public bool IsValid()
        {
            var companyIds = new int?[] { HotelCompanyId, FlightCompanyId, CarRentalCompanyId, TourCompanyId, TourId };
            return companyIds.Count(id => id.HasValue) == 1;
        }

        /// <summary>
        /// Gets the company ID based on company type
        /// </summary>
        public int? GetCompanyId()
        {
            return CompanyType?.ToLower() switch
            {
                "hotel" => HotelCompanyId,
                "flight" => FlightCompanyId,
                "carrental" => CarRentalCompanyId,
                "tour" => TourId,
                _ => null
            };
        }
    }
}
