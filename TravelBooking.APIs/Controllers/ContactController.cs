
using Microsoft.AspNetCore.Mvc;
using TravelBooking.APIs.DTOS;
using TravelBooking.Core.Interfaces_Or_Repository;
using TravelBooking.Core.Models;
using TravelBooking.Repository.Data;

namespace ContactUsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IEmailService _emailService;

        public ContactController(AppDbContext db, IEmailService emailService)
        {
            _db = db;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] ContactDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // هنا تجيب الإيميل بتاع اليوزر من الـ JWT أو الـ Claims
            var loggedInUserEmail = User?.Identity?.Name ?? dto.Email;

            var message = new ContactMessage
            {
                Name = dto.Name,
                Email = loggedInUserEmail,
                Subject = dto.Subject,
                Message = dto.Message
            };

            _db.ContactMessages.Add(message);
            await _db.SaveChangesAsync();

            try
            {
                await _emailService.SendContactNotificationAsync(message, loggedInUserEmail);
            }
            catch
            {
                // تجاهل خطأ الإيميل
            }

            return Ok(new { message = "Message sent successfully!" });
        }

    }
}
