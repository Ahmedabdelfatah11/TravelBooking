namespace TravelBooking.APIs.DTOS
{
    public class ContactDto
    {
        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, EmailAddress, StringLength(150)]
        public string Email { get; set; }

        [StringLength(150)]
        public string Subject { get; set; }

        [Required, StringLength(2000)]
        public string Message { get; set; }
    }
}
