using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelBooking.Core.Repository.Contract;
using TravelBooking.Core.Models;
using TravelBooking.Core.Specifications;
using Stripe;


[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly IGenericRepository<Booking> _bookingRepo;

    public PaymentController(IPaymentService paymentService, IGenericRepository<Booking> bookingRepo)
    {
        _paymentService = paymentService;
        _bookingRepo = bookingRepo;
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
