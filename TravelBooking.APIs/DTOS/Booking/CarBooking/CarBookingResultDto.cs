﻿namespace TravelBooking.APIs.DTOS.Booking.CarBooking
{
    public class CarBookingResultDto
    {
        public int BookingId { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CarId { get; set; }
        public string CarModel { get; set; }  // Or any other car info you want to return
    }

}
