using System.Threading.Tasks;

namespace VSAND.Services.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string senderName, string email, string subject, string message);
    }
}
