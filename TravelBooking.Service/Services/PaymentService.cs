using Microsoft.Extensions.Configuration;
using Stripe;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravelBooking.Core.Models;
using TravelBooking.Core.Repository.Contract;
using TravelBooking.Core.Specifications.BookingWithIncludesSpecification;

namespace TravelBooking.Service.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IGenericRepository<Booking> _bookingRepo;
        private readonly IGenericRepository<Payment> _paymentRepo;  

        public PaymentService(
            IConfiguration configuration,
            IGenericRepository<Booking> bookingRepo,
            IGenericRepository<Payment> paymentRepo)  
        {
            _configuration = configuration;
            _bookingRepo = bookingRepo;
            _paymentRepo = paymentRepo;  
        }

        public async Task<Booking?> CreatePaymentIntent(int bookingId)
        {
            // Set Stripe API key from configuration
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];

            // Get booking details including related entities
            var spec = new BookingWithIncludesSpecification(bookingId);
            var booking = await _bookingRepo.GetWithSpecAsync(spec);

            if (booking == null)
                return null;

            // If booking is already paid, no need to create a new PaymentIntent
            if (booking.Payment != null && booking.Payment.PaymentStatus == PaymentStatus.Paid)
            {
                return null;
            }

            // Calculate total amount for the booking
            decimal total = CalculateTotalPrice(booking);
            if (total <= 0)
                return null;

            // If booking already has a PaymentIntent, retrieve it from Stripe
            if (!string.IsNullOrEmpty(booking.PaymentIntentId))
            {
                var existing = await new PaymentIntentService().GetAsync(booking.PaymentIntentId);
                booking.ClientSecret = existing.ClientSecret;
                return booking;
            }

            // Create a new PaymentIntent in Stripe
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(total * 100), // Stripe expects amount in cents
                Currency = "usd",
                PaymentMethodTypes = new List<string> { "card" },
                Metadata = new Dictionary<string, string>
                {
                    { "BookingId", booking.Id.ToString() },
                    { "CustomerId", booking.UserId ?? "" }
                }
            };

            var service = new PaymentIntentService();
            var intent = await service.CreateAsync(options);

            // Save PaymentIntent ID and ClientSecret in the booking
            booking.PaymentIntentId = intent.Id;
            booking.ClientSecret = intent.ClientSecret;

            // Create or update Payment entity in database
            if (booking.Payment == null)
            {
                // Create new Payment record
                var payment = new Payment
                {
                    BookingId = booking.Id,
                    Amount = total,
                    PaymentDate = DateTime.UtcNow,
                    PaymentStatus = PaymentStatus.Pending,
                    PaymentMethod = "card",
                    TransactionId = intent.Id
                };

                // Link Payment to Booking
                await _paymentRepo.AddAsync(payment);
                booking.Payment = payment;
            }
            else
            {
                // Update existing Payment record
                booking.Payment.Amount = total;
                booking.Payment.PaymentStatus = PaymentStatus.Pending;
                booking.Payment.TransactionId = intent.Id;
                booking.Payment.PaymentDate = DateTime.UtcNow;

                await _paymentRepo.Update(booking.Payment);
            }

            // Save changes to booking (and related payment)
            await _bookingRepo.Update(booking);

            return booking;
        }

        // Helper method to calculate total booking price
        private decimal CalculateTotalPrice(Booking booking)
        {
            decimal total = 0;
            int duration = (int)(booking.EndDate - booking.StartDate).TotalDays;
            if (duration <= 0) duration = 1; // Minimum duration is 1 day

            switch (booking.BookingType)
            {
                case BookingType.Room:
                    total = (booking.Room?.Price ?? 0) * duration;
                    break;

                case BookingType.Car:
                    total = (booking.Car?.Price ?? 0) * duration;
                    break;

                case BookingType.Flight:
                    total = booking.SeatClass switch
                    {
                        SeatClass.Economy => booking.Flight?.EconomyPrice ?? 0,
                        SeatClass.Business => booking.Flight?.BusinessPrice ?? 0,
                        SeatClass.FirstClass => booking.Flight?.FirstClassPrice ?? 0,
                        _ => 0
                    };
                    break;

                case BookingType.Tour:
                    total = booking.BookingTickets?.Sum(bt => bt.Ticket.Price * bt.Quantity) ?? 0;
                    break;

            }

            return total;
        }
    }
}
