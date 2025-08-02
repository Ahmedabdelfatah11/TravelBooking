using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelBooking.Core.Repository.Contract;
//[Authorize]
[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost("create-intent/{bookingId}")]
    public async Task<IActionResult> CreatePaymentIntent(int bookingId)
    {
        try
        {
            var booking = await _paymentService.CreatePaymentIntent(bookingId);

            if (booking == null)
                return BadRequest("Invalid booking, already paid, or payment failed.");

            return Ok(new
            {
                clientSecret = booking.ClientSecret,
                total = booking.TotalPrice
            });
        }
        catch (Exception ex)
        {
         
            return StatusCode(500, "An error occurred while processing the payment.");
        }
    }
}