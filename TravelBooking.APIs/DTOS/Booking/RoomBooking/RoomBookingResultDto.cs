﻿namespace TravelBooking.APIs.DTOS.Booking.RoomBooking
{
    public class RoomBookingResultDto
    {
        public int BookingId { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int RoomId { get; set; }
        public string RoomType { get; set; }
    }

}
