namespace TravelBooking.APIs.DTOS.Admins
{
    public class AssignAdminToCompanyDto
    {
        public string UserId { get; set; }
        public int? CompanyId { get; set; }
        public string CompanyType { get; set; } // "Hotel", "Flight", "CarRental", "Tour"
    }
}
