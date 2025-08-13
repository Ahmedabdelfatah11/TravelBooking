 
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using TravelBooking.Core.Interfaces_Or_Repository;
using TravelBooking.Core.Models;

namespace ContactUsAPI.Services
{
    public class SmtpEmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public SmtpEmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendContactNotificationAsync(ContactMessage message, string loggedInUserEmail)
        {
            var smtp = _config.GetSection("Smtp");
            var host = smtp["Host"];
            var port = int.Parse(smtp["Port"]);
            var user = smtp["User"];
            var pass = smtp["Pass"];
            var to = smtp["To"];

            var mail = new MailMessage();
            mail.From = new MailAddress(user, "Website Contact");
            mail.To.Add(to);
            mail.ReplyToList.Add(new MailAddress(loggedInUserEmail)); // هنا بنخلي الرد يروح للي عامل login
            mail.Subject = $"New contact: {message.Subject ?? "No subject"}";
            mail.Body = $"Name: {message.Name}\nEmail: {loggedInUserEmail}\n\n{message.Message}";

            using var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(user, pass),
                EnableSsl = true
            };

            await client.SendMailAsync(mail);
        }

    }
}
