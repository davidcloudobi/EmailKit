using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EmailService
{
    public interface IEmailSender
    {

        //  bool SendEmail(MessageDTO message, IFormFileCollection file);
        //  Task<bool> SendEmailAsync(MessageDTO message, IFormFileCollection  file);
        // Task<bool> SendEmailAsync(Message message);
        bool SendEmail(MessageDTO message);
        Task<bool> SendEmailAsync(MessageDTO message);
    }
}