using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace VSAND.Services.Email
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713

    public class EmailSender : IEmailSender
    {
        private IConfiguration _configuration { get; set; }
        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendEmailAsync(string senderName, string email, string subject, string message)
        {
            SmtpClient client = new SmtpClient(_configuration["SMTP_HOST"])
            {
                UseDefaultCredentials = false,
                Port = int.Parse(_configuration["SMTP_PORT"]),
                Credentials = new NetworkCredential(_configuration["SMTP_USER"], _configuration["SMTP_PASSWORD"])
            };

            MailMessage mailMessage = new MailMessage
            {
                IsBodyHtml = true,
                From = new MailAddress(_configuration["SMTP_USER"], senderName),
                Body = message,
                Subject = subject,
            };
            mailMessage.To.Add(email);

            return client.SendMailAsync(mailMessage);
        }
    }
}