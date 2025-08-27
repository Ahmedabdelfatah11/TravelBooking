using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelBooking.Core.Repository.Contract;
using TravelBooking.Core.Models;
using TravelBooking.Core.Specifications;
using Stripe;
using Microsoft.EntityFrameworkCore;
using TravelBooking.Repository.Data;


[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly IGenericRepository<Booking> _bookingRepo;
    private readonly AppDbContext _context;
    private readonly IGenericRepository<Flight> _flightRepo;
    private readonly IGenericRepository<Car> _carRepo;
    public PaymentController(IPaymentService paymentService, IGenericRepository<Booking> bookingRepo,
        AppDbContext context, IGenericRepository<Flight> flightRepo, IGenericRepository<Car> carRepo)
    {
        _paymentService = paymentService;
        _bookingRepo = bookingRepo;
        _context = context;
        _flightRepo = flightRepo;
        _carRepo = carRepo;

    }

    // Endpoint to create a Stripe PaymentIntent for a given booking
    [HttpPost("create-intent/{bookingId}")]
    public async Task<IActionResult> CreatePaymentIntent(int bookingId)
    {
        try
        {
            // Call the service to create the PaymentIntent
            var booking = await _paymentService.CreatePaymentIntent(bookingId);

            // If booking is invalid, already paid, or payment creation failed
            if (booking == null)
                return BadRequest("Invalid booking, already paid, or payment failed.");

            // Return the clientSecret needed for Stripe and the total price
            return Ok(new
            {
                clientSecret = booking.ClientSecret,
                total = booking.TotalPrice
            });
        }
        catch (Exception)
        {
            // If an unexpected error occurs
            return StatusCode(500, "An error occurred while processing the payment.");
        }
    }

    //    confirm a payment after Stripe marks it as succeeded
    [HttpPost("confirm-payment")]
    public async Task<IActionResult> ConfirmPayment([FromBody] ConfirmPaymentRequest request)
    {
        try
        {
            // Retrieve payment information from Stripe using the PaymentIntentId
            var service = new PaymentIntentService();
            var paymentIntent = await service.GetAsync(request.PaymentIntentId);

            // Only proceed if payment succeeded
            if (paymentIntent.Status == "succeeded")
            {
                // Find booking that matches the PaymentIntentId
                var spec = new BookingByPaymentIntentIdSpecification(paymentIntent.Id);
                var booking = await _bookingRepo.GetWithSpecAsync(spec);

                if (booking != null)
                {
                    // Update booking status to confirmed
                    booking.Status = Status.Confirmed;
                    foreach (var bt in booking.BookingTickets)
                    {
                        var ticket = bt.Ticket;

                        if (ticket.AvailableQuantity < bt.Quantity)
                            return BadRequest($"Not enough '{ticket.Type}' tickets left.");

                        ticket.AvailableQuantity -= bt.Quantity;

                        if (ticket.AvailableQuantity == 0)
                            ticket.IsActive = false;

                        _context.TourTickets.Update(ticket);
                    }

                    if (booking.CarId != null)
                    {
                        var car = await _carRepo.GetAsync(booking.CarId.Value);
                        if (car == null)
                            return NotFound("Car not found for booking.");

                        car.IsAvailable = false;
                        await _carRepo.Update(car);
                    }



                    if (booking.FlightId != null)
                    {
                        var flight = await _flightRepo.GetAsync(booking.FlightId.Value);
                        if (flight == null)
                            return NotFound("Flight not found for booking.");

                        switch (booking.SeatClass)
                        {
                            case SeatClass.Economy:
                                if (flight.EconomySeats <= 0)
                                    return BadRequest("No economy seats left.");
                                flight.EconomySeats--;
                                break;
                            case SeatClass.Business:
                                if (flight.BusinessSeats <= 0)
                                    return BadRequest("No business seats left.");
                                flight.BusinessSeats--;
                                break;
                            case SeatClass.FirstClass:
                                if (flight.FirstClassSeats <= 0)
                                    return BadRequest("No first class seats left.");
                                flight.FirstClassSeats--;
                                break;
                        }

                        await _flightRepo.Update(flight);
                    }
                    if (booking.RoomId != null)
                    {
                        var overlappingConfirmed = await _bookingRepo.GetAllAsync(b =>
                    b.RoomId == booking.RoomId &&
                     b.Status == Status.Confirmed &&
                       b.StartDate < booking.EndDate &&
                       booking.StartDate < b.EndDate
                        );

                        if (overlappingConfirmed.Any())
                            return BadRequest("Room is no longer available for the selected dates.");
                    }
                    // ✅ Update or create Payment record
                    if (booking.Payment != null)
                    {
                        // If payment record exists → update it
                        booking.Payment.PaymentStatus = PaymentStatus.Paid;
                        booking.Payment.TransactionId = paymentIntent.Id;
                        booking.Payment.PaymentDate = DateTime.UtcNow;
                    }
                    else
                    {
                        // If payment record does not exist → create new
                        booking.Payment = new Payment
                        {
                            BookingId = booking.Id,
                            Amount = booking.TotalPrice,
                            PaymentMethod = "Card",
                            PaymentStatus = PaymentStatus.Paid,
                            TransactionId = paymentIntent.Id,
                            PaymentDate = DateTime.UtcNow
                        };
                    }

                    // Save changes to the booking (and related payment)
                    await _bookingRepo.Update(booking);

                    // Return success response
                    return Ok(new
                    {
                        message = "Payment confirmed successfully",
                        bookingId = booking.Id,
                        paymentStatus = booking.Payment?.PaymentStatus.ToString(),
                        transactionId = booking.Payment?.TransactionId
                    });
                }
                else
                {
                    // No booking found for this PaymentIntentId
                    Console.WriteLine("Booking not found for PaymentIntentId: " + paymentIntent.Id);
                    return NotFound("Booking not found");
                }
            }
            else
            {
                // Stripe says payment was not successful
                return BadRequest("Payment was not successful");
            }
        }
        catch (Exception ex)
        {
            // Log error and return server error
            Console.WriteLine("Error confirming payment: " + ex.Message);
            return StatusCode(500, "Error confirming payment: " + ex.Message);
        }
    }
}

// DTO for confirm-payment request body
public class ConfirmPaymentRequest
{
    public string PaymentIntentId { get; set; } = string.Empty;
}