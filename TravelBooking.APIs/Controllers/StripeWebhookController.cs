using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.IO;
using System.Threading.Tasks;
using TravelBooking.Core.Models;
using TravelBooking.Core.Repository.Contract;
using TravelBooking.Core.Specifications;
using Microsoft.Extensions.Configuration;

[Route("api/[controller]")]
[ApiController]
public class StripeWebhookController : ControllerBase
{
    private readonly IGenericRepository<Booking> _bookingRepo;
    private readonly string _webhookSecret;

    public StripeWebhookController(IGenericRepository<Booking> bookingRepo, IConfiguration configuration)
    {
        _bookingRepo = bookingRepo;
        _webhookSecret = configuration["StripeSettings:WebhookSecret"];
    }

    [HttpPost]
    public async Task<IActionResult> Post()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

        if (!Request.Headers.TryGetValue("Stripe-Signature", out var stripeSignatureHeader)
            || string.IsNullOrEmpty(stripeSignatureHeader))
        {
            Console.WriteLine("Missing Stripe-Signature header");
            return BadRequest("Missing Stripe-Signature header");
        }

        try
        {
            var stripeEvent = EventUtility.ConstructEvent(
                json,
                stripeSignatureHeader,
                _webhookSecret
            );

            if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)

            {
                var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;

                // جلب الـ booking مع الـ payment
                var spec = new BookingByPaymentIntentIdSpecification(paymentIntent.Id);
                var booking = await _bookingRepo.GetWithSpecAsync(spec);

                if (booking != null && booking.Payment != null)
                {
                    booking.Status = Status.Confirmed;
                    booking.Payment.PaymentStatus = PaymentStatus.Paid;
                    booking.Payment.TransactionId = paymentIntent.Id;
                    booking.Payment.PaymentDate = DateTime.UtcNow;

                    await _bookingRepo.Update(booking);
                }

            }

            return Ok();
        }
        catch (StripeException e)
        {
            Console.WriteLine("Stripe webhook error: " + e.Message);
            return BadRequest();
        }
    }
}
