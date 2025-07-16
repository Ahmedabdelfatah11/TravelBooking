using Jwt.Settings;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace TravelBooking.Core.Models.Services
{
    public class EmailService : IEmailSender
    {
        private readonly IOptions<MailSettings> _mailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSettings.Value.Mail),
                Subject = subject
            };
            message.To.Add(MailboxAddress.Parse(email));

            // Create the body of the email
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlMessage
            };
            message.Body = bodyBuilder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            _logger.LogInformation("Sending email to {Email} with subject {Subject}", email, subject);

            await smtp.ConnectAsync(_mailSettings.Value.Host, _mailSettings.Value.Port, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_mailSettings.Value.Mail, _mailSettings.Value.Password);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
    }
}
